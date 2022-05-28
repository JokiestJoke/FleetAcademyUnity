/*
Author: Mark Doghramji
LinkedIn: https://www.linkedin.com/in/mark-doghramji/
Last Update: 5/26/2022
Notes:
This class acts as the trigger to start dialogues with specific npcs that the script is attached to. This NPC will also have a .xml file with their SPECIFIC dialogue attached to it.
When the player interacts with the NPC, the xml file and the npc name are fed to the DialogueManager.cs with is public method of startDialogue(). It is important to note that
the format of the xml should not be changed from the format i have assigned. In truth if you wanted to change the xml file up to add more nodes, you would simply need to key
in to the proper nodes of the xml, wherever and however you are choosing populate the Dialogue object(s). Also the NPC gameObject should be named after the NPC's actual name that is
in the .xml file. 

To do: I need to add a visual cue with artwork. 
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue; /// need artword for visual cue before calling it in game. Will create in Asperite later.
    

    GameObject NPC;

    [Header("XML Document")]
    [SerializeField] private TextAsset xmlDocumentTextAsset;

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
                DialogueManager.getInstance().startDialogue(xmlDocumentTextAsset, NPC.name);
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
