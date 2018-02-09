using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeToggle : MonoBehaviour {
    public PlayerController playerController;
	// Use this for initialization
	void Start () {
        playerController.SetAlternativeControll(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
