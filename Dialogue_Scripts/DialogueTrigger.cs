using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue; /// need artword for visual cue before calling it in game. Will create in Asperite later.
    
    private GameObject NPC;

    [Header("XML Document")]
    [SerializeField] private TextAsset xmlTextAsset;

    private bool playerInRange;

    private void Awake(){
        playerInRange = false;
        visualCue.SetActive(false);
        NPC = gameObject;
    }

    private void Update()
    {
        if (playerInRange == true && !DialogueManager.getInstance().isDialogueActive){
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F)){
                DialogueManager.getInstance().startDialogue(xmlTextAsset, NPC.name);
            }
        } else {
            visualCue.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player"){
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
            if (other.gameObject.tag == "Player"){
            playerInRange = false;
        }
    }
}
