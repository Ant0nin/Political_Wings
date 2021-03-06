﻿using UnityEngine;
using System.Collections;

public class TeleportOnCollision : MonoBehaviour
{
    public Transform teleportLocation;
    public bool resetVelocityOnTeleport = true;
    public string tagTarget = "Ball";

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(tagTarget))
        {
            other.transform.position = teleportLocation.position;
            if (resetVelocityOnTeleport)
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}