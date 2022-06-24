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
    
    /*
    public void playAnimation(Animator animator, string animationName){
        Animator playerAnimator = animator;

        checkSingleton();
        //Debug.Log("Player Vertical Input: " + vertical);
        if (animationName == PLAYER_IDLE_ONE) {
            playerAnimator.Play(PLAYER_IDLE_ONE);
        } else if (animationName == RUN_TOWARD) {
            playerAnimator.Play(RUN_TOWARD);
        } else if (animationName == RUN_AWAY) {
            playerAnimator.Play(RUN_AWAY);
        } else if (animationName == DEFAULT_TOWARD) {
            playerAnimator.Play(DEFAULT_TOWARD);
        } else if (animationName == DEFAULT_AWAY){
            playerAnimator.Play(DEFAULT_AWAY);
        } else {
            Debug.Log("Animation not found! Error!");
        }
    }
    */

    public void playAnimation(Animator animator){
        Animator playerAnimator = animator;

        verticalInput = PlayerMovement.getInstance().lastVerticalInput;
        Debug.Log("Vertical Input: " + verticalInput);
        if (verticalInput == 0){
            playerAnimator.Play(DEFAULT_TOWARD);
        } else if (verticalInput > 0){
            playerAnimator.Play(RUN_AWAY);
        } else if (verticalInput < 0){
            playerAnimator.Play(RUN_TOWARD);
        } else {
            Debug.Log("Error Animation not found for current vertical input!");
        }
    }

    private IEnumerator playDefault(Animator animator, string animationName){
        yield return new WaitForSeconds(5);
        animator.Play(animationName);
    }
    
}
