using System.Collections.Generic;
using UnityEngine;

public class PauseExitController : MonoBehaviour
{
    public List<AudioSource> scoreUpdateSounds;

    private ActionsContoller actions;

    private void Start()
    {
        actions = ActionsContoller.GetActions();
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
}
