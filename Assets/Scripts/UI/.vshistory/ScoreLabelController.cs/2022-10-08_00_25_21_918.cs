using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLabelController : MonoBehaviour
{
    public AudioSource gameOverSound;
    public AudioSource scoreUpdateSound0;
    public AudioSource scoreUpdateSound1;
    public AudioSource scoreUpdateSound2;

    private Renderer ground1;
    private Renderer ground2;

    public int teamScore1 = 0;
    public int teamScore2 = 0;

    private int winScore = 10;
    private TextMeshProUGUI score;
    private StartupMenuController menu;

    // animations
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        ActionsContoller.OnUpdateScore += UpdateScore;
        score = gameObject.GetComponent<TextMeshProUGUI>();
        animator = transform.parent.GetComponent<Animator>();
        menu = GameObject.Find("Startup Menu").GetComponent<StartupMenuController>();
        ground1 = GameObject.Find("Ground 1").GetComponent<Renderer>();
        ground2 = GameObject.Find("Ground 2").GetComponent<Renderer>();
    }

    public void UpdateScore(bool isFirstTeam, bool isSecondTeam)
    {
        if (isFirstTeam)
        {
            teamScore1++;
            ground1.material.color = new Color32(255, 0, 0, 1);
        }

        if (isSecondTeam)
        {
            teamScore2++;
            ground2.material.color = new Color32(0, 255, 0, 1);
        }

        updateScoreText();

        if (isFirstTeam || isSecondTeam)
        {
            animator.Play("Score Update");
            StartCoroutine(menu.NextRoundDelayed());
        }

        if (teamScore1 == winScore || teamScore2 == winScore)
        {
            gameOverSound.Play();
            StartCoroutine(menu.WinGameDelayed());
        }

        else if(isFirstTeam || isSecondTeam)
        {
            StartCoroutine(playScoreUpdateSound());
        }
    }

    public IEnumerator playScoreUpdateSound()
    {
        yield return new WaitForSeconds(0.1f);

        var random = Random.Range(0f, 1f);

        if (random < 0.33f)
        {
            scoreUpdateSound0.Play();
        }
        else if (random > 0.66f)
        {
            scoreUpdateSound1.Play();
        }
        else
        {
            scoreUpdateSound2.Play();
        }
    }

    public void resetScore()
    {
        teamScore1 = 0;
        teamScore2 = 0;
        updateScoreText();
    }

    private void updateScoreText()
    {
        score.text = teamScore1 + ":" + teamScore2;
    }
}
