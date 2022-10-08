using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreLabelController : MonoBehaviour
{
    public AudioSource gameOverSound;

    private int winScore = 10;
    private int teamScore1 = 0;
    private int teamScore2 = 0;

    private ActionsContoller actions;
    private TextMeshProUGUI score;

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
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);
        actions.EndGame();
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
