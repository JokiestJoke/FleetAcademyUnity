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
    private List<XmlData> dialogues; //declaring an Array for Dialogue objects called dialogues
    private int dialogueIndex = 0; //initialize current dialogueIndex at 0
    private int choiceIndex = 0; //initialize current dialogueIndex at 0

    public List<XmlData> readXml(TextAsset xmlTextAsset){ // needs to be a list?
        //XmlData dialogue = new TestDialogue();
        //numberOfDialogues = calculateNumberOfCoreNodes(xmlTextAsset);
        //dialogues = new TestDialogue[numberOfDialogues];
        dialogues = new List<XmlData>();
        assembleDialoguesFromXml(xmlTextAsset);
        return dialogues;
    }
   
    private void assembleDialoguesFromXml(TextAsset xmlTextAsset){
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlTextAsset.text);
        dialogueIndex = 0;  // make sure the anytime we load dialogue its index starts at 0. 

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){ //We loop through each node called character under the parent node dialogues.
            dialogueIndex = 0; //making sure the dialogueIndex is set to 0
            loadNodes(xmlDocument); // calling a helper function
        }
    }

    private void loadNodes(XmlDocument xmlDocument){
        int testDialogueID = 0;
       foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){ // we loop through the dialogue node that is a child of character
            TestDialogue dialogue = new TestDialogue();
            choiceIndex = 0;
            
            //need someway to take in the name..... other than this test.
            dialogue.name = "TestDialogue ID: " + testDialogueID;
            dialogue.content = dialogueFromXML.Attributes.GetNamedItem("content").Value; //assign message attribute of the Dialogue object to the content of this dialogue node
            dialogue.response = new string[3]; //define the size of the response array for this Dialogue Objec
            dialogue.targetForResponse = new int[3];//define the size of the targetForResponse array for this Dialogue Object
            
            foreach(XmlNode choice in dialogueFromXML){ //loop through each choice node that is a child of the corresponding dialogue node
                dialogue.response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;//assign the response attribute of the Dialogue object to the choice's content
                dialogue.targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value); // assign the targetForResponse attribute of the Dialogue object to the Parsed value of target
                choiceIndex++; //increment choiceIndex everytime a node is complete
            }
            dialogues.Add(dialogue);
            dialogueIndex++; // increment dialogueIndex everytime a Dialogue Object is created
            testDialogueID++;
        }
    }    
}
