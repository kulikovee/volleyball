using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadController : MonoBehaviour
{
    private Axis axis = new Axis();
    // TODO: remove device dependency
    private DeviceController device;

    private void Start()
    {
        // TODO: remove device dependency
        device = GetComponent<DeviceController>();
    }

    void Update()
    {
        if (!device.IsSelected() || Gamepad.all.Count - 1 < device.GetId())
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
