using UnityEngine;

public class Axis
{
    private float x = 0f;
    private float y = 0f;
    private float jump = 0f;
    private float submit = 0f;
    
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

    public void SetJump(float newJump)
    {
        jump = newJump;
    }  

    public float GetJump()
    {
        return jump;
    }

    public void ResetAxis()
    {
        x = 0f;
        y = 0f;
        jump = 0f;
    }
}
