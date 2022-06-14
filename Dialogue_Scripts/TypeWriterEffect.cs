using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriterEffect 
{
    private float typingSpeed = 0.03f;

    private char[] stringToCharArray(string line){
        char[] characterArray;
        characterArray = line.ToCharArray();
        return characterArray;
    }

    public IEnumerator typeLine(string line, TextMeshProUGUI textToDisplay){

        textToDisplay.text = "";

        char[] letters = stringToCharArray(line);

        foreach(char letter in letters){
            textToDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    
}
