public class Axis
{
    private float x = 0f;
    private float y = 0f;
    private float jump = 0f;
    
    public void SetX(float newX)
    {
        x = newX;
    }

    public float GetX()
    {
        return x;
    }

    public void SetY(float newY)
    {
        y = newY;
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

    public void Reset()
    {
        x = 0f;
        y = 0f;
        jump = 0f;
    }
}
