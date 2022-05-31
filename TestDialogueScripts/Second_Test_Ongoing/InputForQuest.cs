using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputForQuest : InputFromXML
{
    public List<XmlData> readXml(TextAsset xmlTextAsset){
        List<XmlData> quests = new List<XmlData>();
        return quests;
    }

    public void report(){
        Debug.Log("InputForQuest Object created");
    }
}
