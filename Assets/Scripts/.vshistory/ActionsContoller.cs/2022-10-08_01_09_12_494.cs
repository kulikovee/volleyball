using System.Collections;
using UnityEngine;

public class ActionsContoller : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void ScoreDelegate(bool firstTeamIncrement, bool secondTeamIncrement);
    public static event VoidDelegate OnRoundEnd;
    public static event VoidDelegate OnRoundStart;
    public static event VoidDelegate OnRoundRestart;
    public static event VoidDelegate OnResetScene;
    public static event VoidDelegate OnShowStartupMenu;
    public static event VoidDelegate OnFirstShowStartupMenu;
    public static event VoidDelegate OnResetPlayersText;
    public static event VoidDelegate OnJoinedPlayersText;
    public static event VoidDelegate OnEndGame;
    public static event VoidDelegate OnStartGame;
    public static event ScoreDelegate OnUpdateScore;

    public static ActionsContoller GetActions()
    {
        return GameObject.Find("State").GetComponent<ActionsContoller>();
    }

    public void RoundEnd()
    {
        if (OnRoundEnd != null) OnRoundEnd();
    }
    public void RoundStart()
    {
        if (OnRoundStart != null) OnRoundStart();
    }
    public void RoundRetart()
    {
        RoundEnd();
        StartCoroutine(StartRoundAfterDelay());
    }
    IEnumerator StartRoundAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        RoundStart();
    }

    public void ResetScene()
    {
        if (OnResetScene != null) OnResetScene();
    }

    public void ShowStartupMenu()
    {
        if (OnShowStartupMenu != null) OnShowStartupMenu();
    }
    public void FirstShowStartupMenu()
    {
        if (OnFirstShowStartupMenu != null) OnFirstShowStartupMenu();
    }

    public void ResetPlayersText()
    {
        if (OnResetPlayersText != null) OnResetPlayersText();
    }

    public void JoinedPlayersText()
    {
        if (OnJoinedPlayersText != null) OnJoinedPlayersText();
    }
    
    public void EndGame()
    {
        if (OnEndGame != null) OnEndGame();
    }

    public void StartGame()
    {
        if (OnStartGame != null) OnStartGame();
    }

    public void UpdateScore(bool firstTeamIncrement, bool secondTeamIncrement)
    {
        if (OnUpdateScore != null) OnUpdateScore(firstTeamIncrement, secondTeamIncrement);
    }
}
