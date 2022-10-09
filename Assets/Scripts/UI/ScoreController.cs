using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public List<AudioSource> scoreUpdateSounds;

    private ActionsContoller actions;
    private Animator animator;
    private bool isShown = false;

    private void Start()
    {
        ActionsContoller.OnStartGame += Show;

        actions = ActionsContoller.GetActions();
        animator = GetComponent<Animator>();
    }

    /** Called from animation: Score Update **/
    public void PlayScoreUpdateSound()
    {
        scoreUpdateSounds[Random.Range(0, scoreUpdateSounds.Count)].Play();
    }

    /** Called from animation: Score Update **/
    public void RoundRestart()
    {
        actions.RoundRestart();
    }    
    
    public void Show()
    {
        if (!isShown)
        {
            isShown = true;
            animator.Play("Score Show");
        }
    }
}
