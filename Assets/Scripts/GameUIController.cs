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
    [Header("Game End Properties")]
    public GameObject gameCanvas;
    public TextMeshProUGUI winnerText;

    [Space()]
    [Header("Main Menu Properties")]
    public GameObject menuCanvas;
    public TMP_InputField adressField;

    [Space()]
    public GameObject waitCanvas;
    public Board board;

    private readonly string localhost = "127.0.0.1";
    
    

    public void StartServerButton()
    {
        waitCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        NetworkManager.Singleton.StartHost();
    }

    public void StartClientButton()
    {
        NetworkManager manager = NetworkManager.Singleton;
        if (adressField.text == "")
        {
            manager.GetComponent<UNetTransport>().ConnectAddress = localhost;
        }
        else
        {
            manager.GetComponent<UNetTransport>().ConnectAddress = adressField.text;
        }
        manager.StartClient();
    }


    public void GameEnd(int value)
    {
        gameCanvas.SetActive(true);        
       
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


    public void RetryButton()
    {
        this.GetComponent<AudioSource>().Play();
        gameCanvas.SetActive(false);
        board.Setup();
    }

    public void MenuButton()
    {
        this.GetComponent<AudioSource>().Play();
        menuCanvas.SetActive(true);
        gameCanvas.SetActive(false);
    }

}
