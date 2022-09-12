/*
Author: Mark Doghramji
Last Updated: 9/12/2022
Version: 1.0
Notes: This script moves the player and handles gravity for a 3rd person game. Please note that this script works with Cinemachine to bring a seemless
and cost effective means for a player to interact with the camera. 

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    
    [Header("Camera UI")]
    [SerializeField] private Transform thirdPersonCamera; // make sure this corresponds with the MAIN camera.
    
    [Header("Player Movement UI")]
    [SerializeField] private float speed = 6f; // create a speed for Player
    [SerializeField] private float smoothTurnTime= 0.1f;
    //[SerializeField] private float mouseSensitivity; 
    
    private float turnSmoothVelocity;
    private Vector3 playerVelocity;
    
    
    private GUIManager GUIManager;
    /*
    [Header("Player Idle UI")]
    //[Header("Formula =  Timer + longIdleTime")]
    [SerializeField] private float idleTime; // create a speed for Player
    [SerializeField] public float longIdleTime; // create a speed for Player
    public float timer = 0.0f;
    public bool isIdle = false;
    */
    private CharacterController characterController; //cache the Character Controller GameObject

    // Start is called before the first frame update
    void Start()
    {
        //initializing character Controller component
        characterController = GetComponent<CharacterController>();
    
        //lock camera to game screen
        Cursor.lockState = CursorLockMode.Locked;

        //initializing a GUIManager
        GUIManager = new GUIManager();
        
    }
    // Update is called once per frame
    void Update()
    {
        //handle all player movement
        playerMovement();

        //handle the dialogueMode
        hangleDialogueMode();
        
    }

    void FixedUpdate()
    {
        //handle player gravity
        playerGravity();
    
    }

    private void playerGravity(){
        //if the character is grounded no gravity needed
        if (characterController.isGrounded){
            playerVelocity.y = 0f; 
        } else {
            //Apply Gravity to player (if not grounded) 
            playerVelocity.y += -9.18F * Time.deltaTime;
            characterController.Move(playerVelocity * Time.deltaTime);
        }
    }

    private void playerMovement() {
        //Horizontal will return input between -1 ... 1. -1 if A Key, 0 if none, 1 is B key
        float horizontal = Input.GetAxisRaw("Horizontal");
        //Vertical will return input between -1 ... 1. -1 if W Key, 0 if none, 1 is S key
        float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        //Storing the X and Z directional movements of the player. we normalize so that 2 keys dont double the speed.
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (direction.magnitude >= 0.1f) {
            //Atan2 is a math function I will use to calculate the Target Angle the player should be pointing.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + thirdPersonCamera.eulerAngles.y;
            //Capturing an angle that the player should be pointing (corresponds with camera)
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // tranforming the rotation of the player
            //Creating a movement firection for the player
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //telling the corresponding "Character Controller" to move the player based on the input.
            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);

        }
    }

    private void hangleDialogueMode(){
        //freeze the player if dialogue is active
        if (DialogueManager.getInstance().isDialogueActive){ /// remember for testing must fix later
            GUIManager.enableGUIMouseControl();
            return;
        } else {
            GUIManager.disableGUIMouseControl();
        }
    }
}
