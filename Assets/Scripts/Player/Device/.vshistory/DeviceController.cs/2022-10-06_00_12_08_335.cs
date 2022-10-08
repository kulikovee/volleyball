using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device : MonoBehaviour
{
    public static int NO_DEVICE = -100;
    public static int AI_CONTROLLS = -4;
    public static int KEYBOARD_WASD = -3;
    public static int KEYBOARD_ARROWS = -2;

    private int deviceId = NO_DEVICE;

    private Axis defaultAxis = new Axis();
    private Axis axis;

    public Device()
    {
        this.deviceId = NO_DEVICE;
    }

    public void SetDeviceId(int newDeviceId)
    {
        deviceId = newDeviceId;
    }

    public int GetDeviceId()
    {
        return deviceId;
    }

    public void Reset()
    {
        deviceId = NO_DEVICE;
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
        return IsSelected() ? axis : defaultAxis;
    }
}
