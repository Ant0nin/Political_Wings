using UnityEngine;
using System.Collections;

public class WinBallPuzzleOnCollision : MonoBehaviour {

    public string targetTag = "Ball";

	void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Equals(targetTag))
        {
            PlayerProgress progressComp = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();
            progressComp.WinPuzzleBoule();
        }
    }
}
