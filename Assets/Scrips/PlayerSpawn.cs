using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerSpawn : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Player spawned");
        GameObject plane = GameObject.FindGameObjectWithTag("Plane");
        this.transform.SetParent(plane.transform, false);
        float x = 0;
        if(isServer)
        {
            Debug.Log("Host Spawned");
            x = 28f;
        }else 
        {
            Debug.Log("Client  Spawned");
            x = -28f;
        }

        if(this.transform.position.x == 0)
        {
            this.transform.Translate(x, 0, 0);
        }
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
