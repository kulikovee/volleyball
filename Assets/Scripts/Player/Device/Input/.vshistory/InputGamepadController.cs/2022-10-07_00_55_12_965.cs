using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputGamepadController : MonoBehaviour
{
    private static readonly int NOT_SELECTED = -1;

    private readonly Axis axis = new();
    private int gamePadId = NOT_SELECTED;

    void Update()
    {
        if (gamePadId >= 0 || Gamepad.all.Count - 1 < gamePadId)
        {
            return;
        }

        var gamePad = Gamepad.all[gamePadId];
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

    public void SetGamepadId(int newGamePadId)
    {
        gamePadId = newGamePadId;
    }

    public void ResetGamepadId()
    {
        gamePadId = NOT_SELECTED;
    }

    public Axis GetAxis()
    {
        return axis;
    }
}
