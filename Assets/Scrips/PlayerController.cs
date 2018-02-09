using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PlayerController : NetworkBehaviour
{
    public static PlayerController instance;
    public GameObject ballPrefab;
    public Transform ballSpawn;
    public float scalingSpeed = 0.03f;
    public float rotationSpeed = 70.0f;
    public float translationSpeed = 5.0f;
    //	public GameObject Model;
    bool repeatScaleUp = false;
    bool repeatScaleDown = false;
    bool repeatPositionUp = false;
    bool repeatPositionDown = false;
    [SyncVar(hook = "BallSpawn")]public bool hostReady = false;
    [SyncVar(hook = "BallSpawn")] public bool clientReady = false;
    [SyncVar(hook = "RpcHostMoved")]public float positionZHost = 0;
    [SyncVar(hook = "RpcClientMoved")]public float positionZClient = 0;
    public bool alternativeControll = false;
    public bool isSinglePlayer = false;

    public void SetSingleplayer(bool newValue)
    {
        Debug.Log(newValue);
        isSinglePlayer = newValue;
    }

    public bool GetSingleplayer()
    {
        Debug.Log("Get Alternative Controll: " + alternativeControll);
        return isSinglePlayer;
    }

    public void SetAlternativeControll(bool newValue)
    {
        Debug.Log(newValue);
        alternativeControll = newValue;
    }

    public bool GetAlternativeControll()
    {
        Debug.Log("Get Alternative Controll: " + alternativeControll);
        return alternativeControll;
    }

    public void Start()
    {

       // alternativeControll = false;
        if(isLocalPlayer)
        {
            GetComponentInChildren<Canvas>().enabled = true;
        }
    }
    /*
    private void Start()
    {
        GameObject rightButton = GameObject.FindGameObjectWithTag("RightButton");
        GameObject leftButton = GameObject.FindGameObjectWithTag("LeftButton");
        GameObject movePlayerObject = GameObject.FindGameObjectWithTag("MovePlayer");

        if(!isLocalPlayer)
        {
            return;
        }

        rightButton.GetComponent<Button>().onClick.AddListener(() => { movePlayerObject.GetComponent<PlayerController>().PositionUpButton(); });
        leftButton.GetComponent<Button>().onClick.AddListener(() => { movePlayerObject.GetComponent<PlayerController>().PositionDownButton(); });
    }
    */
    void Update()
    {
      //  if(!isLocalPlayer)
        //{
           // return;
        //}
        //Debug.Log("local player");

        if (repeatPositionUp)
        {
            Debug.Log("Repeat Position up");
            CmdPositionUpButton();
        }

        if (repeatPositionDown)
        {
            Debug.Log("Repeat Position down");
            CmdPositionDownButton();
        }

    }

    public void CloseAppButton()
    {
        Application.Quit();
    }

    public void PositionDownButtonRepeat()
    {
        Debug.Log("Down: ON");
        repeatPositionDown = true;
    }

    public void PositionUpButtonRepeat()
    {
        Debug.Log("Up: ON");
        repeatPositionUp = true;
    }

    public void PositionUpButtonOff()
    {
        repeatPositionUp = false;
        Debug.Log("Off");
    }

    public void PositionDownButtonOff()
    {
        repeatPositionDown = false;
        Debug.Log("Off");
    }
    

    //[Command]
    public void CmdPositionUpButton()
    {
        Debug.Log("Position up");
        if (isServer)
        {
            Debug.Log("Host up");
            if (GameObject.FindWithTag("RightBar").transform.position.z > -17.58)
            {
                positionZHost = -translationSpeed * Time.deltaTime;
                //GameObject.FindWithTag("RightBar").transform.Translate(0, 0, positionZHost);
                //Debug.Log(GameObject.FindWithTag("RightBar").transform.position.z);
            }
            else
            {
                Debug.Log("Out of Bounce");
            }
        }
        else
        {
            Debug.Log("Client up");
            if (GameObject.FindWithTag("LeftBar").transform.position.z > -17.58)
            {
                positionZClient = -translationSpeed * Time.deltaTime;
                CmdSetPosition(positionZClient);
                // Debug.Log(GameObject.FindWithTag("LeftBar").transform.position.z);
                //GameObject.FindWithTag("LeftBar").transform.Translate(0, 0, positionZClient);
            } else
            {
                Debug.Log("Out of Bounce");
            }
        }
    }

   // [Command]
    public void CmdPositionDownButton()
    {
        Debug.Log("Position down");
        if (isServer)
        {
            Debug.Log("Host down");
            if (GameObject.FindWithTag("RightBar").transform.position.z < 17.58)
            {
                // Debug.Log(GameObject.FindWithTag("RightBar").transform.position.z);
                positionZHost = translationSpeed * Time.deltaTime;
               // GameObject.FindWithTag("RightBar").transform.Translate(0, 0, positionZHost);
            }
            else
            {
                Debug.Log("Out of Bounce");
            }

        }
        else
        {
            Debug.Log("Client down");
            if (GameObject.FindWithTag("LeftBar").transform.position.z < 17.58)
            {
                positionZClient = translationSpeed * Time.deltaTime;
                // Debug.Log(GameObject.FindWithTag("LeftBar").transform.position.z);
                // GameObject.FindWithTag("LeftBar").transform.Translate(0, 0, positionZClient);
                CmdSetPosition(positionZClient);
            }
            else
            {
                Debug.Log("Out of Bounce");
            }
        }
    }

    [Command]
    void CmdSetPosition(float newZ)
    {
        RpcClientMoved(newZ);
    }

    [ClientRpc]
    public void RpcHostMoved(float newZPosition)
    {
        GameObject.FindWithTag("RightBar").transform.Translate(0, 0, newZPosition);
    }

    [ClientRpc]
    public void RpcClientMoved(float newZPosition)
    {
        GameObject.FindWithTag("LeftBar").transform.Translate(0, 0, newZPosition);
    }

    public void ChangeScene(string a)
    {
        Application.LoadLevel(a);
    }

    public void ReadyButtonClicked()
    {
       // if(!isLocalPlayer)
       // {
          //  return;
       // }
       if(isServer)
        {
            hostReady = true;
        }else if (isClient)
        {
            clientReady = true;
        }
    }

    public void BallSpawn(bool ready)
    {
        Debug.Log("ready");
   

        if (hostReady && clientReady)
        {
            Debug.Log("both ready");
            CmdBallSpawn();
        }
    }
    [Command]
    public void CmdBallSpawn()
    {
        Debug.Log("Any");
        GameObject ball = (GameObject)Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);

        NetworkServer.Spawn(ball);
        
        //GameObject plane = GameObject.FindGameObjectWithTag("Plane");
        //transform.SetParent(plane.transform, false);
        //transform.position = ballSpawn.position;

    }

}


