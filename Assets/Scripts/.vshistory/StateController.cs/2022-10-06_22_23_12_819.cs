using UnityEngine;

public class StateController : MonoBehaviour
{
    public delegate void VoidDelegate();
    public static event VoidDelegate onResetGame;
    public static event VoidDelegate onNextRound;

    public void ResetGame()
    {
        if (onResetGame != null) onResetGame();
    } 

    public void NextRound()
    {
        if (onNextRound != null) onNextRound();
    }

    public void UpdateScore()
    {
        if (onNextRound != null) onNextRound();
    }
    
    public void ResetScene()
    {
        if (onNextRound != null) onNextRound();
    }

    public void UpdateScore()
    {
        if (onNextRound != null) onNextRound();
    }
}
