using UnityEngine;

public class Axis
{
    private float x = 0f;
    private float y = 0f;
    private float action = 0f;
    private float pause = 0f;
    
    public void SetX(float newX)
    {
        x = Mathf.Clamp(newX, -1f, 1f);
    }

    public float GetX()
    {
        return x;
    }

    public void SetY(float newY)
    {
        y = Mathf.Clamp(newY, -1f, 1f);
    }
    public float GetY()
    {
        return y;
    }

    public void SetAction(float newJump)
    {
        action = newJump;
    }  

    public float GetJump()
    {
        return action;
    }

    public void ResetAxis()
    {
        x = 0f;
        y = 0f;
        action = 0f;
    }
}
