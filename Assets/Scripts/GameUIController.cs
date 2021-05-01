using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIController : MonoBehaviour
{   
    public TextMeshProUGUI winnerText;
    public Board board;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Canvas>().enabled = false;
    }

    public void GameEnd(int value)
    {
        this.GetComponent<Canvas>().enabled = true;
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
        this.GetComponent<Canvas>().enabled = false;
        board.Setup();        
    }

    public void MenuButton()
    {
        this.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MenuScene");
    }

}
