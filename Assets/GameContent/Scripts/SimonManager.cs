using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class SimonManager : MonoBehaviour {

    public string stringSolution = "1;2;3;4;5|3;2;1";
    public int hoverDuration = 1;
    public Camera mainCamera;
    public float demoSpeed = 0.5f;

    private enum ButtonState {
        UNSELECTED,
        HOVER,
        SELECTED
    };

    private class SimonButton
    {
        public ButtonState state = ButtonState.UNSELECTED;
        public int timerHover = 0;
        public GameObject gameObj;
        public AudioSource audioSound;

        public SimonButton(GameObject gameObj)
        {
            this.gameObj = gameObj;
            audioSound = gameObj.GetComponent<AudioSource>();
        }
    }

    private List<int[]> listSequences = new List<int[]>();
    private int countButtons;
    private List<int> playerSelection = new List<int>();
    private SimonButton[] buttons;
    private int currentSequence = 0;
    private bool blockInteraction = true;

	void Start () {

        InitSequences();
        InitButtons();
	}

    void InitButtons()
    {
        countButtons = transform.childCount;
        buttons = new SimonButton[countButtons];
        for (int i = 0; i < countButtons; i++)
        {
            buttons[i] = new SimonButton(transform.GetChild(i).gameObject);
        }
        ResetButtons();
        SequenceDemo();
    }

    void InitSequences()
    {
        string[] stringAllSequences = stringSolution.Split('|');
        foreach (string stringSequence in stringAllSequences)
        {
            string[] arraySequence = stringSequence.Split(';');
            int[] sequence = new int[arraySequence.Length];
            for (int i = 0; i < arraySequence.Length; i++) {
                sequence[i] = Convert.ToInt32(arraySequence[i]);
            }
            listSequences.Add(sequence);
        }
    }
	
	void Update ()
    {
        ManageHoverButtons();
        if(!blockInteraction)
            ManagePlayerInteractions();
        RefreshButtonsMaterials();
    }

    bool CheckWin()
    {
        int countEnabledButtons = listSequences[currentSequence].Length;
        if(playerSelection.Count >= countEnabledButtons)
        {
            for(int i=0; i< countEnabledButtons; i++)
            {
                if (playerSelection[i] != listSequences[currentSequence][i])
                {
                    SequenceDemo();
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    void SequenceDemo()
    {
        blockInteraction = true;
        ResetButtons();
        StartCoroutine(playButton(0));
    }

    IEnumerator playButton(int indexInSeq)
    {
        yield return new WaitForSeconds(demoSpeed);

        int[] sequenceToPlay = listSequences[currentSequence];
        if (indexInSeq == (sequenceToPlay.Length)) {
            ResetButtons();
            blockInteraction = false;
        }
        else
        {
            int buttonId = sequenceToPlay[indexInSeq] - 1;
            SimonButton btn = buttons[buttonId];

            btn.state = ButtonState.SELECTED;
            btn.audioSound.Play();

            StartCoroutine(playButton(++indexInSeq));
        }
    }

    void ResetButtons()
    {
        int countEnabledButtons = listSequences[currentSequence].Length;
        playerSelection.Clear();
        for (int i = 0; i<countButtons; i++)
        {
            SimonButton btn = buttons[i];
            btn.state = ButtonState.UNSELECTED;

            if (i < countEnabledButtons)
                btn.gameObj.SetActive(true);
            else
                btn.gameObj.SetActive(false);
        }
            
    }

    void ManagePlayerInteractions()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            if (objectHit.IsChildOf(transform))
            {
                string objName = objectHit.name;
                char charID = objName[objName.Length - 1];
                int buttonIndex = (int)Char.GetNumericValue(charID); // start from 1
                SimonButton targetButton = buttons[buttonIndex-1];

                if(targetButton.state != ButtonState.SELECTED)
                {
                    if (Input.GetMouseButtonDown(0)) { // on click
                        targetButton.state = ButtonState.SELECTED;
                        targetButton.audioSound.Play();
                        playerSelection.Add(buttonIndex);
                        bool hasWin = CheckWin();
                        if(hasWin)
                        {
                            currentSequence++;
                            if (currentSequence >= listSequences.Count)
                                EndGame();
                            else
                                SequenceDemo();
                        }
                    }
                    else { // on hover
                        targetButton.timerHover = hoverDuration;
                    }
                }
            }
        }
    }

    void EndGame()
    {
        PlayerProgress progressComp = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();
        progressComp.WinPuzzleSimon();
    }

    void ManageHoverButtons()
    {
        for (int i = 0; i < countButtons; i++)
        {
            SimonButton targetButton = buttons[i];
            if (targetButton.state != ButtonState.SELECTED)
            {
                if (targetButton.timerHover > 0)
                    targetButton.state = ButtonState.HOVER;
                else
                    targetButton.state = ButtonState.UNSELECTED;
            }
            targetButton.timerHover--;
        }
    }

    void RefreshButtonsMaterials()
    {
        for(int i=0; i<countButtons; i++)
        {
            SimonButton targetButton = buttons[i];
            Material btnMaterial = targetButton.gameObj.GetComponent<Renderer>().material;
            switch (targetButton.state)
            {
                case ButtonState.UNSELECTED:
				btnMaterial.color = new Color(0.2f, 0.2f, 0.2f); // black
                    break;

                case ButtonState.SELECTED:
				btnMaterial.color = new Color(1f, 1f, 1f); // green
                    break;

                case ButtonState.HOVER:
				btnMaterial.color = new Color(1f, 1f, 1f); // blue
                    break;
            }
        }
    }
}
