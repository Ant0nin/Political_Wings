using UnityEngine;
using System.Collections;

public class WinBallPuzzleOnCollision : MonoBehaviour {

	public string tagTarget = "Ball";

	void OnCollisionEnter(Collision other)
    {
		
            PlayerProgress progressComp = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();
            progressComp.WinPuzzleBoule();
    }
}
