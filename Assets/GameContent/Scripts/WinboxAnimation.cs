using UnityEngine;
using System.Collections;

public class WinboxAnimation : MonoBehaviour {

    public GameObject nextLocation;
    public float speed = 0.3F;
    public Camera mainCamera;

    private Transform endLocation;
    private bool win = false;
    bool collected = false;

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

            /*if (!collected && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    if (objectHit.IsChildOf(transform))
                    {
                        collected = true;*/
                        PlayerProgress progress = GameObject.Find("PlayerProgress").GetComponent<PlayerProgress>();
                        progress.StartCoroutine(progress.BackToMainScene());
                    /*}
                }
            }

            if(collected)
            {*/
                Transform key = transform.GetChild(0);
                key.position = Vector3.Lerp(key.position, mainCamera.transform.position, speed);
                key.Rotate(new Vector3(0, speed, 0)*Time.deltaTime);
            //}
        }
    }
}
