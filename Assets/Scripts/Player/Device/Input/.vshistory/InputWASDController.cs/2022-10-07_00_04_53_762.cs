using UnityEngine;

public class InputWASDController : MonoBehaviour
{
    private readonly Axis axis = new();
    private readonly float keyboardMovementFactor = 0.7f;

    void Update()
    {
        bool isUp = Input.GetKey(KeyCode.W);
        bool isDown = Input.GetKey(KeyCode.S);
        bool isRight = Input.GetKey(KeyCode.D);
        bool isLeft = Input.GetKey(KeyCode.A);
        bool isJump = Input.GetKey(KeyCode.Space);

        axis.SetX((isLeft ? -1 : (isRight ? 1 : 0)) * keyboardMovementFactor);
        axis.SetY((isDown ? -1 : (isUp ? 1 : 0)) * keyboardMovementFactor);
        axis.SetJump(isJump ? 1 : 0);
    }

    public Axis GetAxis()
    {
        return axis;
    }
}
