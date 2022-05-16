/*
Author: Mark Doghramji
Last Update: 5/16/2022
Version 1.1
Notes:
This class is used in chorus with the DialogueSystem.cs. Basically all dialogue loaded from an xml will need to be created as an object. We can key into the object attributes
as needed to get the dialogue system working properly. This stores basic information about dialogue per character. One major benefit of creating dialogue objects from XMLS
is that we do not need to have dialogue seperated by different files. We have every dialogue from ANY character nested safely in a single file where even someone who is not a 
programmer can easily manipulate. 

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
