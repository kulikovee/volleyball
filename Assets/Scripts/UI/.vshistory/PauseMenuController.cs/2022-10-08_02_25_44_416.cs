using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    public bool isVisible = false;

    // sounds
    public AudioSource toggleSound;
    public AudioSource showSound;
    public AudioSource hideSound;

    // animation
    private Animator animator;

    // togglers
    private int selectedOption = 0;
    private int optionsCount = 2;
    private float lastPauseAt = 0;
    private float lastValueAt = 0;
    private float togglePauseTimeout = 0.25f;
    private float toggleValueTimeout = 0.2f;

    // links
    private TextMeshProUGUI resumeText;
    private TextMeshProUGUI exitText;

    void Start()
    {
        resumeText = GameObject.Find("Resume Text").GetComponent<TextMeshProUGUI>();
        exitText = GameObject.Find("Exit Text").GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();

        UpdateText();
    }

    void Update()
    {
        Navigate();
    }

    public void UpdatePauseTimeout()
    {
        lastPauseAt = Time.realtimeSinceStartup;
    }

    public void ToggleVisibility()
    {
       if (Time.realtimeSinceStartup - lastPauseAt >= togglePauseTimeout)
        {
            lastPauseAt = Time.realtimeSinceStartup;
            SetVisibility(!isVisible);
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
                SelectOption((int) -axis.GetY());
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
            Time.realtimeSinceStartup - lastValueAt >= toggleValueTimeout 
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
