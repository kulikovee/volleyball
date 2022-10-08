using UnityEngine;

public class DeviceController : MonoBehaviour
{
    public static int NO_DEVICE = -100;
    public static int AI_CONTROLLS = -4;
    public static int KEYBOARD_WASD = -3;
    public static int KEYBOARD_ARROWS = -2;

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
            axisGamepad.set
        }
    }

    public int GetId()
    {
        return deviceId;
    }

    public void Reset()
    {
        previousDeviceId = deviceId;
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

    public bool IsPrevious(int checkDeviceId)
    {
        return previousDeviceId == checkDeviceId;
    }

    private bool IsGamepad()
    {
        return deviceId >= 0;
    }
}
