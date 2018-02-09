using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager
{
    public static NetworkManager_Custom instance;
    // public static Button hostButton;
    //public static Button clientButton;
    private static Canvas menuUi;
    private static GameObject optionsMenu;
    private bool alternativeContoll = false;
    GameObject clientGoal;
    public PlayerController playerController;
    

    private void Start()
    {
        Debug.Log("Started");
        optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        optionsMenu.SetActive(false);
        DontDestroyOnLoad(optionsMenu);
        //menuUi = GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>();
    }

    public void StartupSinglePlayer()
    {
        playerController.SetSingleplayer(true);
        Debug.Log("Startup Singelpalyer");
        SetPort();
        NetworkManager.singleton.StartHost();
        Debug.Log("Changing size");
   
    }

    public void StartupHost() {
        playerController.SetSingleplayer(false);
        Debug.Log("Startup Host");
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame() {
        playerController.SetSingleplayer(false);
        Debug.Log("Startup Client");
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIPAddress() {
       // GameObject inputField = GameObject.Find("InputFieldIPAddress");
       
       // string ipAddress = inputField.GetComponent<Text>().text;
       // Debug.Log("IP-Address: " + ipAddress);
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
        Debug.Log("IP-Address: " + ipAddress);
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetPort() {
        NetworkManager.singleton.networkPort = 7777;   
    }

    void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            SetupMenueSceneButtons();
        } else
        {
            SetupOtherSceneButtons();
        }
    }

    void SetupMenueSceneButtons()
    {
        //GameObject optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        //optionsMenu.SetActive(true);
        Debug.Log("Menue loaded");
        GameObject menu = GameObject.FindGameObjectWithTag("Menu");
        GameObject options = null;
        Transform[] trs = menu.GetComponentsInChildren<Transform>(true);
        foreach(Transform t in trs)
        {
            if(t.name == "OptionsMenu")
            {
                Debug.Log("Options Menu Found");
                options = t.gameObject;
            }
        }

        options.SetActive(true);

        Button playButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
        playButton.onClick.RemoveAllListeners();
        playButton.GetComponent<Button>().onClick.AddListener(StartupSinglePlayer);

        Button hostButton = GameObject.FindGameObjectWithTag("HostButton").GetComponent<Button>();
        hostButton.GetComponent<Button>().onClick.RemoveAllListeners();
        hostButton.GetComponent<Button>().onClick.AddListener(StartupHost);

        Button clientButton = GameObject.FindGameObjectWithTag("ClientButton").GetComponent<Button>();
        clientButton.GetComponent<Button>().onClick.RemoveAllListeners();
        clientButton.GetComponent<Button>().onClick.AddListener(JoinGame);

        options.SetActive(false);
        //optionsMenu.SetActive(false);

    }

    void SetupOtherSceneButtons()
    {
        Debug.Log("Game loaded");
       // Canvas menuUi = GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>();
        //menuUi.enabled = false;
        //GetComponentInChildren<Canvas>().enabled = false;
        //Button disconnectButton = GameObject.FindGameObjectWithTag("Disconnect").GetComponent<Button>();
        //Debug.Log("Disconnected");
        //disconnectButton.onClick.RemoveAllListeners();
        //disconnectButton.onClick.AddListener(NetworkManager.singleton.StopHost);

    }

    public void SetAlternativeControll(bool newValue)
    {
        Debug.Log(newValue);
        alternativeContoll = newValue;
    }

    public bool GetAlternativeControll()
    {
        return alternativeContoll;
    }
}
