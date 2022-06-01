/*
Author: Mark Doghramji
Last Update: 5/9/2022
Version 1.1
Notes:
This class acts as the manager of all GameObjects that wish to be billboarded. This class relies on the creation of a GameObject Array, where we store all the 
GameObjects that I wish to billboard. Not only that, but I created a UI element for unity where I can drag GameObjects from the scene manager into the GameObject
Array. A quick discussion on 'Single Responsibility' and classes. While making this class I weighed whether it was the BillboardManager's responsibillty to give the
Billboarding effect, or was its responsibility simply to manage the GameObjects that wish to be 'billboarded'. I weighed the cost/benefits and decided the best
approach would be to let this class only be responsible for managing the GameObjects and I would create another class called 'BillboardEffect' to literally deal soley
with the single responsibility of handling the Billboarding effect. This made it easy to create a new instance of the effect and simply call it on the GameObject Array
and main camera. Another important note is that this manager is a singleton class. We DO NOT want another intance of this object and since we only ever need 1, the
singleton pattern works nicely.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardManager : MonoBehaviour
{


    [Header("Billboard UI")]
    [SerializeField] private GameObject[] objectsToBillboard;

    private static BillboardManager instance; // declaring an instance of the singleton

    private Camera camera; // declare the camera, initialize it in start()

    private BillboardEffect billBoardEffect; // declare the billboard effect object we will call its public billboardToCamera method.

    private static BillboardManager getInstance(){ 
        return instance;
    }

    void Awake()
    {
        if(instance != null){ // error checking to make sure there are not 2 or more managers.
            Debug.LogWarning("There is more than 1 instance of the Billboard Manager!");
        }
        instance = this; //Assign the instance of BillboardManager to this
    }

    // Start is called before the first frame update
    void Start()
    { 
        camera = Camera.main; //Initializing Camera Main & cache it
        billBoardEffect = new BillboardEffect(); // assigning the instance to a new BillboardEffect class. 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        billBoardEffect.billboardToCamera(objectsToBillboard, camera); // calling the BillboardEffect's public method on the GameObject Array & the main camera
    }

}
