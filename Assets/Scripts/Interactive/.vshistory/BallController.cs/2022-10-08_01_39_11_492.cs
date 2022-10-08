using UnityEngine;
using System.Collections.Generic;
using System;

public class BallController : MonoBehaviour
{    
    public List<AudioSource> hitSounds;
    public Renderer ground1;
    public Renderer ground2;

    // links
    private ActionsContoller actions;

    private float soundTimeout = 0.2f;
    private float lastSoundAt = 0;
    private Rigidbody rigidbody;
    private bool isTouchedGround = false;

    private void Start()
    {
        ActionsContoller.OnRoundEnd += FreezeAndResetPosition;
        ActionsContoller.OnRoundStart += Unfreeze;

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
            RoundRestart();
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
            ground1.material.color = new Color32(255, 160, 129, 1);
            ground2.material.color = new Color32(107, 255, 130, 1);

            UpdateGroundColor(isFirstTeamScore, isSecondTeamScore);            
            UpdateScore(isFirstTeamScore, isSecondTeamScore);
        }
    }

    public void RoundRestart()
    {
        actions.RoundRestart();
    }

    void UpdateScore(bool isFirstTeamScore, bool isSecondTeamScore) {
        actions.UpdateScore(isFirstTeamScore, isSecondTeamScore);
    }

    void UpdateGroundColor(bool isFirstTeamScore, bool isSecondTeamScore) {

        if (isFirstTeamScore)
        {
            ground1.material.color = new Color32(255, 0, 0, 1);
        }
        else if (isSecondTeamScore)
        {
            ground2.material.color = new Color32(0, 255, 0, 1);
        }
    }

    public void FreezeAndResetPosition()
    {
        rigidbody.position = new Vector3(0, 5, 0);
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.isKinematic = true;
    }

    public void Unfreeze()
    {
        rigidbody.isKinematic = false;
        isTouchedGround = false;
    }

    private void PlayHitSound()
    {
        if (Time.timeSinceLevelLoad - lastSoundAt > soundTimeout)
        {
            lastSoundAt = Time.timeSinceLevelLoad;
            hitSounds[UnityEngine.Random.Range(0, hitSounds.Count)].Play();
        }
    }
}
