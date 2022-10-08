public class Axis
{
    private float x = 0f;
    private float y = 0f;
    private float jump = 0f;
    
    public void SetX(float newX)
    {
        x = newX;
    }   

    public void SetY(float newY)
    {
        y = newY;
    }

    public void SetJump(float newJump)
    {
        jump = newJump;
    }
    
    public void GetX()
    {
        return x;
    }   

    public void GetY(float newY)
    {
        y = newY;
    }

    public void GetJump(float newJump)
    {
        jump = newJump;
    }

    public void Reset()
    {
        x = 0f;
        y = 0f;
        jump = 0f;
    }
}
