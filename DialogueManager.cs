using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI; // this is very important to import this library so that this file can manipulate the unity UI I created
using TMPro;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{
    private string characterName; // declaring a string containing the characters name
    private Dialogue[] dialogues; //declaring an Array for Dialogue objects called dialogues
    private int numberOfDialogues; // declaring an integer for total number of dialogues on a character basis
    private int currentDialogueIndex = 0; // declaring an initial index of 0;
    
    private bool holdForResponse = false; // delclaring a bool which holds for a players response to dialogue choices
    public bool isDialogueActive = false; // I will use this bool to check if the dialogue is active currently.

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    private GameObject dialogueBox; // lets see if we need to use this...
    
    
    
    private int dialogueIndex = 0;
    private int choiceIndex = 0;

    private static DialogueManager instance; // declare instance so we can create a singleton.


     public static DialogueManager getInstance(){ //basic getInstance singleton we will refer to in our trigger.
        return instance;
    }

      private void Awake(){  // all thats needed in this awake is to error check to make sure there is not more than 1 instance of 
        if (instance != null){
            Debug.LogWarning("Warning: DialogueManager Singleton -> More than 1 instance!");
        }
        instance = this;
    }

    private void Start(){
        dialogueBox = GameObject.Find("dialogueBox"); // declaring the diaologuePanel variable to the GameObject dialogueBox which is a UI element in the unity Heirarchy
        dialoguePanel = GameObject.Find("dialoguePanel"); // declaring the diaologuePanel variable to the GameObject dialoguePanel which is a UI element in the unity Heirarchy
        dialoguePanel.SetActive(false); // make sure the dialogue panel is not set to active
    }

    private int calculateNumberOfDialogues(TextAsset xmlDocumentTextAsset){
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlDocumentTextAsset.text);
        int dialogueIndex = 0;

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){ //for each child character node in the dialogues parent node
            if (character.Attributes.GetNamedItem("name").Value == characterName){ // if the dialogues character name equals the NPC's name
                foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){
                    dialogueIndex++;// we increment the dialogueIndex for each dialogue, and we return the final index as the final number of dialogues
                }
            }
        }
        return dialogueIndex;
    }

    public void startDialogue(TextAsset xmlDocumentTextAsset, string npcName){ // helper function that makes sure our dialogues start properly.
        holdForResponse = false;
        isDialogueActive = true;
        characterName = npcName;
        Debug.Log(characterName);



        numberOfDialogues = calculateNumberOfDialogues(xmlDocumentTextAsset); // calculate the number of dialogues with a helper function.
        Debug.Log("Number of Dialogues: " + numberOfDialogues);
        //dialogues = new Dialogue[numberOfDialogues]; // initializing an array of Dialogue objects with a size of the total number of dialogue options on a character basis
       // assembleDialogueFromXml(xmlDocumentTextAsset); // assemble the dialogue with the helper function
    }

    private void assembleDialogueFromXml(TextAsset xmlDocumentTextAsset){
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlDocumentTextAsset.text);
        dialogueIndex = 0;  // make sure the anytime we load dialogue its index starts at 0. 

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){ //We loop through each node called character under the parent node dialogues.
            if (character.Attributes.GetNamedItem("name").Value == characterName){//we check if the character nodes 'name' attribute is equal to the value of the GameObjects name.
                dialogueIndex = 0; //making sure the dialogueIndex is set to 0
                populateDialogue(xmlDocument); // calling a helper function
            }
        }
    }

    private void populateDialogue(XmlDocument xmlDocument){
       foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){ // we loop through the dialogue node that is a child of character
            dialogues[dialogueIndex] = new Dialogue(); //create a new Dialogue Object for each dialogue we find. Store it in an array.
            dialogues[dialogueIndex].message = dialogueFromXML.Attributes.GetNamedItem("content").Value; //assign message attribute of the Dialogue object to the content of this dialogue node
            choiceIndex = 0; // reset the choice index. 

            dialogues[dialogueIndex].response = new string[2]; //define the size of the response array for this Dialogue Object
            dialogues[dialogueIndex].targetForResponse = new int [2]; //define the size of the argetForResponse array for this Dialogue Object
            populateResponses(dialogueFromXML); // calling a helper function
            dialogueIndex++; // increment dialogueIndex everytime a Dialogue Object is created
        }
    }

    private void populateResponses(XmlNode xmlNode){
        foreach(XmlNode choice in xmlNode){ //loop through each choice node that is a child of the corresponding dialogue node
                dialogues[dialogueIndex].response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;//assign the response attribute of the Dialogue object to the choice's content
                dialogues[dialogueIndex].targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value); // assign the targetForResponse attribute of the Dialogue object to the Parsed value of target
                choiceIndex++; //increment choiceIndex everytime a node is complete
            }
    }

}
