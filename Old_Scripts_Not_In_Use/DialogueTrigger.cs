using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    private bool isPlayerInRange;
    private GameObject npc;


    private void Awake(){
        isPlayerInRange = false; // when we start the game we want to make sure this bool is set ot false. The player and a collision will make it true later.
    }

    private void Start() {
        npc = gameObject; 
    }
    
    void Update(){
        toggleDialogue();

        
    }

    
    private void toggleDialogue(){
        if (isPlayerInRange == true && !gameObject.GetComponent<DialogueSystem>().isDialogueActive){
            Debug.Log("The NPC's name is " + npc.name + " and has collided with the trigger"); // testing purposes
            Debug.Log("Dialogue status is: " + gameObject.GetComponent<DialogueSystem>().isDialogueActive); // testing purposes

            gameObject.GetComponent<DialogueSystem>().startDialogue();



        }
    }
    

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
