using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Scoreboard_Controller : NetworkBehaviour {

    public static Scoreboard_Controller instance;

    public Text hostScore;
    public Text clientScore;
    public int singleplayerScore;
    public PlayerController playerController;

    [SyncVar(hook = "OnChangeScoreHost")] public int hostScoreValue;
    [SyncVar(hook = "OnChangeScoreClient")] public int clientScoreValue;

    // Use this for initialization
    void Start () {
        instance = this;
        hostScoreValue = clientScoreValue = singleplayerScore = 0;
        if(playerController.isSinglePlayer)
        {
            clientScore.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		

	}

    public void HostScored()
    {
      
        Debug.Log("Host Scored");
        if(!isServer) {
            return;
        }
        hostScoreValue++;
        hostScore.text = hostScoreValue.ToString();
    }

    public void ClientScored()
    {

        Debug.Log("Client Scored");
        if (!isServer) {
            return;
        }

        if(playerController.isSinglePlayer)
        {
            singleplayerScore = 0;
            OnChangeScoreHost(singleplayerScore);
        }

        clientScoreValue++;
        clientScore.text = clientScoreValue.ToString();

    }

    public void SingleplayerScored()
    {
        if(playerController.isSinglePlayer) { 
        Debug.Log("SingleplayerScore: " + singleplayerScore);
        singleplayerScore++;
        OnChangeScoreHost(singleplayerScore);
        }
    }

    void OnChangeScoreHost(int newHostScore)
    {
        if(playerController.isSinglePlayer)
        {
            Debug.Log("Score Changed");
            hostScore.text = singleplayerScore.ToString();
        } else
        {
            hostScore.text = newHostScore.ToString();
        }
        
    }

    void OnChangeScoreClient(int newClientScore)
    {
        clientScore.text = newClientScore.ToString();
    }
}
