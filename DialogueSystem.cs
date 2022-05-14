using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;


public class DialogueSystem : MonoBehaviour
{
    string characterName; // declaring a string containing the characters name
    Dialogue[] dialogues; //declaring an Array for Dialogue objects called dialogues
    int numberOfDialogues; // declaring an integer for total number of dialogues on a character basis
    int currentDialogueIndex = 0; // declaring an initial index of 0;
    bool holdForResponse = false; // delclaring a bool 


    // Start is called before the first frame update
    void Start()
    {
        characterName = gameObject.name;   // the game object that this script is attached to will give define the character name;
        numberOfDialogues = calculateNumberOfDialogues(); // calculate the number of dialogues with a helper function.
        dialogues = new Dialogue[numberOfDialogues]; // initializing an array of Dialogue objects with a size of the total number of dialogue options on a character basis
        loadDialogueFromXML();
        numberOfDialogues = calculateNumberOfDialogues(); // calculate the number of dialogues with a helper function.

        for (int i = 0; i < numberOfDialogues; i++)
        {

            print("Message:" + dialogues[i].message);
            print("Answer A:" + dialogues[i].response[0]);
            print("Answer B:" + dialogues[i].response[1]);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadDialogueFromXML(){
        TextAsset textAsset = (TextAsset)Resources.Load("dialogues"); // load a file named dialogues.xml in the Resources folder under Assets
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);
        int dialogueIndex = 0;

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){//We loop through Character Nodes 
            if (character.Attributes.GetNamedItem("name").Value == characterName){
                //We loop through each node called character under the parent node dialogues.
                //we check if the character nodes 'name' attribute is equal to the value of the GameObjects name.
                dialogueIndex = 0; 

                foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){ // we loop through the dialogue node that is a child of character
                    dialogues[dialogueIndex] = new Dialogue(); //create a new Dialogue Object for each dialogue we find. Store it in an array.
                    dialogues[dialogueIndex].message = dialogueFromXML.Attributes.GetNamedItem("content").Value; //assign message attribute of the Dialogue object to the content of this dialogue node
                    int choiceIndex = 0; 

                    dialogues[dialogueIndex].response = new string[2]; //define the size of the response array for this Dialogue Object
                    dialogues[dialogueIndex].targetForResponse = new int [2]; //define the size of the argetForResponse array for this Dialogue Object
                    
                    foreach(XmlNode choice in dialogueFromXML){ //loop through each choice node that is a child of the corresponding dialogue node
                        dialogues[dialogueIndex].response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;//assign the response attribute of the Dialogue object to the choice's content
                        dialogues[dialogueIndex].targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value); // assign the targetForResponse attribute of the Dialogue object to the Parsed value of target
                        choiceIndex++; //increment choiceIndex everytime a node is complete
                    }
                    dialogueIndex++; // increment dialogueIndex everytime a Dialogue Object is created
                }
            }
        }
    }
    //private void populateDialogue

    //private void populateResponses


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

    //helper function
    //countDialogues

}
