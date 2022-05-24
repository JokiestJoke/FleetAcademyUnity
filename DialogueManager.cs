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
    private int numberOfResponses;
    private int currentDialogueIndex = 0; // declaring an initial index of 0;
    
    private bool holdForResponse; // delclaring a bool which holds for a players response to dialogue choices
    public bool isDialogueActive; // I will use this bool to check if the dialogue is active currently.

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    //[SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choiceButtons; // choices that will correspond to the Buttons in unity
    private TextMeshProUGUI[] choicesText;
    
    private int dialogueIndex;
    private int choiceIndex;

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

        dialogueIndex = 0;
        choiceIndex = 0;

        isDialogueActive = false; // declaring the diaologuePanel variable to the GameObject dialogueBox which is a UI element in the unity Heirarchy
        holdForResponse = false; // declaring the diaologuePanel variable to the GameObject dialoguePanel which is a UI element in the unity Heirarchy
        dialoguePanel.SetActive(false); // make sure the dialogue panel is not set to active

        choicesText = new TextMeshProUGUI[choiceButtons.Length]; //initializing a TextMesh array for the response text. same length as the array holding the GameObject buttons

        setupButtonsAtStart(); // call helper function to set up buttons at start of the frame.

        //int numResponses = calculateNumberOfResponses();
        //Debug.Log(numResponses);

        
    
    }

    private void Update(){

        manageDialogue();

    }

    private void setupButtonsAtStart(){ //helper function to setup the buttons with the TextMeshPROGUI components attached to them. essential for displaying text.
        int choiceIndex = 0;
        foreach(GameObject choice in choiceButtons){
            choicesText[choiceIndex] = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceIndex++;
        }
    } 

    private void displayResponsesToButtons(){
        //string[] currentResponses = new string[dialogues.response.Length];
        string[] currentResponses;

    }

    private void manageDialogue(){
        if (isDialogueActive){
            if (!holdForResponse){ //if we are waiting for a repsonse - remeber that holdForResponse is by default initiliazed as false
                dialoguePanel.SetActive(true); // if dialogue is active and we are waiting for a response we set the dialoguePanel to active
                if (currentDialogueIndex != -1){// and the dialogue is NOT finished (which -1 would indicate in the XML file under target)
                    displayDialogue(); // then we display dialogue
                } 
                else { //if the dialogue is -1 then we must end the dialogue
                    endDialogue();
                }
                holdForResponse = true; //we then wait for user input 
            } else {
                handleUserInputForResponse();
            }
        }
    }

    private int calculateNumberOfResponses(TextAsset xmlTextAsset){
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlTextAsset.text);
        int resonseCount = 0;
        
        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){ //for each child character node in the dialogues parent node
            if (character.Attributes.GetNamedItem("name").Value == characterName){ // if the dialogues character name equals the NPC's name
                foreach(XmlNode responseDialogue in xmlDocument.SelectNodes("dialogues/character/dialogue/choice")){
                    resonseCount++;// we increment the dialogueIndex for each dialogue, and we return the final index as the final number of dialogues
                }
            }
        }
        return resonseCount;
    }

    private int calculateNumberOfDialogues(TextAsset xmlTextAsset){
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlTextAsset.text);
        int dialogueCount = 0;

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){ //for each child character node in the dialogues parent node
            if (character.Attributes.GetNamedItem("name").Value == characterName){ // if the dialogues character name equals the NPC's name
                foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){
                    dialogueCount++;// we increment the dialogueIndex for each dialogue, and we return the final index as the final number of dialogues
                }
            }
        }
        return dialogueCount;
    }

    public void startDialogue(TextAsset xmlTextAsset, string npcName){ // helper function that makes sure our dialogues start properly.
        holdForResponse = false; 
        isDialogueActive = true;
        characterName = npcName;
        //Debug.Log(characterName);

        numberOfDialogues = calculateNumberOfDialogues(xmlTextAsset); // calculate the number of dialogues with a helper function.
        numberOfResponses = calculateNumberOfResponses(xmlTextAsset);
        Debug.Log("Number of Responses: " + numberOfResponses);
        //Debug.Log("Number of Dialogues: " + numberOfDialogues);
        dialogues = new Dialogue[numberOfDialogues]; // initializing an array of Dialogue objects with a size of the total number of dialogue options on a character basis
        assembleDialoguesFromXml(xmlTextAsset); // assemble the dialogue with the helper function
    }

    private void endDialogue(){
        isDialogueActive = false; //We set isDialogueActive to false as the id for the next target equals -1
        dialoguePanel.SetActive(false); // if the dialogue is over we set the dialoguePanel to inactive
        holdForResponse = false; //We set holdForResponse to false as we do not need to wait for the user to respond if the dialogue is over
        currentDialogueIndex = 0; // Reset the currentDialogueIndex to 0 as we do not want any ids to carry over to further dialogues
        dialogueText.text = "";
    }

    private void assembleDialoguesFromXml(TextAsset xmlTextAsset){
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlTextAsset.text);
        dialogueIndex = 0;  // make sure the anytime we load dialogue its index starts at 0. 

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){ //We loop through each node called character under the parent node dialogues.
            if (character.Attributes.GetNamedItem("name").Value == characterName){//we check if the character nodes 'name' attribute is equal to the value of the GameObjects name.
                dialogueIndex = 0; //making sure the dialogueIndex is set to 0
                loadDialogueNodes(xmlDocument); // calling a helper function
            }
        }
    }

    private void loadDialogueNodes(XmlDocument xmlDocument){
       foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){ // we loop through the dialogue node that is a child of character
            dialogues[dialogueIndex] = new Dialogue(); //create a new Dialogue Object for each dialogue we find. Store it in an array.
            dialogues[dialogueIndex].message = dialogueFromXML.Attributes.GetNamedItem("content").Value; //assign message attribute of the Dialogue object to the content of this dialogue node
            choiceIndex = 0; // reset the choice index. 

            dialogues[dialogueIndex].response = new string[2]; //define the size of the response array for this Dialogue Object
            dialogues[dialogueIndex].targetForResponse = new int [2]; //define the size of the targetForResponse array for this Dialogue Object
            loadResponsesNodes(dialogueFromXML); // calling a helper function
            dialogueIndex++; // increment dialogueIndex everytime a Dialogue Object is created
        }
    }

    private void loadResponsesNodes(XmlNode xmlNode){
        foreach(XmlNode choice in xmlNode){ //loop through each choice node that is a child of the corresponding dialogue node
                dialogues[dialogueIndex].response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;//assign the response attribute of the Dialogue object to the choice's content
                dialogues[dialogueIndex].targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value); // assign the targetForResponse attribute of the Dialogue object to the Parsed value of target
                choiceIndex++; //increment choiceIndex everytime a node is complete
        }
    }

    private void handleUserInputForResponse(){
        if (Input.GetKeyDown(KeyCode.Q)){
            currentDialogueIndex = dialogues[currentDialogueIndex].targetForResponse[0];
            holdForResponse = false;
            //Debug.Log("Q is pressed"); //keep for testing
        } else if (Input.GetKeyDown(KeyCode.E)){
            currentDialogueIndex = dialogues[currentDialogueIndex].targetForResponse[1];
            holdForResponse = false;
            //Debug.Log("E is pressed"); // keep for testing
        }
    }

    private void displayDialogue(){ // helper fucntion for testing. In reality the button mapping will not be a keyboard button but a clickable one.
        
        /*
        Debug.Log(dialogues[currentDialogueIndex].message);
        Debug.Log("1: " + dialogues[currentDialogueIndex].response[0]);
        Debug.Log("2: " + dialogues[currentDialogueIndex].response[1]);
        */
        string dialogueToDisplay = "[" + dialogues[currentDialogueIndex].characterName + "]" + " " + dialogues[currentDialogueIndex].message + "\n [A]> " + dialogues[currentDialogueIndex].response[0] + "\n [B]> " + dialogues[currentDialogueIndex].response[1];
        dialogueText.text = dialogueToDisplay;
        //GameObject.Find("dialogueBox").GetComponent<Text>().text = dialogueToDisplay; 
    }

}
