using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private bool holdForResponse; // delclaring a bool which holds for a players response to dialogue choices
    public bool isDialogueActive; // I will use this bool to check if the dialogue is active currently.

    private int currentDialogueIndex;

    private string speaker;
    private string dataType;

    private List<XmlData> dialogues;

    private InputFromXML input;
    private InputFactory inputFactory;

    
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    //[SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choiceButtons; // choices that will correspond to the Buttons in unity
    private TextMeshProUGUI[] choicesText;
    
    private StringAssembler stringAssembler; //declaring a NameAssembler object to stringify names by delimiters

    private static DialogueManager instance; // declare instance so we can create a singleton.

    public static DialogueManager getInstance(){ //basic getInstance singleton we will refer to in our trigger.
        return instance;
    }

    private void Awake(){  // all thats needed in this awake is to error check to make sure there is not more than 1 instance of 
        if (instance != null){
            Debug.LogWarning("Warning: DialogueManager Singleton -> More than 1 instance!");
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        stringAssembler = new StringAssembler(); // initializing a new string assembler to convert gameObject names to a clean string
        
        dataType = "Dialogue"; //<--- the DialogueManager Shoudlnt be deciding this....Mark you need to revist this...
        
        currentDialogueIndex = 0; //initialize current dialogueIndex at 0
        isDialogueActive = false; // declaring the diaologuePanel variable to the GameObject dialogueBox which is a UI element in the unity Heirarchy
        holdForResponse = false; // declaring the diaologuePanel variable to the GameObject dialoguePanel which is a UI element in the unity Heirarchy
        
        inputFactory = new InputFactory();// initialize the InputFactory
        input = inputFactory.create(dataType); //Will need a helper function to work with create, but for now this works.

        dialoguePanel.SetActive(false); // make sure the dialogue panel is not set to active
        choicesText = new TextMeshProUGUI[choiceButtons.Length]; //initializing a TextMesh array for the response text. same length as the array holding the GameObject buttons
        setupButtonsAtStart(); // call helper function to set up buttons at start of the frame.
    }

    // Update is called once per frame
    void Update()
    {
        manageDialogue();
    }

    private void setupButtonsAtStart(){ //helper function to setup the buttons with the TextMeshPROGUI components attached to them. essential for displaying text.
        int choiceIndex = 0;
        foreach(GameObject choice in choiceButtons){
            choicesText[choiceIndex] = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceIndex++;
        }
    }

    private void manageDialogue(){
        if (isDialogueActive){
            if (!holdForResponse){ //if we are waiting for a repsonse - remeber that holdForResponse is by default initiliazed as false
                dialoguePanel.SetActive(true); // if dialogue is active and we are waiting for a response we set the dialoguePanel to active
                if (currentDialogueIndex != -1){// and the dialogue is NOT finished (which -1 would indicate in the XML file under target)
                    displayDialogue(); // then we display dialogue
                    displayResponsesToButtons(); 
                } 
                else { //if the dialogue is -1 then we must end the dialogue
                    endDialogue();
                }
                holdForResponse = true; //we then wait for user input 
            }
        }
    }

    public void startDialogue(TextAsset xmlTextAsset, string npcName){ // helper function that makes sure our dialogues start properly.
        holdForResponse = false;// we start the dialogue so we do not hold
        isDialogueActive = true;// we state there is active dialogue
        speaker = stringAssembler.assembleString(npcName);
        dialogues = input.readXml(xmlTextAsset); //Call read
    }

    private void endDialogue(){
        isDialogueActive = false; //We set isDialogueActive to false as the id for the next target equals -1
        dialoguePanel.SetActive(false); // if the dialogue is over we set the dialoguePanel to inactive
        holdForResponse = false; //We set holdForResponse to false as we do not need to wait for the user to respond if the dialogue is over
        currentDialogueIndex = 0; // Reset the currentDialogueIndex to 0 as we do not want any ids to carry over to further dialogues
        dialogueText.text = "";
    }

    private void displayDialogue(){ // Helper function that displays the message of the current dialogue
        Dialogue currentDialogue = (Dialogue) dialogues[currentDialogueIndex];
        if (speaker == currentDialogue.name){
            string dialogueTextToDisplay = "[" + currentDialogue.name + "]" + " " + currentDialogue.content;
            dialogueText.text = dialogueTextToDisplay; // the message of the Dialogues's message at the currentDialogueIndex is set to the string that was just created        
        } else {
            throw new InvalidSpeakerException(speaker);
        }
    }

    private void displayResponsesToButtons(){ // for every Response of the currentDialogueIndex we set the text of the button to the response of the Dialogue.response array.
        int index = 0;
        Dialogue currentDialogue = (Dialogue) dialogues[currentDialogueIndex]; // set the current Dialogue and cast it explicitly into a TestDialogue Object
        foreach(string response in currentDialogue.response){
            choiceButtons[index].gameObject.SetActive(true); // activating the buttons to show. By default at game start they are invisible.
            choicesText[index].text = response; // assign the button.text the response(s) of the current dialogue.
            index++;
        }
        deactivateIdleButtons(index); // call helper function to deactivate    
    }

    private void deactivateIdleButtons(int indexOfLastActiveButton){ //not all dialogues with have the max options. this function will hide the buttons w/o a corresponding response
        int index;
        for(index = indexOfLastActiveButton; index < choiceButtons.Length; index++){
            choiceButtons[index].gameObject.SetActive(false); // set false and idle buttons.
        }
    }
    //this method will be used on the buttons OnClick Function. This will not be called in this file, rather only defined.
    //For future refrence see that the gameObject choiceButtons have DialogueManager.makeResponseChoice() called when the specific buttons is pressed!
    public void makeResponseChoice(int targetForResponseIndex){
        Dialogue currentDialogue = (Dialogue) dialogues[currentDialogueIndex]; //define the current Dialogue from the dialogues List
        currentDialogueIndex = currentDialogue.targetForResponse[targetForResponseIndex]; //define the currentDialogueIndex based off the targetForResponseIndex
        holdForResponse = false;
    }
}
