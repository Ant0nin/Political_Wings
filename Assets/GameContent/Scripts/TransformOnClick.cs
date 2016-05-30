using UnityEngine;
using System.Collections;

public class TransformOnClick : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject nextLocation;
    public float speed = 0.3F;

    private bool trigger = false;

    private Vector3 startLocationPos;
    private Quaternion startLocationRotate;
    private Transform endLocation;

	void Start () {
        startLocationPos = transform.position;
        startLocationRotate = transform.rotation;
        endLocation = nextLocation.transform;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0))
            trigger = !trigger;
    }

    void Update()
    {
        if (trigger) {
            transform.position = Vector3.Lerp(transform.position, endLocation.position, speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, endLocation.rotation, speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, startLocationPos, speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, startLocationRotate, speed);
        }
    }
}
