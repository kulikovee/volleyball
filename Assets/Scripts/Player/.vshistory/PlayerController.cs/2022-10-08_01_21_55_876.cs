using System.Collections;
using TMPro;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // configurable params
    public int playerId;
    public TextMeshProUGUI playerText;

    // physics
    private float verticalVelocity = 0;
    private float gravityValue = 0.30f;
    private float jumpHeight = 1f;
    private float pushPower = 1f;

    // player settings
    private DeviceController device;
    private CharacterController characterController;
    private bool isLeft;
    private Vector3 warpPosition = Vector3.zero;
    private Vector3 defaultPosition;

    void Start()
    {
        ActionsContoller.OnResetPlayersText += SetPlayerTextDefault;
        ActionsContoller.OnJoinedPlayersText += JoinedPlayersText;
        ActionsContoller.OnEndGame += ResetPlayer;
        ActionsContoller.OnRoundEnd += FreezePlayer;
        ActionsContoller.OnRoundStart += UnfreezePlayer;

        characterController = GetComponent<CharacterController>();
        device = GetComponent<DeviceController>();
        isLeft = playerId % 2 == 0;
        defaultPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (device.IsFrozen())
        {
            return;
        }

        var axis = device.GetUpdatedAxis();

        verticalVelocity -= gravityValue;

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = 0f;
            }

            verticalVelocity += axis.GetJump() * jumpHeight;
        }

        var direction = new Vector3(axis.GetX(), verticalVelocity, axis.GetY()) * 10 * Time.deltaTime;
        var rotateAt = new Vector3(direction.x, 0, direction.z);

        if (rotateAt.magnitude > 0.15f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateAt), Time.deltaTime * 10f);
        }

        characterController.Move(direction);

        if ((isLeft && transform.position.x > 0.5f) || (!isLeft && transform.position.x < -0.5f))
        {
            ResetPosition();
        }
    }

    void LateUpdate()
    {
        if (warpPosition != Vector3.zero)
        {
            transform.position = warpPosition;
            warpPosition = Vector3.zero;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic || hit.moveDirection.y < -0.3f || hit.gameObject.name != "Ball")
        {
            return;
        }

        bool isSameDirectionX = (characterController.velocity.x > 0 && hit.moveDirection.x > 0) || (characterController.velocity.x < 0 && hit.moveDirection.x < 0);
        bool isSameDirectionZ = (characterController.velocity.z > 0 && hit.moveDirection.z > 0) || (characterController.velocity.z < 0 && hit.moveDirection.z < 0);
        float additionalForceX = isSameDirectionX ? characterController.velocity.x : 0;
        float additionalForceZ = isSameDirectionZ ? characterController.velocity.z : 0;
        Vector3 additionalVelocity = new Vector3(additionalForceX, 1, additionalForceZ);

        body.velocity = (hit.moveDirection + additionalVelocity) * pushPower;
    }

    public void FreezePlayer()
    {
        SetFrozen(true);
    }

    public void UnfreezePlayer()
    {
        SetFrozen(false);
    }

    public void ResetPosition()
    {
        warpPosition = defaultPosition;
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
        playerText.text = "Player " + (playerId + 1) + "\nPress any button";
    }

    public void JoinedPlayersText()
    {
        if (device.IsSelected())
        {
            playerText.text = "Player " + (playerId + 1) + "\n<b>Joined!</b>";
        }
    }
}
