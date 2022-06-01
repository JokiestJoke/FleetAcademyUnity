/*
Author: Mark Doghramji
LinkedIn: https://www.linkedin.com/in/mark-doghramji/
Last Update: 5/26/2022
Notes:
This class manages the Dialogue per NPC and works in chorus with the file DialogueTrigger.cs. Essentially, an NPC will pass a string NPCname variable and a XML file.
The DialogueManager will parse through the nodes of the XML, and populate (a) Dialogue Object(s). These Dialogue Objects will be stored in an array, which I use to logically
handle dialogue and user input. This class is a Singleton as we need exactly 1 DialogueManager. This DialogueManager is attached to a empty game object in the game's
hierarchy.

To do: A quick discussion on Single Responsibility Classes ---> this class in my opinion breaks the SRC theory. In other words, should the manager "manage" the dialogue
and the user input solely. Right now the mananger ALSO parses out the Dialogue Objects from an XML. Ideally, I should have a seperate class called "XMLDialogueReader"
with public methods interacting with this DialogueManager in some manner. In truth I could make an arguement for why breaking or not the SRC principle is or is not a big deal 
in this case. However, I shall in a future attempt try to pivot and commit to a design decision that promotes SRC and hopefully make the code in this file a bit more contained logically
and clean to read.
*/

using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{
    private string characterName; // declaring a string containing the characters name
    private Dialogue[] dialogues; //declaring an Array for Dialogue objects called dialogues
    private int numberOfDialogues; // number of dialogues
    private int currentDialogueIndex; // declaring an initial index of 0;
    
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

    private StringAssembler stringAssembler; //declaring a NameAssembler object to stringify names by delimiters

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

        stringAssembler = new StringAssembler(); // initializing a new name assembler

        currentDialogueIndex = 0; //initialize current dialogueIndex at 0
        dialogueIndex = 0; //initialize current dialogueIndex at 0
        choiceIndex = 0; //initialize current dialogueIndex at 0

        isDialogueActive = false; // declaring the diaologuePanel variable to the GameObject dialogueBox which is a UI element in the unity Heirarchy
        holdForResponse = false; // declaring the diaologuePanel variable to the GameObject dialoguePanel which is a UI element in the unity Heirarchy
        dialoguePanel.SetActive(false); // make sure the dialogue panel is not set to active

        choicesText = new TextMeshProUGUI[choiceButtons.Length]; //initializing a TextMesh array for the response text. same length as the array holding the GameObject buttons
        setupButtonsAtStart(); // call helper function to set up buttons at start of the frame.
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

    private void manageDialogue(){
        if (isDialogueActive){
            if (!holdForResponse){ //if we are waiting for a repsonse - remeber that holdForResponse is by default initiliazed as false
                dialoguePanel.SetActive(true); // if dialogue is active and we are waiting for a response we set the dialoguePanel to active
                if (currentDialogueIndex != -1){// and the dialogue is NOT finished (which -1 would indicate in the XML file under target)
                    displayDialogue(); // then we display dialogue
                    displayResponsesToButtons(); 
                } 
                else { //if the dialogue is -1 then we must end the dialogue
                    endDialogue();
                }
                holdForResponse = true; //we then wait for user input 
            }
        }
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
        holdForResponse = false;// we start the dialogue so we do not hold
        isDialogueActive = true;// we state there is active dialogue
        characterName = npcName;// get the NPC name from the collision

        numberOfDialogues = calculateNumberOfDialogues(xmlTextAsset); // calculate the number of dialogues with a helper function.
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
            
            string NPCname = stringAssembler.assembleString(characterName); //create a NPC name by calling name assembler on characterName. 
            
            dialogues[dialogueIndex].characterName = NPCname; //assign the current dialogue the newly created npc name
            dialogues[dialogueIndex].message = dialogueFromXML.Attributes.GetNamedItem("content").Value; //assign message attribute of the Dialogue object to the content of this dialogue node
            
            choiceIndex = 0; // reset the choice index. 

            dialogues[dialogueIndex].response = new string[3]; //define the size of the response array for this Dialogue Object
            dialogues[dialogueIndex].targetForResponse = new int [3]; //define the size of the targetForResponse array for this Dialogue Object
            
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

    private void displayDialogue(){ // Helper function that displays the message of the current dialogue
        string dialogueTextToDisplay = "[" + dialogues[currentDialogueIndex].characterName + "]" + " " + dialogues[currentDialogueIndex].message; // creating a string to display to the dialogueText GameObject
        dialogueText.text = dialogueTextToDisplay; // the message of the Dialogues's message at the currentDialogueIndex is set to the string that was just created
        //Debug.Log("Number of Responses: " + dialogues[currentDialogueIndex].response.Length);
    }

    private void displayResponsesToButtons(){ // for every Response of the currentDialogueIndex we set the text of the button to the response of the Dialogue.response array.
        int index = 0;
        foreach(string response in dialogues[currentDialogueIndex].response){ //looping through the response array of the dialogue
            Debug.Log("Response: " + response); //for testing
            choiceButtons[index].gameObject.SetActive(true); // activating the buttons to show. By default at game start they are invisible.
            choicesText[index].text = response; // assign the button.text the response(s) of the current dialogue. 
            index++;
        }
        deactivateIdleButtons(index); // call helper function to deactivate 
    }

    private void deactivateIdleButtons(int indexOfLastActiveButton){ //not all dialogues with have the max options. this function will hide the buttons w/o a corresponding response
        int index;
        for(index = indexOfLastActiveButton; index < choiceButtons.Length; index++){
            choiceButtons[index].gameObject.SetActive(false); // set false and idle buttons.
        }
    }

    //this method will be used on the buttons OnClick Function. This will not be called in this file, rather only defined.
    //For future refrence see that the gameObject choiceButtons have DialogueManager.makeResponseChoice() called when the specific buttons is pressed!
    public void makeResponseChoice(int targetForResponseIndex){
        currentDialogueIndex = dialogues[currentDialogueIndex].targetForResponse[targetForResponseIndex];
        holdForResponse = false;
    }
}
