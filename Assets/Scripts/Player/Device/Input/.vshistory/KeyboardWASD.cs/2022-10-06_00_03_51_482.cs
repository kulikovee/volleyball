using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardWASDController : MonoBehaviour
{
    private Axis axis = new Axis();
    private Device device;

    void Update()
    {
        var deviceId = device.GetDeviceId();

        if (Gamepad.all.Count <= deviceId)
        {
            return;
        }

        var gamePad = Gamepad.all[deviceId];
        var stickX = gamePad.leftStick.x.ReadValue();
        var stickY = gamePad.leftStick.y.ReadValue();

        if (Math.Abs(stickX) > 0.1f || Math.Abs(stickY) > 0.1f)
        {
            axis.setX(stickX);
            axis.setY(stickY);
        }
        else
        {
            axis.setX(gamePad.dpad.x.ReadValue());
            axis.setY(gamePad.dpad.y.ReadValue());
        }

        axis.setJump(gamePad.aButton.ReadValue());
    }

    public void SetDevice(Device newDevice)
    {
        device = newDevice;
    }
}
