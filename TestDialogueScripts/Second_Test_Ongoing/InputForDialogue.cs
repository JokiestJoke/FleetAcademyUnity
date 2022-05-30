using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputForDialogue : InputFromXML
{

    private StringAssembler stringAssembler = new StringAssembler();
    
    private string characterName; // declaring a string containing the characters name
    private Dialogue[] dialogues; //declaring an Array for Dialogue objects called dialogues
    private int numberOfDialogues; // number of dialogues
    private int currentDialogueIndex; // declaring an initial index of 0

    private int dialogueIndex = 0;
    private int choiceIndex = 0; //initialize current dialogueIndex at 0

    public InputFromXML readFile(TextAsset xmlTextAsset){ // needs to be a list?
        InputFromXML XMLInput = new InputForDialogue();
        return XMLInput;
    }


    public int calculateNumberOfCoreNodes(TextAsset xmlTextAsset){
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
    
}
