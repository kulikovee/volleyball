using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputGamepadController : MonoBehaviour
{
    private static readonly int NOT_SELECTED = -1;

    private readonly Axis axis = new();
    private int gamePadId = NOT_SELECTED;

    void Update()
    {
        if (gamePadId < 0 || gamePadId > Gamepad.all.Count - 1)
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

    public static List<int> GetPressedIds()
    {
        var pressedIds = new List<int>();

        for (int gamePadId = 0; gamePadId < Gamepad.all.Count; gamePadId++)
        {
                var gamePad = Gamepad.all[gamePadId];
                if (
                    gamePad.aButton.isPressed
                    || gamePad.bButton.isPressed
                    || gamePad.yButton.isPressed
                    || gamePad.xButton.isPressed
                )
                {
                    pressedIds.Add(gamePadId);
                }
            /*
             * else
            {
                if (Gamepad.all[gamePadId].startButton.isPressed)
                {
                    pause.updatePauseTimeout();
                    TimerFinished();
                }
            }
            */
        }

        return pressedIds;
    }


    public static List<int> GetSkipGamepadIds()
    {
        var skipGamePadIds = new List<int>();

        for (int gamePadId = 0; gamePadId < Gamepad.all.Count; gamePadId++)
        {
            if (Gamepad.all[gamePadId].startButton.isPressed)
            {
                skipGamePadIds.Add(gamePadId);
            }
        }

        return skipGamePadIds;
    }
}
