using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // links
    private Transform head;
    private CharacterController characterController;
    private PauseMenuController pauseController;
    private Rigidbody ball;

    // physics
    private float verticalVelocity = 0;
    private float gravityValue = 0.30f;
    private float jumpHeight = 1f;
    private float pushPower = 1f;

    // player settings
    private DeviceController device = new DeviceController();
    private Axis axis = new Axis();
    private bool isFrozen = false;
    private Vector3 warpPosition = Vector3.zero;
    private Vector3 defaultPosition;
    public bool isLeft = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        pauseController = GameObject.Find("Pause Menu").GetComponent<PauseMenuController>();
        head = transform.Find("Armature/Bone/Head Bone");
        ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        defaultPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (isFrozen || pauseController.isVisible)
        {
            return;
        }

        Axis axis = device.getAxis();

        verticalVelocity -= gravityValue;

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0)
            {
                verticalVelocity = 0f;
            }

            verticalVelocity += axis[2] * jumpHeight;
        }

        var direction = new Vector3(axis[0], verticalVelocity, axis[1]) * 10 * Time.deltaTime;
        var moveTarget = new Vector3(direction.x, 0, direction.z);

        if (moveTarget.magnitude > 0.15f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveTarget), Time.deltaTime * 10f);
        }

        var prevPosition = transform.position;
        characterController.Move(direction);
        head.position = new Vector3(prevPosition.x, prevPosition.y + 0.8f, prevPosition.z);

        if ((isLeft && transform.position.x > 0.5f) || (!isLeft && transform.position.x < -0.5f))
        {
            restartPosition();
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
    public void restartPosition()
    {
        warpPosition = defaultPosition;
        axis.reset();
    }

    public bool hasDevice()
    {
        return device.IsSelected();
    }

    public void setFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

    public void clearDevice()
    {
        device.Reset();
    }

    public void setDeviceId(int newDeviceId)
    {
        device.SetDeviceId(newDeviceId);
    }

    public void restartPlayer()
    {
        clearDevice();
        restartPosition();
    }

    public Axis getAxis()
    {
        return axis;
    }
}
