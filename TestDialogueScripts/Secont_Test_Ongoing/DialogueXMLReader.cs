using System.Collections;
using System.Collections.Generic;
//using System.Xml;


public class DialogueXMLReader : XMLReader
{
    private string characterName; // declaring a string containing the characters name
    private Dialogue[] dialogues; //declaring an Array for Dialogue objects called dialogues
    private int numberOfDialogues; // number of dialogues
    private int currentDialogueIndex; // declaring an initial index of 0;


    public static void Main(string[] args){ //for testing purposes...
        
    }


    /*The "core" XMLNodes of an Dialogue XML are the <dialogue></dialogue> nodes. We need to know the total number of "Dialogues" to populate
    the Dialogue Objects Array we wish to be creating.*/
    protected override int calculateNumberOfCoreXMLNodes(){
        return -1; 
    }




}
