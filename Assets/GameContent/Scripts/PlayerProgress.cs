using UnityEngine;
using System.Collections;
using PlayMaker;
using HutongGames.PlayMaker;

public class PlayerProgress : MonoBehaviour {

	public GameObject tableauSimon;
	public GameObject tableauDominos;

    public bool winPuzzleDomino = false;
    public bool winPuzzleLaser = false;
    public bool winPuzzleBoule = false;
    public bool winPuzzleSimon = false;

    public void WinPuzzleDomino()
    {
        winPuzzleDomino = true;
        TriggerWinAnimation();
        //StartCoroutine(BackToMainScene());
    }

    public void WinPuzzleLaser()
    {
        winPuzzleLaser = true;
        TriggerWinAnimation();
       // StartCoroutine(BackToMainScene());
    }

    public void WinPuzzleBoule()
    {
        winPuzzleBoule = true;
        TriggerWinAnimation();
        //StartCoroutine(BackToMainScene());
    }

    public void WinPuzzleSimon()
    {
        winPuzzleSimon = true;
        TriggerWinAnimation();

        //StartCoroutine(BackToMainScene());
    }

    void TriggerWinAnimation()
    {
        GameObject winBox = GameObject.Find("WinBox");
        WinboxAnimation winAnim = winBox.GetComponent<WinboxAnimation>();
        winAnim.Trigger();
    }

    public IEnumerator BackToMainScene()
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
