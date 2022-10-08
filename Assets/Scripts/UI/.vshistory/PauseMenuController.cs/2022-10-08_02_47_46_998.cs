using System;
using TMPro;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public TextMeshProUGUI resumeText;
    public TextMeshProUGUI exitText;

    // sounds
    public AudioSource toggleSound;
    public AudioSource showSound;
    public AudioSource hideSound;

    // params
    private bool isVisible = false;
    private bool isDisabled = true;

    // animation
    private Animator animator;

    // togglers
    private int selectedOption = 0;
    private int optionsCount = 2;
    private float lastPauseAt = 0;
    private float lastValueAt = 0;
    private float togglePauseTimeout = 0.25f;
    private float toggleValueTimeout = 0.2f;

    void Start()
    {
        ActionsContoller.OnEndGame += Disable;
        ActionsContoller.OnStartGame += Enable;

        animator = GetComponent<Animator>();
        UpdateText();
    }

    void Update()
    {
        if (!isDisabled)
        {
            Navigate();
        }
    }

    private void Disable()
    {
        isDisabled = true;
    }

    private void Enable()
    {
        isDisabled = false;
        UpdatePauseTimeout();
    }

    void UpdatePauseTimeout()
    {
        lastPauseAt = Time.unscaledTime;
    }

    void ToggleVisibility()
    {
        if (Time.unscaledTime - lastPauseAt >= togglePauseTimeout)
        {
            SetVisibility(!isVisible);
            UpdatePauseTimeout();
        }
    }

    void Navigate()
    {
        var axis = InputGlobalController.GetUpdatedAxis();

        if (axis.GetPause() != 0)
        {
            ToggleVisibility();
        }

        if (isVisible)
        {
            if (axis.GetY() != 0)
            {
                SelectOption((int) axis.GetY());
            }

            if (axis.GetAction() != 0)
            {
                Submit();
            }
        }
    }
    
    void SetVisibility(bool newVisibility)
    {
        isVisible = newVisibility;

        if (isVisible)
        {
            Time.timeScale = 0;
            animator.Play("Pause Menu Show");
            showSound.Play();
        }
        else
        {
            Time.timeScale = 1;
            animator.Play("Pause Menu Hide");
            hideSound.Play();
        }
    }

    void SelectOption(int increment)
    {
        if (
            Time.unscaledTime - lastValueAt >= toggleValueTimeout 
            && selectedOption + increment >= 0 
            && selectedOption + increment < optionsCount
        )
        {
            lastValueAt = Time.realtimeSinceStartup;
            selectedOption += increment;
            UpdateText();
            toggleSound.Play();
        }
    }

    void Submit()
    {
        switch (selectedOption)
        {
            case 0:
                SetVisibility(false);
                break;
            case 1:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    void UpdateText()
    {
        switch (selectedOption)
        {
            case 0:
                resumeText.text = "<b>Resume</b>";
                resumeText.fontStyle = FontStyles.Underline;
                exitText.text = "Exit Game";
                exitText.fontStyle = FontStyles.Normal;
                break;
            case 1:
                resumeText.text = "Resume";
                resumeText.fontStyle = FontStyles.Normal;
                exitText.text = "<b>Exit Game</b>";
                exitText.fontStyle = FontStyles.Underline;
                break;
            default:
                break;
        }
    }
}
