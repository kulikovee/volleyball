using UnityEngine;

public class State : MonoBehaviour
{
    public delegate void OnGameReset();
    public static event OnGameReset onGameReset;

    public void CallOnGameReset()
    {
        if (onGameReset != null)
        {
            onGameReset();
        }
    }
}
