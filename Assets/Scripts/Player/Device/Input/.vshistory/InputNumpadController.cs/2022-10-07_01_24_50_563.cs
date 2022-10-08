using UnityEngine;

public class InputNumpadController : MonoBehaviour
{
    private readonly Axis axis = new();
    private readonly float keyboardMovementFactor = 0.7f;

    void Update()
    {
        bool isUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Keypad8);
        bool isDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Keypad5);
        bool isRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6);
        bool isLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4);
        bool isJump = Input.GetKey(KeyCode.Keypad0)
            || Input.GetKey(KeyCode.RightControl)
            || Input.GetKey(KeyCode.RightCommand)
            || Input.GetKey(KeyCode.RightApple)
            || Input.GetKey(KeyCode.RightShift);

        axis.SetX((isLeft ? -1 : (isRight ? 1 : 0)) * keyboardMovementFactor);
        axis.SetY((isDown ? -1 : (isUp ? 1 : 0)) * keyboardMovementFactor);
        axis.SetJump(isJump ? 1 : 0);
    }

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
