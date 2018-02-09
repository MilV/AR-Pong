using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BallSpawn : NetworkBehaviour
{
    // Use this for initialization
    void Start()
    {
        Debug.Log("spawned");
        GameObject plane = GameObject.FindGameObjectWithTag("Plane");
     
        transform.SetParent(plane.transform, false);
        transform.Translate(0, 0, 0);
    }
}