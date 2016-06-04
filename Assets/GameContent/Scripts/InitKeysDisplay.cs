using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InitKeysDisplay : MonoBehaviour {

	public GameObject tabloDomino;
	public GameObject tabloBoules;
	public GameObject tabloLaser;
	public GameObject tabloSimon;

	public GameObject endScenario;

	void Start () {

        PlayerProgress progressComp = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();

        GameObject keyPuzzleDomino = transform.GetChild(0).gameObject;
        GameObject keyPuzzleBoule = transform.GetChild(1).gameObject;
        GameObject keyPuzzleLaser = transform.GetChild(2).gameObject;
        GameObject keyPuzzleSimon = transform.GetChild(3).gameObject;

        if (!progressComp.winPuzzleDomino)
            keyPuzzleDomino.SetActive(false);
		else
			Destroy(tabloDomino);
		
        if (!progressComp.winPuzzleBoule)
            keyPuzzleBoule.SetActive(false);
		else
			Destroy(tabloBoules);

        if (!progressComp.winPuzzleLaser)
            keyPuzzleLaser.SetActive(false);
		else
			Destroy(tabloLaser);

        if (!progressComp.winPuzzleSimon)
            keyPuzzleSimon.SetActive(false);
		else
			Destroy(tabloSimon);

        if (progressComp.winPuzzleDomino && progressComp.winPuzzleBoule && progressComp.winPuzzleLaser && progressComp.winPuzzleSimon)
            OpenCoffre();
    }

    void OpenCoffre()
    {
		SceneManager.LoadScene("final_scene");
        //GameObject porteCoffre = GameObject.Find("DoorCoffre");
        //TransformOnClick script = porteCoffre.GetComponent<TransformOnClick>();
        //script.enabled = true;
		endScenario.SetActive(true);
    }
}
