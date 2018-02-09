using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameStart : NetworkBehaviour {

    public GameObject ballPrefab;
    public Transform ballSpawn;
    private Text scoreTextField;
    public Button readyButton;
    public Button leftButton;
    public Button rightButton;
    public PlayerController playerController;
    public GameObject joystick;
    public GameObject clientGoal;
   // private boolean bulletSpawned;
    [SyncVar(hook = "RpcChangeText")] public string scoreText = "Get ready";
    
    // Use this for initialization
    private void Start()
    {
        scoreTextField = GameObject.FindGameObjectWithTag("StatusText").GetComponent<Text>();
        Button disconnectButton = GameObject.FindGameObjectWithTag("Disconnect").GetComponent<Button>();
        
        disconnectButton.onClick.RemoveAllListeners();
        disconnectButton.onClick.AddListener(GameStop);

        Debug.Log("checking Controll");
        if (playerController.GetAlternativeControll())
        {
            Debug.Log("Alternative ON");
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
            leftButton.enabled = false;
            rightButton.enabled = false;

        } else
        {
            joystick.SetActive(false);
        }

        if(playerController.GetSingleplayer())
        {
            clientGoal = GameObject.FindGameObjectWithTag("LeftBar");
             if(clientGoal == null)
              {
              Debug.Log("Not found");
             }
             clientGoal.transform.localScale = new Vector3(0.03125F, 0.07F, 1F);
        }
        // readyButton = GameObject.FindGameObjectWithTag("ReadyButton").GetComponent<Button>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!isServer)
        {
            return;
        }

        if(scoreTextField.text == "" && (GameObject.FindGameObjectWithTag("Ball") == null))
        {
            Debug.Log("Both Players are Ready!");
            CmdSpawnBall();
        }
    }
    public void GameStop()
    {
        if(isServer)
        {
            Debug.Log("Host Disconnected");
            NetworkManager.singleton.StopHost();
            RealoadScene();
        } else
        {
            Debug.Log("Client Disconnected");
            NetworkManager.singleton.StopClient();
            RealoadScene();
        }
    }

    public void ReadyButton()
    {
        if (playerController.GetSingleplayer())
        {
            Destroy(readyButton.gameObject);
            scoreText = "";
        } else { 

        if (isServer)
        {
            
            Destroy(readyButton.gameObject);
            Debug.Log("ready Player 1");

         

            if (scoreTextField.text.Equals("Waiting for Player 1"))
            {
                // both players ready -> spawn ball
                scoreText = "";
                //CmdSpawnBall();
            } else
            {
                scoreText = "Waiting for Player 2";
            }
           

            //readyButton.text = "Waiting for Player 2";
        }
        else
        {
            Destroy(readyButton.gameObject);
            Debug.Log("ready Player 2");
            if(scoreTextField.text.Equals("Waiting for Player 2"))
            {
                //both players ready -> spawn ball
                scoreText = "";
                //CmdSpawnBall();
            } else
            {
                scoreText = "Waiting for Player 1";
            }
          
            CmdSetText(scoreText);
            //readyButton.text = "Waiting for Player 1";
        }
        }
    }

    [Command]
    void CmdSpawnBall()
    {
        GameObject ball = (GameObject)Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);
        NetworkServer.Spawn(ball);

    }

    void RealoadScene()
    {
        SceneManager.LoadScene(0);
        
    }

    [Command]
    void CmdSetText(string text)
    {
        RpcChangeText(text);
    }

    [ClientRpc]
    void RpcChangeText(string text)
    {
        Debug.Log("clicked");
        scoreTextField.text = text;
    }
}
