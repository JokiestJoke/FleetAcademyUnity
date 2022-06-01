/*
Author: Mark Doghramji
LinkedIn: https://www.linkedin.com/in/mark-doghramji/
Last Update: 5/26/2022
Notes:

*/
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public interface InputFromXML
{


    public List<XmlData> readXml(TextAsset xmlTextAsset);


    //public XmlData[] readFile(TextAsset xmlTextAsset); // gonna try a List instead.
}