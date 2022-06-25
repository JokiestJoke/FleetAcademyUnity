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

    private float verticalInput;
    
    public void playAnimation(Animator animator){
        Animator playerAnimator = animator;
        verticalInput = PlayerMovement.getInstance().lastVerticalInput;
        Debug.Log("Vertical Input: " + verticalInput);
        playMovementAnimation(playerAnimator, verticalInput);
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

}
