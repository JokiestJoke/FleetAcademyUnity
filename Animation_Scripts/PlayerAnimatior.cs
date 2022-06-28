using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : AnimationPlayer
{
    private const string DEFAULT_TOWARD = "default_toward";
    private const string DEFAULT_AWAY = "default_away";
    private const string PLAYER_IDLE_ONE = "player_idle_1";
    private const string RUN_TOWARD = "player_run_toward";
    private const string RUN_AWAY = "player_run_away";
    
   
    //Random randomNumber = new System.Random();
    private int randomNumber;

    private float verticalInput;
    private int timer;
    private bool isIdle;
    
    public void playAnimation(Animator animator){
        Animator playerAnimator = animator;
        verticalInput = PlayerMovement.getInstance().lastVerticalInput;
        isIdle = PlayerMovement.getInstance().isIdle;
        //Debug.Log("Vertical Input: " + verticalInput);
        playMovementAnimation(playerAnimator, verticalInput);
        
        if (isIdle){
            playIdleAnimation(animator);
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

    
    private IEnumerator generateRandomeNumber(){
        yield return new WaitForSeconds(10);
        randomNumber = Random.Range(0, 2);
    }



    
    private void playIdleAnimation(Animator animator){
        animator.Play(PLAYER_IDLE_ONE);
        //int randomNumber = Random.Range(0, 2);
        /*
        if (randomNumber == 0){
            Debug.Log("0");
        } else if (randomNumber == 1){
            Debug.Log("1");
        } else {
            Debug.Log("Animation Not found!");
        }
        */
    }
    

}
