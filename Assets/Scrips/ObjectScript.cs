using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;
public class ObjectScript : NetworkBehaviour
{
    public GameObject arObject;
   // public Text counter;

    //ObjectScript objectScript = new ObjectScript();
    //[SyncVar (hook = "RpcChangeCounter")] 
    //[SyncVar(hook = "RpcChangeCounter")] public int number = 0;

    public override void OnStartServer()  {
        Debug.Log("change");
        arObject.GetComponent<Renderer>().material.color = Color.black;

    }
    public void changeGameObject() {
        arObject.GetComponent<Renderer>().material.color = Color.red;
    }

    /*
    public void increaseCounter(int amount)
    {
        Debug.Log(isLocalPlayer);
        Debug.Log(isServer);

        if (isServer)
        {
            Int32.TryParse(counter.text, out number);
            Debug.Log(number);
            number++;
        }

        if (isLocalPlayer)
        {

            //int number;
            Int32.TryParse(counter.text, out number);
            Debug.Log(number);
            number++;
            //counter.text = number.ToString();
            CmdSetNumber(number);
            //counter.text = number.ToString();

        }
    }
    [Command]
    void CmdSetNumber(int number)
    {
        RpcChangeCounter(number);
    }

    [ClientRpc]
    void RpcChangeCounter(int number)
    {
        changeGameObject();
        //Debug.Log("change");
        //arObject.GetComponent<Renderer>().material.color = Color.red;

        counter.text = number.ToString();

    }
    */
}
