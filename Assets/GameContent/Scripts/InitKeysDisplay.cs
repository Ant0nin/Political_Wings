using UnityEngine;
using System.Collections;

public class InitKeysDisplay : MonoBehaviour {

	void Start () {

        PlayerProgress progressComp = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();

        GameObject keyPuzzleDomino = transform.GetChild(0).gameObject;
        GameObject keyPuzzleBoule = transform.GetChild(1).gameObject;
        GameObject keyPuzzleLaser = transform.GetChild(2).gameObject;
        GameObject keyPuzzleSimon = transform.GetChild(3).gameObject;

        if (!progressComp.winPuzzleDomino)
            keyPuzzleDomino.SetActive(false);

        if (!progressComp.winPuzzleBoule)
            keyPuzzleBoule.SetActive(false);

        if (!progressComp.winPuzzleLaser)
            keyPuzzleLaser.SetActive(false);

        if (!progressComp.winPuzzleSimon)
            keyPuzzleSimon.SetActive(false);

        if (progressComp.winPuzzleDomino && progressComp.winPuzzleBoule && progressComp.winPuzzleLaser && progressComp.winPuzzleSimon)
            OpenCoffre();
    }

    void OpenCoffre()
    {
        GameObject porteCoffre = GameObject.Find("DoorCoffre");
        TransformOnClick script = porteCoffre.GetComponent<TransformOnClick>();
        script.enabled = true;
    }
}
