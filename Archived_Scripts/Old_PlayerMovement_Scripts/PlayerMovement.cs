using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [Header("Player Movement UI")]
    [SerializeField] private float speed; // create a speed for Player
    [SerializeField] private float minFOV; // factor which we use to manipulate zoom with Time.timedelta
    [SerializeField] private float maxFOV; // The desired speed we use in Mathf.lerp
    [SerializeField] private float mouseSensitivity; 

    [Header("Player Idle UI")]
    //[Header("Formula =  Timer + longIdleTime")]
    [SerializeField] private float idleTime; // create a speed for Player
    [SerializeField] public float longIdleTime; // create a speed for Player
    public float timer = 0.0f;
    public bool isIdle = false;

    private float spriteAxisRotationY = 0f; // this is for the sprite rotation it should start at 0f.
    private Animator animator;
    private PlayerAnimator playerAnimator;

    private CharacterController characterController;
    private Camera camera;
    private GameObject player;
    
    private Vector3 playerVelocity; 

    private GUIManager GUIManager;

    public float lastVerticalInput;

    private static PlayerMovement instance; // declare instance so we can create a singleton.

    public static PlayerMovement getInstance(){ //basic getInstance singleton we will refer to in our trigger.
        return instance;
    }

    private void Awake(){  // all thats needed in this awake is to error check to make sure there is not more than 1 instance of 
        if (instance != null){
            Debug.LogWarning("Warning: PlayerMovement Singleton -> More than 1 instance!");
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize a new GuiManager, I will use this to control basic GUI functions
        GUIManager = new GUIManager();

        lastVerticalInput = 0;
        
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
        
        animator = GetComponent<Animator>();

        playerAnimator = new PlayerAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        //freeze the player if dialogue is active
        if (DialogueManager.getInstance().isDialogueActive){ /// remember for testing must fix later
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
        checkIfIdle();

       //detect if player is grounded
        if (characterController.isGrounded){
            playerVelocity.y = 0f;
        } else {
        //Apply Gravity to player (if not grounded) 
        playerVelocity.y += -9.18F * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        }  
    }

    private void checkIfIdle(){
        if (!Input.anyKey){
            timer += Time.deltaTime;
            Debug.Log("Idle Timer: " + timer);
            if (timer >= idleTime){
                Debug.Log("Idle State Active at: " + timer);
                isIdle = true;
            }
        } else {
            Debug.Log("A key has been pressed.");
            timer = 0;
            isIdle = false;
        }
    }
    
    private void playerMoveHandler() {
        //WASD input
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");
        lastVerticalInput = vertical;

        playerAnimator.playAnimation(animator);
        
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

    private void updateFieldOfView(float newFOV) {
        camera.fieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV); // helper function basically clamps the fields of view between 2 points
    }

    private void rotateXAxis(float mouseX) {
        //rotate the player based on the X input of the mouse.
        transform.Rotate(Vector3.up * mouseX * 3);
    }

}
