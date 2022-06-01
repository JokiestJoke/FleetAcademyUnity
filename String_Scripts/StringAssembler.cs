/*
Author: Mark Doghramji
LinkedIn: https://www.linkedin.com/in/mark-doghramji/
Last Update: 5/27/2022
Notes:
This class is a basic helper class that assembles strings. Many of my GameObjects, dialogue files, character names, .png files follow a naming
convention such as: Tiberius_Claudius.png. Even GameObjects (like NPCs) can be named Tiberius_Claudius. This class aims at 
creatings strings while avoiding delimiters that may be found in named gameObjects or files. Since '_' is a common delimeter in many of my file names, it made sense 
to create a class that would save me repeating code accros other C# files. An example is this: a NPC gameObject is named Tiberius_Claudius. 
Say I wish to display this name during Dialogue by using gameObject.name. I would not want to display the string object "Tiberius_Claudius" but rather isolate substrings 
by delimiters chars (such as '_') and reconstruct the substrings into a string such as "Tiberius Claudius". The beauty of this class is that if I wish to add delimiters 
that I find common amongst file names in my hierarchy, I need only add the new delimiting char to the splitApartString method. Building this class allows the code to be reusable
throughtout my game in many files.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringAssembler
{
    private string[] splitApartString(string stringToSplit){
        char[] delimiterChars = {'_'}; // define a char array of delimiters that will create substrings
        string[] substringArray = stringToSplit.Split(delimiterChars); //create a string array of substrings divided by the delimiters
        return substringArray; // return the array of substrings
    }

    private string createStringFromArray(string[] substringArray){ // split a string apart by delimiters
        string newString = ""; // declare and initialize a empty string object
        foreach(string substring in substringArray){// loop through the substrings in the array and add on to a string object
            newString += substring + " "; // basic formula for how the string will come out (i.e. "Tiberius Claudius Nero")
        }
        newString = newString.Trim(); // eliminate any trailing or preceding white space.
        return newString;
    }

    public string assembleString(string targetString){ // public function that will asseble strings based on delimiters
        string[] substringArray = splitApartString(targetString);
        string assembledString = createStringFromArray(substringArray);
        return assembledString;
    }
}
