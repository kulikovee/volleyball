using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class BallController : MonoBehaviour
{    
    // sounds
    public AudioSource hit0;
    public AudioSource hit1;
    public AudioSource hit2;

    // links
    private ActionsContoller actions;

    // params
    private float soundTimeout = 0.2f;
    private float lastSoundAt = 0;
    private Rigidbody rigidbody;

    public bool isTouchedGround = false;

    private void Start()
    {
        actions = ActionsContoller.GetActions();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rigidbody.isKinematic)
        {
            return;
        }

        var position = gameObject.transform.position;

        if (
            Math.Abs(position.x) > 5.5f || Math.Abs(position.z) > 5.5f || position.y > 10 || position.y < -2
        )
        {
            actions.NextRound();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        bool isFirstTeamScore = collision.gameObject.name == "Ground 1";
        bool isSecondTeamScore = collision.gameObject.name == "Ground 2";

        PlayHitSound();

        if (!isTouchedGround && (isFirstTeamScore || isSecondTeamScore))
        {
            isTouchedGround = true;
            UpdateScore(isFirstTeamScore, isSecondTeamScore);            
        }
    }

    void UpdateScore(bool isFirstTeamScore, bool isSecondTeamScore) {
        actions.ResetScene();
        actions.UpdateScore(isFirstTeamScore, isSecondTeamScore);
    }

    public void FreezeBall()
    {
        rigidbody.position = new Vector3(0, 5, 0);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;
    }

    public void UnfreezeBall()
    {
        rigidbody.isKinematic = false;
    }

    private void PlayHitSound()
    {
        if (Time.timeSinceLevelLoad - lastSoundAt > soundTimeout)
        {
            lastSoundAt = Time.timeSinceLevelLoad;

            var randomSound = UnityEngine.Random.Range(0f, 1f);

            if (randomSound < 0.33f)
            {
                hit0.Play();
            }
            else if (randomSound > 0.66f)
            {
                hit1.Play();
            }
            else
            {
                hit2.Play();
            }
        }
    }
}
