using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    private bool isPlayerInRange;


    private void Awake(){
        isPlayerInRange = false; // when we start the game we want to make sure this bool is set ot false. The player and a collision will make it true later.
    }
    
    void Update(){
        //toggleDialogue();
    }

    /*
    private void toggleDialogue(){
        if (isPlayerInRange == true && !DialogueManager.getInstance().dialogueIsPlaying){
           //visualCue.SetActive(true);
           if (Input.GetKeyDown(KeyCode.F)){
               //DialogueSystem.getInstance().startDialogue(); we may want to make the DialogueSystem a singleton so we can call getInstance() safely.
               Debug.Log("F is pressed and Player is in Range");
               
           }
       } else {
           //visualCue.SetActive(false);
           Debug.Log("Player is not in Range and Dialogue is not starting.");
       }

    }
    */

    private void OnTriggerEnter(Collider other) {
       if (other.gameObject.tag == "Player"){
           isPlayerInRange = true;
       }
   }

   private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player"){
           isPlayerInRange = false;
       }
   }

    
}
