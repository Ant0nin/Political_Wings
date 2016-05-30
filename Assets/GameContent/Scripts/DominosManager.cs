using UnityEngine;
using System.Collections;
using System;

public class DominosManager : MonoBehaviour {

    public string stringSolution;
    public Camera mainCamera;
    public float moveSpeed = 0.5F;

    private int countDominos;
    private bool[] gamestate;
    private bool[] solution;

	void Start ()
    {
        countDominos = transform.childCount;
        gamestate = new bool[countDominos];
        solution = new bool[countDominos];

        string[] arraySolution = stringSolution.Split(';');
        for (int i = 0; i < countDominos; i++)
        {
            gamestate[i] = false;
            solution[i] = Convert.ToInt32(arraySolution[i]) == 0 ? false : true;
        }
    }

    void Update()
    {
        RefreshGameState();
        UpdateDominosTransform();
        bool win = CheckWin();
        if (win)
            EndGame();
    }

    void RefreshGameState()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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
                domino.rotation = Quaternion.Lerp(domino.transform.rotation, Quaternion.LookRotation(Vector3.forward, Vector3.up), moveSpeed);
            else
                domino.rotation = Quaternion.Lerp(domino.transform.rotation, Quaternion.LookRotation(Vector3.forward, Vector3.down), moveSpeed);
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
        Destroy(gameObject);
    }
}
