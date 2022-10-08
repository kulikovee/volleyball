using System.Collections.Generic;
using UnityEngine;

public class InputNumpadController : MonoBehaviour
{
    private readonly Axis axis = new();
    private readonly float keyboardMovementFactor = 0.7f;
    public static List<KeyCode> KEYBOARD_ARROWS_KEYS = new()
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad8,
        KeyCode.Keypad0,
    };

    public Axis GetAxis()
    {
        return axis;
    }

    public static bool IsPressed()
    {
        var isPressed = false;

        KEYBOARD_ARROWS_KEYS.ForEach((keyCode) => {
            if (Input.GetKeyDown(keyCode))
            {
                isPressed = true;
            }
        });

        return isPressed;
    }
}
