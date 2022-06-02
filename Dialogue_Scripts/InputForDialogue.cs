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
    private string dataType; // we will use this "Type" to tell our Input Factory to create
    private List<XmlData> dialogues; //declaring an Array for Dialogue objects called dialogues
    private int dialogueIndex = 0; //initialize current dialogueIndex at 0
    private int choiceIndex = 0; //initialize current dialogueIndex at 0

    public List<XmlData> readXml(TextAsset xmlTextAsset){ // the readXml will return a List of XmlData
        dialogues = new List<XmlData>(); // initialize the dialogues list
        assembleDialoguesFromXml(xmlTextAsset); // assemble the dialogue
        return dialogues; // return the dialogues list.
    }
   
    private void assembleDialoguesFromXml(TextAsset xmlTextAsset){
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlTextAsset.text);
        dialogueIndex = 0;  // make sure the anytime we load dialogue its index starts at 0. 

        foreach(XmlNode character in xmlDocument.SelectNodes("dialogues/character")){ //We loop through each node called character under the parent node dialogues.
            dialogueIndex = 0; //making sure the dialogueIndex is set to 0
            characterName = character.Attributes.GetNamedItem("name").Value;
            //dataType = character.Attributes.GetNamedItem("dataType").ToUpper();
            loadNodes(xmlDocument); // calling a helper function
        }
    }

    private void loadNodes(XmlDocument xmlDocument){
        int dialogueID = 0;
       foreach(XmlNode dialogueFromXML in xmlDocument.SelectNodes("dialogues/character/dialogue")){ // we loop through the dialogue node that is a child of character
            Dialogue dialogue = new Dialogue();
            choiceIndex = 0;
            
            //populate the dialogue Object with the proper fields
            dialogue.id = dialogueID; //assign an ID to the current Dialogue object
            dialogue.dataType = dataType.Trim(); //define the type of data and make sure no whitespace came through
            dialogue.name = characterName; // assign the name of the current speaker of the Dialogue Object
            dialogue.content = dialogueFromXML.Attributes.GetNamedItem("content").Value; //assign message attribute of the Dialogue object to the content of this dialogue node
            dialogue.response = new string[3]; //define the size of the response array for this Dialogue Object
            dialogue.targetForResponse = new int[3];//define the size of the targetForResponse array for this Dialogue Object
            
            foreach(XmlNode choice in dialogueFromXML){ //loop through each choice node that is a child of the corresponding dialogue node
                dialogue.response[choiceIndex] = choice.Attributes.GetNamedItem("content").Value;//assign the response attribute of the Dialogue object to the choice's content
                dialogue.targetForResponse[choiceIndex] = int.Parse(choice.Attributes.GetNamedItem("target").Value); // assign the targetForResponse attribute of the Dialogue object to the Parsed value of target
                choiceIndex++; //increment choiceIndex everytime a node is complete
            }
            dialogues.Add(dialogue);
            dialogueIndex++; // increment dialogueIndex everytime a Dialogue Object is created
            dialogueID++;
        }
    }    
}
