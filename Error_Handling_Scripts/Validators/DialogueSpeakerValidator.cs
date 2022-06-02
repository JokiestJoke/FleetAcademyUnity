using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSpeakerValidator : XmlValidator
{
    public void validate(XmlData data, GameObject gameObject){
        Dialogue dialogue = (Dialogue) data;
        if(dialogue.name != gameObject.name){
            throw new InvalidSpeakerException(dialogue.name);
        }
    }
}
