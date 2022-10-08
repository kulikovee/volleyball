using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLabelController : MonoBehaviour
{
    public AudioSource gameOverSound;

    private Renderer ground1;
    private Renderer ground2;

    public int teamScore1 = 0;
    public int teamScore2 = 0;

    private int winScore = 2;
    private ActionsContoller actions;
    private TextMeshProUGUI score;
    private StartupMenuController menu;

    // animations
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        ActionsContoller.OnUpdateScore += UpdateScore;
        ActionsContoller.OnStartGame += ResetScore;

        actions = ActionsContoller.GetActions();
        score = gameObject.GetComponent<TextMeshProUGUI>();
        animator = transform.parent.GetComponent<Animator>();
        menu = GameObject.Find("Startup Menu").GetComponent<StartupMenuController>();
    }

    public void UpdateScore(bool isFirstTeam, bool isSecondTeam)
    {
        if (isFirstTeam)
        {
            teamScore1++;
        }

        if (isSecondTeam)
        {
            teamScore2++;
        }

        UpdateScoreText();

        if (isFirstTeam || isSecondTeam)
        {
            animator.Play("Score Update");
        }

        if (teamScore1 == winScore || teamScore2 == winScore)
        {
            gameOverSound.Play();
            StartCoroutine(menu.WinGameDelayed());
        }
    }
    public void RoundRestart()
    {
        actions.RoundRestart();
    }
    
    public void ResetScore()
    {
        teamScore1 = 0;
        teamScore2 = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        score.text = teamScore1 + ":" + teamScore2;
    }
}
