using UnityEngine;

public class ActionsContoller : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void ScoreDelegate(bool firstTeamIncrement, bool secondTeamIncrement);
    public static event VoidDelegate OnNextRound;
    public static event VoidDelegate OnResetScene;
    public static event VoidDelegate OnShowStartupMenu;
    public static event VoidDelegate OnResetPlayersText;
    public static event VoidDelegate OnJoinedPlayersText;
    public static event ScoreDelegate OnUpdateScore;

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
        if (OnResetScene != null) OnResetScene();
    }
    public void showStartupMenu()
    {
        if (OnShowStartupMenu != null) OnShowStartupMenu();
    }

    public void ResetPlayersText()
    {
        if (OnResetPlayersText != null) OnResetPlayersText();
    }

    public void JoinedPlayersText()
    {
        if (OnJoinedPlayersText != null) OnJoinedPlayersText();
    }

    public void UpdateScore(bool firstTeamIncrement, bool secondTeamIncrement)
    {
        if (OnUpdateScore != null) OnUpdateScore(firstTeamIncrement, secondTeamIncrement);
    }
}
