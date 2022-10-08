using System.Collections;
using UnityEngine;

public class InputGlobalController : MonoBehaviour
{
    private static Axis axis = new();

    public static Axis GetAxis()
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
}
}