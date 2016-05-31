using UnityEngine;
using System.Collections;

public class WinboxAnimation : MonoBehaviour {

    public GameObject nextLocation;
    public float speed = 0.3F;

    private Transform endLocation;
    private bool win = false;

    void Start()
    {
        endLocation = nextLocation.transform;
    }

	public void Trigger()
    {
        win = true;
    }

    void Update()
    {
        if(win)
        {
            gameObject.SetActive(true);
            Transform keytransform = transform.GetChild(0);
            keytransform.position = Vector3.Lerp(keytransform.position, endLocation.position, speed);
            keytransform.rotation = Quaternion.Lerp(keytransform.rotation, endLocation.rotation, speed);
        }
    }
}
