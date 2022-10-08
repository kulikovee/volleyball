using UnityEngine;

public class ActionsContoller : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void ScoreDelegate(bool firstTeamIncrement, bool secondTeamIncrement);
    public static event VoidDelegate OnNextRound;
    public static event VoidDelegate onResetScene;
    public static event VoidDelegate onShowStartupMenu;
    public static event VoidDelegate onResetPlayersText;
    public static event VoidDelegate onJoinedPlayersText;
    public static event ScoreDelegate onUpdateScore;

    public static ActionsContoller GetActions()
    {
        return GameObject.Find("State").GetComponent<ActionsContoller>();
    }

    public void NextRound()
    {
        if (OnNextRound != null) OnNextRound();
    }
    public void ResetScene()
    {
        if (onResetScene != null) onResetScene();
    }
    public void showStartupMenu()
    {
        if (onShowStartupMenu != null) onShowStartupMenu();
    }

    public void ResetPlayersText()
    {
        if (onResetPlayersText != null) onResetPlayersText();
    }

    public void JoinedPlayersText()
    {
        if (onJoinedPlayersText != null) onJoinedPlayersText();
    }

    public void UpdateScore(bool firstTeamIncrement, bool secondTeamIncrement)
    {
        if (onUpdateScore != null) onUpdateScore(firstTeamIncrement, secondTeamIncrement);
    }
}
