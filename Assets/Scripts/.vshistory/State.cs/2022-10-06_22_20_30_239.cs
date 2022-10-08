using UnityEngine;

public class State : MonoBehaviour
{
    public delegate void VoidDelegate();
    public static event VoidDelegate onResetGame;

    public void GameReset()
    {
        if (onResetGame != null) onResetGame();
    }
}
