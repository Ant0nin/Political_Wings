using UnityEngine;
using System.Collections.Generic;
using System;

public class SimonManager : MonoBehaviour {

    public string stringSolution = "1;2;3;4;5|3;2;1";
    public int hoverDuration = 1;
    public Camera mainCamera;

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

        public SimonButton(GameObject gameObj)
        {
            this.gameObj = gameObj;
        }
    }

    private List<int[]> listSequences = new List<int[]>();
    private int countButtons;
    private List<int> playerSelection = new List<int>();
    private SimonButton[] buttons;
    private int currentSequence = 0;

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
                    ResetButtons();
                    return false;
                }
            }
            return true;
        }
        return false;
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
                        playerSelection.Add(buttonIndex);
                        bool hasWin = CheckWin();
                        if(hasWin)
                        {
                            currentSequence++;
                            if (currentSequence >= listSequences.Count)
                                EndGame();
                            else
                                ResetButtons();
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
        Destroy(gameObject);
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
                    btnMaterial.color = new Color(1f, 0f, 0f); // red
                    break;

                case ButtonState.SELECTED:
                    btnMaterial.color = new Color(0f, 1f, 0f); // green
                    break;

                case ButtonState.HOVER:
                    btnMaterial.color = new Color(0f, 0f, 1f); // blue
                    break;
            }
        }
    }
}
