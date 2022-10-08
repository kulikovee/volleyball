using UnityEngine;

public class StateController : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void ScoreDelegate();
    public static event VoidDelegate onNextRound;
    public static event VoidDelegate onResetScene;
    public static event VoidDelegate onUpdateScore;

    public void NextRound()
    {
        if (onNextRound != null) onNextRound();
    }
    
    public void ResetScene()
    {
        if (onResetScene != null) onResetScene();
    }

    public void UpdateScore()
    {
        if (onUpdateScore != null) onUpdateScore();
    }
}
