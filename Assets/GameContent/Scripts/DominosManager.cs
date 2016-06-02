using UnityEngine;
using System.Collections;
using System;

public class DominosManager : MonoBehaviour {

    public string stringSolution;
    public Camera cam;
    public float moveSpeed = 0.2F;

    private int countDominos;
    private bool[] gamestate;
    private bool[] solution;
    private Vector3[] upDominos;
    private bool blockInteraction = false;

	void Start ()
    {
        countDominos = transform.childCount;
        gamestate = new bool[countDominos];
        solution = new bool[countDominos];
        upDominos = new Vector3[countDominos];

        string[] arraySolution = stringSolution.Split(';');
        for (int i = 0; i < countDominos; i++)
        {
            gamestate[i] = false;
            solution[i] = Convert.ToInt32(arraySolution[i]) == 0 ? false : true;
            upDominos[i] = transform.GetChild(i).transform.up;
        }
    }

    void Update()
    {
        RefreshGameState();
        UpdateDominosTransform();
        bool win = CheckWin();
        if (win)
        {
            blockInteraction = true;
            EndGame();
        }
    }

    void RefreshGameState()
    {
        if(!blockInteraction && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.IsChildOf(transform))
                {
                    int dominoIndex = 0;
                    for (int i = 0; i < countDominos; i++)
                    {
                        if (transform.GetChild(i) == objectHit)
                        {
                            dominoIndex = i;
                            break;
                        }
                    }

                    gamestate[dominoIndex] = !gamestate[dominoIndex];
                }
            }
        }
    }

    void UpdateDominosTransform()
    {
        for (int i = 0; i < countDominos; i++)
        {
            Transform domino = transform.GetChild(i);
            bool dominoState = gamestate[i];
            if(dominoState == true)
                domino.rotation = Quaternion.Lerp(domino.transform.rotation, Quaternion.LookRotation(Vector3.forward, -upDominos[i]), moveSpeed);
            else
                domino.rotation = Quaternion.Lerp(domino.transform.rotation, Quaternion.LookRotation(Vector3.forward, upDominos[i]), moveSpeed);
        }
    }

    bool CheckWin()
    {
        for(int i=0; i<countDominos; i++)
        {
            if (gamestate[i] != solution[i])
                return false;
        }
        return true;
    }

    void EndGame()
    {
		
        GameObject progressObj = GameObject.Find("PlayerProgress");
        PlayerProgress progressComp = progressObj.GetComponent<PlayerProgress>();
        progressComp.WinPuzzleDomino();
    }
}
