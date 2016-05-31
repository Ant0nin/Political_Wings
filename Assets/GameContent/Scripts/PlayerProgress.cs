﻿using UnityEngine;
using System.Collections;

public class PlayerProgress : MonoBehaviour {

    public bool winPuzzleDomino = false;
    public bool winPuzzleLaser = false;
    public bool winPuzzleBoule = false;
    public bool winPuzzleSimon = false;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject); // persistant
    }

    public void WinPuzzleDomino()
    {
        winPuzzleDomino = true;
        TriggerWinAnimation();
        StartCoroutine(BackToMainScene());
    }

    public void WinPuzzleLaser()
    {
        winPuzzleLaser = true;
        TriggerWinAnimation();
        StartCoroutine(BackToMainScene());
    }

    public void WinPuzzleBoule()
    {
        winPuzzleBoule = true;
        TriggerWinAnimation();
        StartCoroutine(BackToMainScene());
    }

    public void WinPuzzleSimon()
    {
        winPuzzleSimon = true;
        TriggerWinAnimation();
        StartCoroutine(BackToMainScene());
    }

    void TriggerWinAnimation()
    {
        GameObject winBox = GameObject.Find("WinBox");
        WinboxAnimation winAnim = winBox.GetComponent<WinboxAnimation>();
        winAnim.Trigger();
    }

    IEnumerator BackToMainScene()
    {
        yield return new WaitForSeconds(1.4f);
        GameObject transitionGameobject = GameObject.Find("TransitionToMainScene");
        if(transitionGameobject != null)
        {
            SceneFadeInOut fading = transitionGameobject.GetComponent<SceneFadeInOut>();
            fading.GoToNextScene();
        }
        
    }
}