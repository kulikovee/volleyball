using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{
    public List<Transform> ignoredColliders;

    private float pushPower = 5f;
    private CapsuleCollider characterCollider;
    private float gravityValue = 0.03f;
    private bool isGrounded = true;
    private Vector3 velocity = new();

    void Start()
    {
        characterCollider = GetComponent<CapsuleCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDynamicCollision(collision))
        {
            OnCollisionDynamic(collision);
        }
        else
        {
            OnCollisionStatic(collision);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isDynamicCollision(collision))
        {
            OnCollisionStatic(collision);
        }
    }

    private void OnCollisionDynamic(Collision collision)
    {
        var rigidbodyDirection = collision.transform.position - transform.position;

        if (!collision.rigidbody.isKinematic && rigidbodyDirection.y >= -0.3f)
        {
            OnRigidbodyHit(collision.rigidbody, rigidbodyDirection);
        }
    }

    private void OnCollisionStatic(Collision collision)
    {
        var collisionPoint = collision.GetContact(0).point;
        velocity = new Vector3(transform.position.x - collisionPoint.x, 0, transform.position.z - collisionPoint.z) / (collision.rigidbody != null ? 4f : 10f);
        transform.position += velocity;
    }

    bool isDynamicCollision(Collision collision)
    {
        return collision.rigidbody != null && !collision.rigidbody.freezeRotation;
    }

    void OnRigidbodyHit(Rigidbody body, Vector3 moveDirection)
    {
        bool isSameDirectionX = (velocity.x > 0 && moveDirection.x > 0) || (velocity.x < 0 && moveDirection.x < 0);
        bool isSameDirectionZ = (velocity.z > 0 && moveDirection.z > 0) || (velocity.z < 0 && moveDirection.z < 0);
        float additionalForceX = isSameDirectionX ? velocity.x : 0;
        float additionalForceZ = isSameDirectionZ ? velocity.z : 0;
        Vector3 additionalVelocity = new(additionalForceX, 0.02f, additionalForceZ);
        moveDirection /= 4f;
        additionalVelocity *= 10f;
        body.velocity = (moveDirection + additionalVelocity) * pushPower;
    }

    public void Move(Vector3 _move)
    {
        var move = _move / 4f;

        velocity.x = move.x;
        velocity.y -= gravityValue;
        velocity.z = move.z;

        if (isGrounded && move.y > 0)
        {
            isGrounded = false;
            velocity.y = 0.2f;
        }

        List<RaycastHit> hitsX = Raycast(new Vector3(velocity.x, 0, 0));
        List<RaycastHit> hitsZ = Raycast(new Vector3(0, 0, velocity.z));

        var subwayDelta = 0.05f;
        var subwayFactor = 1f;
        if (hitsX.Count > 0)
        {
            var previousVelocityX = velocity.x;
            velocity.x = 0;
            
            if (velocity.z == 0)
            {
                List<RaycastHit> hitsXZ1 = Raycast(new Vector3(previousVelocityX * subwayFactor, 0, -subwayDelta));

                if (hitsXZ1.Count == 0)
                {
                    velocity.x = previousVelocityX * subwayFactor;
                    velocity.z = -subwayDelta;
                }
                else
                {
                    List<RaycastHit> hitsXZ2 = Raycast(new Vector3(previousVelocityX * subwayFactor, 0, subwayDelta));
                    if (hitsXZ2.Count == 0)
                    {
                        velocity.x = previousVelocityX * subwayFactor;
                        velocity.z = subwayDelta;
                    } 
                }
            }
        }

        if (velocity.y + transform.position.y <= 0f)
        {
            velocity.y = -transform.position.y;
            isGrounded = true;
        }

        if (hitsZ.Count > 0)
        {
            var previousVelocityZ = velocity.z;
            velocity.z = 0;

            if (velocity.x == 0)
            {
                List<RaycastHit> hitsXZ1 = Raycast(new Vector3(-subwayDelta, 0, previousVelocityZ * subwayFactor));

                if (hitsXZ1.Count == 0)
                {
                    velocity.x = -subwayDelta;
                    velocity.z = previousVelocityZ * subwayFactor;
                }
                else
                {
                    List<RaycastHit> hitsXZ2 = Raycast(new Vector3(subwayDelta, 0, previousVelocityZ * subwayFactor));
                    if (hitsXZ2.Count == 0)
                    {
                        velocity.x = subwayDelta;
                        velocity.z = previousVelocityZ * subwayFactor;
                    }
                }
            }
        }

        RotateTo(velocity);
        transform.position += velocity;
    }

    private List<RaycastHit> Raycast(Vector3 direction)
    {
        var colliderBottom = direction + transform.position + characterCollider.center + Vector3.up * -characterCollider.height * 0.5F;
        var colliderTop = colliderBottom + Vector3.up * characterCollider.height;
        var radius = characterCollider.radius;
        var allHits = Physics.CapsuleCastAll(colliderBottom, colliderTop, radius, direction, 0.1f);
        var affectingHits = new List<RaycastHit>();

        foreach(var hit in allHits)
        {
            if (
                hit.rigidbody == null
                && hit.transform != transform
                && !ignoredColliders.Contains(hit.transform)
            )
            {
                affectingHits.Add(hit);
            }
        }

        return affectingHits;
    }

    private void RotateTo(Vector3 _rotateTo)
    {
        var rotateTo = new Vector3(_rotateTo.x, 0, _rotateTo.z);
        if (rotateTo.magnitude > 0.15f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(rotateTo),
                Time.deltaTime * 10f
            );
        }
    }
}
