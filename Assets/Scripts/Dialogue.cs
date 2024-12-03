using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dialogue : MonoBehaviour
{
    //fields
    //Window
    public GameObject window;
    //Indicator
    public GameObject indicator;
    //Dialogues list
    public List<string> dialogues;
    //Index on dialogue
    private int index;
    //Character index
    private int charIndex;
    //Started boolean
    private bool started;
    //TextComponent
    public TMP_Text dialogueText;
    //wwriting speed
    public float SpeedOfWriting;
    //wait for next bool
    private bool waitForNext;


    private void Awake()
    {
        ToggleIndicator(false);
        ToggleWindow(false);
    }
    private void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }
    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }
    //Start Dialogue
    public void StartDialogue()
    {
        if (started)
            return;

        //boolean to indicated that we have started
        started = true;
        //Show the window
        ToggleWindow(true);
        //hide the indicator
        ToggleIndicator(false);
        //start with first dialogue
        GetDialogue(0); 

    }
    private void GetDialogue(int i) {
        //start intex at 0
        index = i;
        //Reset the character index
        charIndex = 0;
        //clear the dialogue component text
        dialogueText.text = string.Empty;
        //start writing
        StartCoroutine(Writing());
    }
    //End Dialogue
    public void EndDialogue()
    {
        started = false;
        waitForNext = false;
        StopAllCoroutines();
        //Hide the window
        ToggleWindow(false);
    }

    //Writing logic
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(SpeedOfWriting);
        string currentDialogue = dialogues[index];
        //write the character
        dialogueText.text += currentDialogue[charIndex];
        //increase the character index
        charIndex++;
        //makes sure you have reached the end of the sentence
        if(charIndex < currentDialogue.Length)
        {
            //wait x seconds
            yield return new WaitForSeconds(SpeedOfWriting);
            //restart same process
            StartCoroutine(Writing());
        }
        else
        {
            //end this sentence and wait for the next one
            waitForNext = true;
        } 
    }
    private void Update()
    {
        if (!started)
            return;
        if(waitForNext && Input.GetKeyUp(KeyCode.E))
        {
            waitForNext=false;
            index++;
            if(index < dialogues.Count)
            {
                GetDialogue(index);
            }
            else
            {
                ToggleIndicator(true);
                EndDialogue();
            }
            
        }
    }
}
