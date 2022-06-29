using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : AnimationPlayer
{
    private const string DEFAULT_TOWARD = "default_toward";
    private const string DEFAULT_AWAY = "default_away";
    private const string PLAYER_IDLE = "player_idle";
    private const string PLAYER_IDLE_SLEEP = "player_idle_sleep";
    private const string PLAYER_IDLE_ONE = "player_idle_towards_one";
    private const string PLAYER_IDLE_TWO = "player_idle_towards_two";
    private const string RUN_TOWARD = "player_run_toward";
    private const string RUN_AWAY = "player_run_away";

    //Random randomNumber = new System.Random();
    //private int randomIdleIndex = Random.Range(0, 2);
    private float verticalInput;
    private float timer;
    private float longIdleTime;
    private bool isIdle;
        
    public void playAnimation(Animator animator){
        Animator playerAnimator = animator;
        //randomIdleIndex = Random.Range(0, 2);
        verticalInput = PlayerMovement.getInstance().lastVerticalInput;
        isIdle = PlayerMovement.getInstance().isIdle;
        timer = PlayerMovement.getInstance().timer;
        longIdleTime = PlayerMovement.getInstance().longIdleTime;

        if (isIdle == true && timer < longIdleTime) {
            animator.Play(PLAYER_IDLE);
        } else if (isIdle == true && timer > longIdleTime){
            animator.Play(PLAYER_IDLE_SLEEP);
        } else {
            playMovementAnimation(playerAnimator, verticalInput);
        }
    }

    private void playMovementAnimation(Animator animator, float verticalInput){
        if (verticalInput == 0){
            animator.Play(DEFAULT_TOWARD);
        } else if (verticalInput > 0){
            animator.Play(RUN_AWAY);
        } else if (verticalInput < 0){
            animator.Play(RUN_TOWARD);
        } else {
            Debug.Log("Error Animation not found for current vertical input!");
        }
    }

    // lets revist this. see if we can get random animations playing. As of 6/29/2022 this code is deprciated and needs updating/rewriting...
    private void playIdleAnimation(Animator animator, int idleIndex){
        //int randomNumber = Random.Range(0, 2);
        if (idleIndex == 0){
            animator.Play(PLAYER_IDLE_ONE);
            Debug.Log("Player Idle One Playing");
        } else if (idleIndex == 1){
            animator.Play(PLAYER_IDLE_TWO);
            Debug.Log("Player Idle Two Playing");
        } else {
            Debug.Log("Animation Not found!");
        }
    }
    

}
