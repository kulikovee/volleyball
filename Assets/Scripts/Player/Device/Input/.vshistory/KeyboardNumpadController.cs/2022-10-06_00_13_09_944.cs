using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardNumpadController : MonoBehaviour
{
    private Axis axis = new Axis();
    private float keyboardMovementFactor = 0.7f;

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

        axis.setX((isLeft ? -1 : (isRight ? 1 : 0)) * keyboardMovementFactor);
        axis.setY((isDown ? -1 : (isUp ? 1 : 0)) * keyboardMovementFactor);
        axis.setJump(isJump ? 1 : 0);
    }

    public void SetDevice(DeviceController newDevice)
    {
        device = newDevice;
    }
}
