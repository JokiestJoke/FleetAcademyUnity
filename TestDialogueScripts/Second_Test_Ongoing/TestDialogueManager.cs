using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TestDialogueManager : MonoBehaviour
{
    private bool holdForResponse; // delclaring a bool which holds for a players response to dialogue choices
    public bool isDialogueActive; // I will use this bool to check if the dialogue is active currently.

    private int currentDialogueIndex;

    private string characterName;

    private List<XmlData> dialogues;

    private InputFromXML input;
    private InputFactory inputFactory;

    /*
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    //[SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choiceButtons; // choices that will correspond to the Buttons in unity
    private TextMeshProUGUI[] choicesText;
    */

    [Header("XML Document")] // testing purposes
    [SerializeField] private TextAsset xmlDocumentTextAsset;

    private StringAssembler stringAssembler; //declaring a NameAssembler object to stringify names by delimiters

    private static TestDialogueManager instance; // declare instance so we can create a singleton.

    public static TestDialogueManager getInstance(){ //basic getInstance singleton we will refer to in our trigger.
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
        
        stringAssembler = new StringAssembler(); // initializing a new name assembler

        currentDialogueIndex = 0; //initialize current dialogueIndex at 0

        isDialogueActive = false; // declaring the diaologuePanel variable to the GameObject dialogueBox which is a UI element in the unity Heirarchy
        holdForResponse = false; // declaring the diaologuePanel variable to the GameObject dialoguePanel which is a UI element in the unity Heirarchy
        //Attempting to get the Input factory working. Lots of the logic is purely just a test.
        inputFactory = new InputFactory();
        input = inputFactory.create("Dialogue"); //Will need a helper function to work with create, but for now this works.
        dialogues = input.readXml(xmlDocumentTextAsset);

        displayDialogue(dialogues);
        
        //dialoguePanel.SetActive(false); // make sure the dialogue panel is not set to active
        //choicesText = new TextMeshProUGUI[choiceButtons.Length]; //initializing a TextMesh array for the response text. same length as the array holding the GameObject buttons
        //setupButtonsAtStart(); // call helper function to set up buttons at start of the frame.
    }

    // Update is called once per frame
    void Update()
    {
        //manageDialogue();
    }

     private void displayDialogue(List<XmlData> dialogues){
        int dialogueObjects = 0;
        foreach(TestDialogue dialogue in dialogues){
            Debug.Log("------------------------------------");
            Debug.Log("Dialogue Name: " + dialogue.name);
            Debug.Log("Dialogue Content: " + dialogue.content);
            Debug.Log("Response 1: " + dialogue.response[0]);
            Debug.Log("Response 2: " + dialogue.response[1]);
            Debug.Log("Response 3: " + dialogue.response[2]);
            Debug.Log("------------------------------------");


            dialogueObjects++;
        }
        Debug.Log("The total number of Dialogue Objects are: " + dialogueObjects);
        /*
        string dialogueToDisplay = "[" + dialogues[currentDialogueIndex].characterName + "]" + " " + dialogues[currentDialogueIndex].message + "\n [A]> " + dialogues[currentDialogueIndex].response[0] + "\n [B]> " + dialogues[currentDialogueIndex].response[1];
        GameObject.Find("dialogueBox").GetComponent<Text>().text = dialogueToDisplay;
        */
    }



}
