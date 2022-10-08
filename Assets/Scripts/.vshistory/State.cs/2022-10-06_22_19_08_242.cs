using UnityEngine;

public class State : MonoBehaviour
{
    private delegate void VoidDelegate();
    private static event VoidDelegate onGameReset;

    public void GameReset()
    {
        if (onGameReset != null)
        {
            onGameReset();
        }
    }

    public void OnGameReset(VoidDelegate onGameResetCallback) {
        onGameReset += onGameResetCallback;
    }
}
