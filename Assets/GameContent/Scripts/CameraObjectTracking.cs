using UnityEngine;
using System.Collections;

public class CameraObjectTracking : MonoBehaviour {

    public GameObject target;
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPos = target.transform.position;
        transform.localPosition.Set(targetPos.x, targetPos.y, 10);
    }
}
