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
    private StartupMenuController startupMenu;


    void Start()
    {
        resumeText = GameObject.Find("Resume Text").GetComponent<TextMeshProUGUI>();
        exitText = GameObject.Find("Exit Text").GetComponent<TextMeshProUGUI>();
        startupMenu = GameObject.Find("Startup Menu").GetComponent<StartupMenuController>();
        animator = GetComponent<Animator>();

        updateText();
    }

    void Update()
    {
        if (!startupMenu.isVisible)
        {
            navigate();
        }
    }

    public void updatePauseTimeout()
    {
        lastPauseAt = Time.realtimeSinceStartup;
    }

    public void toggleVisibility()
    {
       if (Time.realtimeSinceStartup - lastPauseAt >= togglePauseTimeout)
        {
            lastPauseAt = Time.realtimeSinceStartup;
            setVisibility(!isVisible);
        }
    }

    void navigate()
    {
        var axisY = 0;
        var isVisiblityToggle = false;
        var isSubmit = false;

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.leftStick.y.ReadValue() > 0.2f || gamepad.dpad.y.ReadValue() == 1)
            {
                axisY = -1;
            }

            if (gamepad.leftStick.y.ReadValue() < -0.2f || gamepad.dpad.y.ReadValue() == -1)
            {
                axisY = 1;
            }

            if (gamepad.startButton.ReadValue() != 0)
            {
                isVisiblityToggle = true;
            }

            if (gamepad.aButton.ReadValue() != 0)
            {
                isSubmit = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isVisiblityToggle = true;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            axisY = -1;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            axisY = 1;
        }

        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space))
        {
            isSubmit = true;
        }

        if (isVisiblityToggle)
        {
            toggleVisibility();
        }

        if (isVisible)
        {
            if (axisY != 0)
            {
                selectOption(axisY);
            }

            if (isSubmit)
            {
                submit();
            }
        }
    }
    
    void setVisibility(bool newVisibility)
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

    void selectOption(int increment)
    {
        if (
            Time.realtimeSinceStartup - lastValueAt >= toggleValueTimeout 
            && selectedOption + increment >= 0 
            && selectedOption + increment < optionsCount
        )
        {
            lastValueAt = Time.realtimeSinceStartup;
            selectedOption = selectedOption + increment;
            updateText();
            toggleSound.Play();
        }
    }

    void submit()
    {
        if (selectedOption == 0)
        {
            setVisibility(false);
        } else
        {
            Application.Quit();
        }
    }

    void updateText()
    {
        if (selectedOption == 1)
        {
            resumeText.text = "Resume";
            resumeText.fontStyle = FontStyles.Normal;
            exitText.text = "<b>Exit Game</b>";
            exitText.fontStyle = FontStyles.Underline;
        }
        else
        {
            resumeText.text = "<b>Resume</b>";
            resumeText.fontStyle = FontStyles.Underline;
            exitText.text = "Exit Game";
            exitText.fontStyle = FontStyles.Normal;
        }
    }
}
