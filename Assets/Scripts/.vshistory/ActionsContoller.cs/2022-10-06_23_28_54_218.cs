using UnityEngine;

public class ActionsContoller : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void ScoreDelegate(bool firstTeamIncrement, bool secondTeamIncrement);
    public static event VoidDelegate onNextRound;
    public static event VoidDelegate onResetScene;
    public static event VoidDelegate onResetScene;
    public static event ScoreDelegate onUpdateScore;

    public static ActionsContoller GetActions()
    {
        return GameObject.Find("State").GetComponent<ActionsContoller>();
    }

    public void NextRound()
    {
        if (onNextRound != null) onNextRound();
    }
    public void ResetScene()
    {
        if (onResetScene != null) onResetScene();
    }
    public void showStartupMenu()
    {
        if (onUpdateScore != null) onUpdateScore(firstTeamIncrement, secondTeamIncrement);
    }

    public void UpdateScore(bool firstTeamIncrement, bool secondTeamIncrement)
    {
        if (onUpdateScore != null) onUpdateScore(firstTeamIncrement, secondTeamIncrement);
    }
}
