using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using MLAPI;
using MLAPI.Transports.UNET;

public class GameUIController : MonoBehaviour
{   
    public static GameUIController Instance { get; private set; }


    [Header("Game End Properties")]
    public GameObject gameCanvas;
    public TextMeshProUGUI winnerText;
    public GameObject ServerUI;
    public GameObject ClientUI;

    [Space()]
    [Header("Main Menu Properties")]
    public GameObject menuCanvas;
    public TMP_InputField adressField;
    public TMP_InputField roomField;
    public List<Button> buttons;
    private bool connected;
    public Board board;
    [Space()]
    public GameObject waitCanvas;
    public ReferenceKeeping referencer;

    private readonly string localhost = "127.0.0.1";

    private void Start()
    {
        Instance = this;
        NetworkManager.Singleton.OnClientConnectedCallback += GoToGame;
        NetworkManager.Singleton.OnClientDisconnectCallback += BreakConnection;
    }



    public void StartServerButton()
    {
        this.GetComponent<AudioSource>().Play();
        waitCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        NetworkManager.Singleton.StartHost();
    }

    public void StartClientButton()
    {
        this.GetComponent<AudioSource>().Play();
        NetworkManager manager = NetworkManager.Singleton;        
        if (adressField.text == "")
        {
            manager.GetComponent<UNetTransport>().ConnectAddress = localhost;
        }
        else
        {
            manager.GetComponent<UNetTransport>().ConnectAddress = adressField.text;
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
        manager.StartClient();
        connected = false;
        StartCoroutine(BreakConnectionAttempt());        
    }

    public void CreateRoomButton()
    {
        this.GetComponent<AudioSource>().Play();
        if (roomField.text == "")
        {
            Debug.LogWarning("No room name");
            return;
        }
        waitCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        NetworkManager.Singleton.StartHost();
    }

    public void JoinRoomButton()
    {
        this.GetComponent<AudioSource>().Play();
        if (roomField.text == "")
        {
            Debug.LogWarning("No room name");
            return;
        }
        NetworkManager manager = NetworkManager.Singleton;
        if (adressField.text == "")
        {
            manager.GetComponent<UNetTransport>().ConnectAddress = localhost;
        }
        else
        {
            manager.GetComponent<UNetTransport>().ConnectAddress = adressField.text;
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
        manager.StartClient();
        connected = false;
        StartCoroutine(BreakConnectionAttempt());
    }

    public void QuitButton()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }
        Application.Quit();
    }


    public void GameEnd(int value)
    {
        gameCanvas.SetActive(true);
        ServerUI.SetActive(NetworkManager.Singleton.IsServer);
        ClientUI.SetActive(!NetworkManager.Singleton.IsServer);


        switch (value)
        {
            case 1:
                winnerText.text = "TicTacs won!!!";
                winnerText.color = Color.green;
                break;
            case 2:
                winnerText.text = "Toes won!!!";
                winnerText.color = Color.red;
                break;
            case 3:
                winnerText.text = "A draw! yaaaaay....";
                winnerText.color = Color.black;
                break;
            default:
                break;
        }
    }

    public void GoToGame(ulong newplayer)
    {
        waitCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        board.gameObject.SetActive(true);        
        ServerManager.Instance.AddPlayer(newplayer);
        connected = true;
    }

    public void BreakConnection(ulong required)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.StopServer();
        }
        if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }
        board.Setup();
        board.gameObject.SetActive(false);
        menuCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        waitCanvas.SetActive(false);
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }
        ServerManager.Instance.playerIDList.Clear();
        referencer.players.Clear();
    }


    public void RetryCallResponse()
    {
        gameCanvas.SetActive(false);
        board.Setup();
    }

    public void RetryButton()
    {
        this.GetComponent<AudioSource>().Play();
        PlayerController pc = referencer.GetLocalPlayer();
        pc.CallRetry();
    }

    public void MenuButton()
    {
        this.GetComponent<AudioSource>().Play();
        PlayerController pc = referencer.GetLocalPlayer();
        pc.CallMenu();
    }


    IEnumerator BreakConnectionAttempt()
    {
        yield return new WaitForSeconds(5);
        if (!connected)
        {
            NetworkManager.Singleton.StopClient();
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].interactable = true;
            }
        }
    }

}
