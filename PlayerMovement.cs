using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField] private float speed; // create a speed for Player
    [SerializeField] private float minFOV; // factor which we use to manipulate zoom with Time.timedelta
    [SerializeField] private float maxFOV; // The desired speed we use in Mathf.lerp
    [SerializeField] private float mouseSensitivity;
    
    private float spriteAxisRotationY = 0f; // this is for the sprite rotation it should start at 0f.

    
    private CharacterController characterController;
    private Camera camera;
    private GameObject player;
    
    private Vector3 playerVelocity; 

     private GUIManager GUIManager;

    // Start is called before the first frame update
    void Start()
    {
        //initialize a new GuiManager, I will use this to control basic GUI functions
        GUIManager = new GUIManager();
        
        //initializing character Controller component
        characterController = GetComponent<CharacterController>();
        if (characterController == null){
            Debug.Log("Error: No Character Controler Attached to Player GameObject. Please Review.");
        }
        //Initializing Camera Main
        camera = Camera.main;
        //lock camera to game screen
        Cursor.lockState = CursorLockMode.Locked;
        
        player = GameObject.Find("Player");// cache the player gameObject


    }

    // Update is called once per frame
    void Update()
    {
        //freeze the player if dialogue is active
        if (DialogueManager.getInstance().isDialogueActive){
            GUIManager.enableGUIMouseControl();
            return;
        } else {
            GUIManager.disableGUIMouseControl();
        }
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // attempting to capture the mouse movement to affect the camera.
        
        playerMoveHandler();
        cameraZoomHandler();
        rotateXAxis(mouseX);

        
        
    }
    private void FixedUpdate() {
       //detect if player is grounded
        if (characterController.isGrounded){
            playerVelocity.y = 0f;
        } else {
        //Apply Gravity to player (if not grounded) 
        playerVelocity.y += -9.18F * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        }  
        
    }

    private void playerMoveHandler()
    {
        //WASD input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // moving character with WASD input
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        characterController.Move(movement * Time.deltaTime * speed);
    }

    private void cameraZoomHandler(){
        //define the mousewheel input  (will return "1" Up or "-1" down)
        float MouseWheelInput = Input.GetAxis("Mouse ScrollWheel"); // capture the mousewheel input
        
        if (MouseWheelInput > 0f){ //checking for mousewheel in
            updateFieldOfView(camera.fieldOfView - 3);
        }

        if (MouseWheelInput < 0f){ //checking for mousewheel outs
            updateFieldOfView(camera.fieldOfView + 3);
        }      
    }

    private void updateFieldOfView(float newFOV){
        camera.fieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV); // helper function basically clamps the fields of view between 2 points
    }

    private void rotateXAxis(float mouseX){
        //rotate the player based on the X input of the mouse.
        transform.Rotate(Vector3.up * mouseX * 3);
    }

}
