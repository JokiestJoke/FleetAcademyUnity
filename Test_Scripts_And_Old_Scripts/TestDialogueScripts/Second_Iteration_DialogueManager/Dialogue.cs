/*
Author: Mark Doghramji
LinkedIn: https://www.linkedin.com/in/mark-doghramji/
Last Update: 5/16/2022
Notes:
This class is used in chorus with the DialogueSystem.cs. Basically all dialogue loaded from an xml will need to be created as an object. We can key into the object attributes
as needed to get the dialogue system working properly. This stores basic information about dialogue per character. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
   public string characterName;
   public string message;
   public string[] response;
   public int[] targetForResponse;
   
}
