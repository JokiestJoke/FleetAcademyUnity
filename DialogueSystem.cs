/*
Author: Mark Doghramji
Last Update: 5/16/2022
Version 1.1
Notes:
This class implements the basic dialogue system for the game. I need to do a bit more code refracting so that this class truly follows the single responsibility principle. 
Continuing the conversation of 'Single Responsibility classes'-- this class's sole responsibility should be managinge the dialogue of the game. Essentially what this class
does is that it loads the dialogue from a seperated XML file, and prepares the dialogue by creating Dialogue Objects (See Dialogue.cs). These Objects have responses, and a target
where the dialogue should shoot to (by index) when a player interacts with the NPC starting the conversation. Currently the dialogue just goes to console. My next big step
will be allowing the Unity UI to have clickable buttons to navigate the dialogue choices. One major benefit of creating dialogue System and how it interacts with Dialogue.cs is that 
we do not need to have dialogue seperated by different files. We have every dialogue from ANY character nested safely in a single file where even someone who is not a 
programmer can easily manipulate. 
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI; // this is very important to import this library so that this file can manipulate the unity UI I created


public class DialogueSystem : MonoBehaviour
{
    string characterName; // declaring a string containing the characters name
    Dialogue[] dialogues; //declaring an Array for Dialogue objects called dialogues
    int numberOfDialogues; // declaring an integer for total number of dialogues on a character basis
    int currentDialogueIndex = 0; // declaring an initial index of 0;
    
    bool holdForResponse = false; // delclaring a bool which holds for a players response to dialogue choices
    bool isDialogueActive = false; // I will use this bool to check if the dialogue is active currently.

    GameObject dialogueBox;
    GameObject dialoguePanel;
    
    
    int dialogueIndex = 0;
    int choiceIndex = 0;


    // Start is called before the first frame update
    void Start()
    {


        dialogueBox = GameObject.Find("dialogueBox"); // declaring the diaologuePanel variable to the GameObject dialogueBox which is a UI element in the unity Heirarchy
        dialoguePanel = GameObject.Find("dialoguePanel"); // declaring the diaologuePanel variable to the GameObject dialoguePanel which is a UI element in the unity Heirarchy

        GameObject.Find("dialogueImage").GetComponent<RawImage>().texture = Resources.Load<Texture2D>(gameObject.name) as Texture2D; //dialogue image for each NPC for the specific dialogue

        dialoguePanel.SetActive(false); // we basically DO NOT want the dialogue panel to block the screen at the start of the game.



        characterName = gameObject.name;   // the game object that this script is attached to will give define the character name;
        numberOfDialogues = calculateNumberOfDialogues(); // calculate the number of dialogues with a helper function.
        dialogues = new Dialogue[numberOfDialogues]; // initializing an array of Dialogue objects with a size of the total number of dialogue options on a character basis
        assembleDialogueFromXml(); // assemble the dialogue with the helper function
        startDialogue(); // after the dialogue is assembled we now start the dialogue by calling this helper functions


    }

    // Update is called once per frame
    void Update()
    {

        handleDialogue(); // we call this helper function once every frame.

    }

    private void assembleDialogueFromXml(){
        TextAsset textAsset = (TextAsset)Resources.Load("dialogues"); // load a file named dialogues.xml in the Resources folder under Assets
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);
        dialogueIndex = 0;  // make sure the anytime we load dialogue its index starts at 0. 

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){//We loop through Character Nodes 
            if (character.Attributes.GetNamedItem("name").Value == characterName){
                //We loop through each node called character under the parent node dialogues.
                //we check if the character nodes 'name' attribute is equal to the value of the GameObjects name.
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

    private int calculateNumberOfDialogues(){
        TextAsset textAsset = (TextAsset)Resources.Load("dialogues");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);
        int dialogueIndex = 0;

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){
            if (character.Attributes.GetNamedItem("name").Value == characterName){
                foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){
                    dialogueIndex++;
                }
            }
        }
        return dialogueIndex;
    }

    private void handleDialogue(){
        if (isDialogueActive){
            if (!holdForResponse){ //if we are waiting for a repsonse - remeber that holdForResponse is by default initiliazed as false
                dialoguePanel.SetActive(true); // if dialogue is active and we are waiting for a response we set the dialoguePanel to active
                if (currentDialogueIndex != -1){// and the dialogue is NO over (which -1 would indicate in the XML file under target)
                    displayDialogue(); // then we display dialogue
                } 
                else { //if the dialogue is -1 then we must end the dialogue
                
                    isDialogueActive = false; //We set isDialogueActive to false as the id for the next target equals -1
                    dialoguePanel.SetActive(false); // if the dialogue is over we set the dialoguePanel to inactive
                    holdForResponse = false; //We set holdForResponse to false as we do not need to wait for the user to respond if the dialogue is over
                    currentDialogueIndex = 0; // Reset the currentDialogueIndex to 0 as we do not want any ids to carry over to further dialogues

                }
                holdForResponse = true; //we then wait for user input 
            } else {
                handleUserInputForResponse();
            }
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

        string dialogueToDisplay = "[" + gameObject.name + "]" + " " + dialogues[currentDialogueIndex].message + "\n [A]> " + dialogues[currentDialogueIndex].response[0] + "\n [B]> " + dialogues[currentDialogueIndex].response[1];
        GameObject.Find("dialogueBox").GetComponent<Text>().text = dialogueToDisplay;
    }

    private void startDialogue(){ // hlper function that makes sure our dialogues start properly.
        holdForResponse = false;
        isDialogueActive = true;
    }


}
