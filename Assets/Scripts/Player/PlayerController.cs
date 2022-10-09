using KinematicCharacterController;
using TMPro;
using UnityEngine;
public struct PlayerCharacterInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpDown;
    public bool CrouchDown;
    public bool CrouchUp;
}
public class PlayerController : MonoBehaviour
{
    // configurable params
    public int playerId;
    public TextMeshProUGUI playerText;

    // player settings
    private DeviceController device;
    private KinematicCharacterAdapter characterAdapter;
    private bool isLeft;
    // private Vector3 warpPosition = Vector3.zero;
    private Vector3 defaultPosition;

    void Start()
    {
        ActionsContoller.OnResetPlayersText += SetPlayerTextDefault;
        ActionsContoller.OnJoinedPlayersText += JoinedPlayersText;
        ActionsContoller.OnEndGame += ResetPlayer;
        ActionsContoller.OnRoundEnd += FreezeAndResetPosition;
        ActionsContoller.OnRoundStart += UnfreezePlayer;

        characterAdapter = GetComponent<KinematicCharacterAdapter>();
        device = GetComponent<DeviceController>();
        isLeft = playerId % 2 == 0;
        defaultPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if ((isLeft && transform.position.x > 0.5f) || (!isLeft && transform.position.x < -0.5f))
        {
            ResetPosition();
        }
    }

    public void FreezeAndResetPosition()
    {
        SetFrozen(true);
        ResetPosition();
    }

    public void UnfreezePlayer()
    {
        SetFrozen(false);
    }

    public void ResetPosition()
    {
        characterAdapter.SetPosition(defaultPosition);
        device.GetUpdatedAxis().ResetAxis();
    }

    public void SetFrozen(bool frozen)
    {
        device.SetFrozen(frozen);
    }

    public void ResetPlayer()
    {
        device.ResetDeviceId();
        ResetPosition();
        SetFrozen(true);
        SetPlayerTextDefault();
    }

    public DeviceController GetDevice()
    {
        return device;
    }

    public void SetPlayerTextDefault()
    {
        playerText.SetText("Player " + (playerId + 1) + "\nPress any button");
    }

    public void JoinedPlayersText()
    {
        if (device.IsSelected())
        {
            playerText.SetText("Player " + (playerId + 1) + "\n<b>Joined!</b>");
        }
    }
}
