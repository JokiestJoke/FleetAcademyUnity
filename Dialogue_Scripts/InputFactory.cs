using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFactory
{
    public InputFromXML create(string inputType){
        string type = inputType.Trim().ToUpper();
        switch(type){
            case "DIALOGUE":
                //Debug.Log("InputForDialogue created");
                return new InputForDialogue();
            case "QUEST":
                //Debug.Log("InputForQuest created");
                return new InputForQuest();    
            default:
                throw new ArgumentException("Incorrect Input Type: The only available type are either InputForDialogue or InputForQuest");
        }
    }
}
