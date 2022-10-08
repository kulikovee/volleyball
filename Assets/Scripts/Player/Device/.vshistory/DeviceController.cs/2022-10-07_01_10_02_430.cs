using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour
{
    public static int NO_DEVICE = -100;
    public static int AI_CONTROLLS = -4;
    public static int KEYBOARD_WASD = -3;
    public static int KEYBOARD_NUMPAD = -2;

    private InputAIController inputAI;
    private InputWASDController inputWASD;
    private InputNumpadController inputNumpad;
    private InputGamepadController inputGamepad;
    
    private int deviceId = NO_DEVICE;
    private int previousDeviceId = NO_DEVICE;

    private Axis defaultAxis = new();

    public void Start()
    {
        inputAI = GetComponent<InputAIController>();
        inputWASD = GetComponent<InputWASDController>();
        inputNumpad = GetComponent<InputNumpadController>();
        inputGamepad = GetComponent<InputGamepadController>();
    }

    public void SetId(int newDeviceId)
    {
        deviceId = newDeviceId;

        if (IsGamepad())
        {
            inputGamepad.SetGamepadId(deviceId);
        }
    }

    public int GetId()
    {
        return deviceId;
    }

    public void ResetDeviceId()
    {
        previousDeviceId = deviceId;
        deviceId = NO_DEVICE;
        inputGamepad.ResetGamepadId();
    }

    public bool IsEquals(int checkDeviceId)
    {
        return deviceId == checkDeviceId;
    }

    public bool IsSelected()
    {
        return deviceId != NO_DEVICE;
    }

    public Axis GetAxis()
    {
        if (IsGamepad()) return inputGamepad.GetAxis();
        if (IsWASD()) return inputWASD.GetAxis();
        if (IsNumpad()) return inputNumpad.GetAxis();
        if (IsAI()) return inputAI.GetAxis();

        return defaultAxis;
    }

    public bool IsPrevious(int checkDeviceId)
    {
        return previousDeviceId == checkDeviceId;
    }

    public static bool isWASD()
    {
        var isWasd = false;

        var KEYBOARD_WASD_KEYS = new List<KeyCode>
        {
            KeyCode.A,
            KeyCode.D,
            KeyCode.W,
            KeyCode.S,
            KeyCode.Space,
        };

        KEYBOARD_WASD_KEYS.ForEach((keyCode) => {
            if (Input.GetKeyDown(keyCode))
            {
                isWasd = true;
            }
        });

        return isWasd;
    }

    private bool IsGamepad()
    {
        return deviceId >= 0;
    }

    private bool IsWASD()
    {
        return deviceId == KEYBOARD_WASD;
    }

    private bool IsNumpad()
    {
        return deviceId == KEYBOARD_NUMPAD;
    }

    private bool IsAI()
    {
        return deviceId == AI_CONTROLLS;
    }
}
