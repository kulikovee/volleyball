using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public AudioSource scoreUpdateSound0;
    public AudioSource scoreUpdateSound1;
    public AudioSource scoreUpdateSound2;
    
    public void PlayScoreUpdateSound()
    {
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
}
