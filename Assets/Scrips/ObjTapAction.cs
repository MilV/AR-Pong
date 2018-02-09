using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System;

public class ObjTapAction : NetworkBehaviour
{
    private GameObject body1;
    private GameObject body2;
    private GameObject lines;
    private GameObject spoiler;
    private GameObject doorLines;
    // ObjectScript objectScript ;
    public Text counter;
    [SyncVar(hook = "RpcChangeColor")] public Color objectColor;
    //[SyncVar (hook = "RpcChangeCounter")] 
    [SyncVar(hook = "RpcChangeCounter")] public int number = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ShootRay(ray);
        }
    }


    void ShootRay(Ray ray)
    {
        Debug.Log(isLocalPlayer);
        Debug.Log(isServer);

        RaycastHit rhit;

        bool objectHit = false;

        GameObject gObjectHit = null;

        if (Physics.Raycast(ray, out rhit, 1000.0f))
        {

            Debug.Log("Ray Shot and hit!");

            objectHit = true;

            gObjectHit = rhit.collider.gameObject;
            if (isServer)
            {
                Int32.TryParse(counter.text, out number);
                Debug.Log(number);
                objectColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                number++;
            }

            if (isLocalPlayer)
            {

                //int number;
                Int32.TryParse(counter.text, out number);
                Debug.Log(number);
                number++;
                objectColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                //counter.text = number.ToString();
                CmdSetNumber(number);
                CmdSetColor(objectColor);
                //counter.text = number.ToString();

            }
        }

    }

    private void Start()
    {
        body1 = GameObject.Find("FireGTO/GTO_Body/Empty_Mesh_1");
        body2 = GameObject.Find("FireGTO/GTO_Body/Empty_Mesh_3");
        lines = GameObject.Find("FireGTO/GTO_Body/Empty_Mesh_4");
        //windows
        doorLines = GameObject.Find("FireGTO/GTO_Body/Empty_Mesh_2");
        //spoiler
        spoiler = GameObject.Find("FireGTO/Spoilers/GTO_Spoiler01");
    }

    [Command]
    void CmdSetNumber(int number)
    {
        Debug.Log("client1");
        RpcChangeCounter(number);
    }

    [ClientRpc]
    void RpcChangeCounter(int number)
    {
        // objectScript.changeGameObject();
        Debug.Log("client2");
        //  arObject.GetComponent<Renderer>().material.color = objectColor;
        //Debug.Log("change");
        //arObject.GetComponent<Renderer>().material.color = Color.red;

        counter.text = number.ToString();

    }

    [Command]
    void CmdSetColor(Color color)
    {
        RpcChangeColor(color);
    }

    [ClientRpc]
    void RpcChangeColor(Color color)
    {
        body1.GetComponent<Renderer>().material.color = color;
        body2.GetComponent<Renderer>().material.color = color;
        lines.GetComponent<Renderer>().material.color = color;
        spoiler.GetComponent<Renderer>().material.color = color;
        doorLines.GetComponent<Renderer>().materials[1].color = color;
    }
}
