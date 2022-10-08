using System;
using System.Collections;
using UnityEngine;

public class ActionsContoller : MonoBehaviour
{
    public delegate void VoidDelegate();
    public delegate void IntDelegate(int value);
    public delegate void ScoreDelegate(bool firstTeamIncrement, bool secondTeamIncrement);

    public static event VoidDelegate OnRoundEnd;
    public static event VoidDelegate OnRoundStart;
    public static event VoidDelegate OnRoundRestart;
    public static event VoidDelegate OnShowStartupMenu;
    public static event VoidDelegate OnFirstShowStartupMenu;
    public static event VoidDelegate OnResetPlayersText;
    public static event VoidDelegate OnJoinedPlayersText;
    public static event VoidDelegate OnEndGame;
    public static event VoidDelegate OnStartGame;
    public static event IntDelegate OnTimerUpdate;
    public static event IntDelegate OnSelectPauseOption;
    public static event ScoreDelegate OnUpdateScore;

    public static ActionsContoller GetActions()
    {
        return GameObject.Find("State").GetComponent<ActionsContoller>();
    }

    public void RoundRestart()
    {
        RoundEnd();
        StartCoroutine(StartRoundAfterDelay());
    }

    IEnumerator StartRoundAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        RoundStart();
    }

    public void RoundEnd()
    {
        if (OnRoundEnd != null) OnRoundEnd();
    }
    public void RoundStart()
    {
        if (OnRoundStart != null) OnRoundStart();
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

    public void TimerUpdate(int seconds)
    {
        if (OnTimerUpdate != null) OnTimerUpdate(seconds);
    }

    public void SelectPauseOption(int option)
    {
        if (OnSelectPauseOption != null) OnSelectPauseOption(option);
    }

    public void UpdateScore(bool firstTeamIncrement, bool secondTeamIncrement)
    {
        if (OnUpdateScore != null) OnUpdateScore(firstTeamIncrement, secondTeamIncrement);
    }
}
