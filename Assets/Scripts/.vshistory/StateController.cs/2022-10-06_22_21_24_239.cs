using UnityEngine;

public class StateController : MonoBehaviour
{
    public delegate void VoidDelegate();
    public static event VoidDelegate onResetGame;

    public void ResetGame()
    {
        if (onResetGame != null) onResetGame();
    }
}
