using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputGamepadController : MonoBehaviour
{
    private Axis axis = new Axis();
    private int gamePadId = -1;

    void Update()
    {
        if (gamePadId >= 0 || Gamepad.all.Count - 1 < gamePadId)
        {
            return;
        }

        var deviceId = device.GetId();
        var gamePad = Gamepad.all[deviceId];
        var stickX = gamePad.leftStick.x.ReadValue();
        var stickY = gamePad.leftStick.y.ReadValue();

        if (Math.Abs(stickX) > 0.1f || Math.Abs(stickY) > 0.1f)
        {
            axis.SetX(stickX);
            axis.SetY(stickY);
        }
        else
        {
            axis.SetX(gamePad.dpad.x.ReadValue());
            axis.SetY(gamePad.dpad.y.ReadValue());
        }

        axis.SetJump(gamePad.aButton.ReadValue());
    }
}
