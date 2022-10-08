using UnityEngine;

public class State : MonoBehaviour
{
    public delegate void VoidDelegate();
    public static event VoidDelegate onGameReset;

    public void GameReset()
    {
        if (onGameReset != null) onGameReset();
    }
}
