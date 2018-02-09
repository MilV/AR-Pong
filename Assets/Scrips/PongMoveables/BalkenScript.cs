using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System;


public class BalkenScript : MonoBehaviour {

    // [SyncVar(hook = "clientBalkenPosition")] public Vector3 clientBalkenPosition = GameObject.Find("ClientBalken").transform.position;
    // Use this for initialization
    private Rigidbody rb;
    public bool isFlat = true;
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Vector3 tilt = Input.acceleration;
        //transform.Translate(Input.accelerationEvents.x ,0,0);
        //if (isFlat){
        //    tilt = Quaternion.Euler(90, 0, 0) * tilt*100;
        //    rb.AddForce(tilt);
        //}
        
        
    }

    private void OnEnable()
    {
     
    }

}
