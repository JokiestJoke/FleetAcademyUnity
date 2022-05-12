/*
Author: Mark Doghramji
Last Update: 5/9/2022
Version 1.1
Notes:
This class implements the Billboard effect that the BillboardManager relies on. Continuing the conversation or 'Single Responsibility classes'-- this class's
only responsibility is to create a method that creates the billboard effect. This class will have an instance in Billboard manager to grant access to the method.
Not only that, but the method is public visibility allowing the BillboardManager to use its method. I would have prefered this method not be public visibility 
but it seemed trivial in nature. Please note that this class does not inherit from Unity's MonoBehaviour.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect
{
    public void billboardToCamera(GameObject[] gameObject, Camera camera){ // one day lets revist the merits of this being public versus protected (if we inherited)
        //int countOfObjects = 0; //For testing purposes
        foreach(GameObject gameObj in gameObject){
           
            gameObj.transform.LookAt(camera.transform); // have the gameobjects look at the camera.
            gameObj.transform.rotation = Quaternion.Euler(0f, gameObj.transform.rotation.eulerAngles.y, 0f); // define rotation with new Quaternion to ensure Y axis is only affected.
    
            //Debug.Log("Game Object name: " + gameObj.ToString());// testing purposes
            //countOfObjects++; // testing purposes
        }
        //Debug.Log("Number of GameObjets detected: " + countOfObjects); // testing purposes
    }
}
