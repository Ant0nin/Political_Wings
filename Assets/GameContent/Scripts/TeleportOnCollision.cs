using UnityEngine;
using System.Collections;

public class TeleportOnCollision : MonoBehaviour
{
    public Transform teleportLocation;
    public bool resetVelocityOnTeleport = true;

    void OnTriggerEnter(Collider other)
    {
        other.transform.position = teleportLocation.position;
        if(resetVelocityOnTeleport)
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}