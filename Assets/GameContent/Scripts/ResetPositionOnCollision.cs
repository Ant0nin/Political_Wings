using UnityEngine;
using System.Collections;

public class ResetPositionOnCollision : MonoBehaviour {

    public string tagTarget = "Ball";
    public bool resetVelocityOnTeleport = true;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(tagTarget))
        {
            other.transform.position = other.gameObject.GetComponent<StartPositionKeeper>().startPosition.transform.position;
            if (resetVelocityOnTeleport)
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
