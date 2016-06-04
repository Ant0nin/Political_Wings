using UnityEngine;
using System.Collections;

public class FramerateLimiter : MonoBehaviour {


	void Awake () {
		
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = 30;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
