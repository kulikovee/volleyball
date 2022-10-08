using UnityEngine;

public class State : MonoBehaviour
{
    public delegate void OnGameResetDelegate();
    public static event OnGameResetDelegate onGameReset;

    public void GameReset()
    {
        if (onGameReset != null)
        {
            onGameReset();
        }
    }
}
