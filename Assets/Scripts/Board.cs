using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows = new Row[3];
    public GameObject[] tiles;
    public Material[] materials;
    public GameUIController UIController;

    public bool gameOver;
    public bool timeout;

    public GameObject spawner;
    public DeleteList deleteList;

    public ReferenceKeeping keeper;
    private Vector3 spawnerPos = new Vector3(0, 12, 0);
    private void Start()
    {
        Instance = this;
        timeout = false;
        Setup();        
    }

    public void Setup()
    {
        deleteList.DestroyList();
        deleteList.Setup();
        gameOver = false;
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                rows[i].element[k] = SectionStatus.empty;
                tiles[k + i * 3].GetComponent<Renderer>().material = materials[0];
            }
        }
    }

    public void UpdateBoard(ulong callerdID, int row,int collum)
    {
        if (callerdID != ServerManager.Instance.GetCurrentPlayer()) return;

        if (rows[row].element[collum] == SectionStatus.empty)
        {
            rows[row].element[collum] = (SectionStatus)(ServerManager.Instance.currentPlayerIndex + 1);            
            int result = CheckBoardState();
            ServerManager.Instance.ChangeCurrentPlayer();
            EndGameCheck(result);

            PlayerController servercontroller = keeper.GetLocalPlayer();
        }
    }

    public void PlayVisuals(int row, int collum)
    {
        tiles[collum + row * 3].GetComponent<Renderer>().material = materials[ServerManager.Instance.currentPlayerIndex + 1];
        ActivateSpawner(tiles[collum + row * 3].transform.position + spawnerPos);
    }

    public void ActivateSpawner(Vector3 spawnPos)
    {
        GameObject spawnerO = Instantiate(spawner, spawnPos, Quaternion.identity);
        spawnerO.GetComponent<SpawnerScript>().player = ServerManager.Instance.currentPlayerIndex;
    }

    // 0 = Ongoing; 1 = Tictacs win; 2 = Toes win; 3 = Tie;
    public int CheckBoardState()
    {
        SectionStatus firstState;
        //Check rows
        for (int i = 0; i < rows.Length; i++)
        {
            firstState = rows[i].element[0];
            if (firstState != SectionStatus.empty)
            {
                int equalCount = 1;
                for (int k = 1; k < 3; k++)
                {
                    if (firstState == rows[i].element[k])
                    {
                        equalCount++;
                    }
                }
                if (equalCount == 3)
                {
                    return (int)firstState;
                }
            }
        }

        //Check colums
        for (int i = 0; i < rows.Length; i++)
        {
            firstState = rows[0].element[i];
            if (firstState != SectionStatus.empty)
            {
                int equalCount = 1;
                for (int k = 1; k < 3; k++)
                {
                    if (firstState == rows[k].element[i])
                    {
                        equalCount++;
                    }
                }
                if (equalCount == 3)
                {
                    return (int)firstState;
                }
            }
        }

        //Diagonal 1      
        firstState = rows[0].element[0];
        if (firstState != SectionStatus.empty)
        {
            int equalCount = 1;
            for (int k = 1; k < 3; k++)
            {
                if (firstState == rows[k].element[k])
                {
                    equalCount++;
                }
            }
            if (equalCount == 3)
            {
                return (int)firstState;
            }
        }

        //Diagonal 2      
        firstState = rows[0].element[2];
        if (firstState != SectionStatus.empty)
        {
            int equalCount = 1;
            for (int k = 1; k < 3; k++)
            {
                if (firstState == rows[k].element[rows.Length - 1 - k])
                {
                    equalCount++;
                }
            }
            if (equalCount == 3)
            {
                return (int)firstState;
            }
        }

        //Check Tie
        for (int i = 0; i < rows.Length; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                if (rows[i].element[k] == SectionStatus.empty)
                {
                    return 0;
                }
            }
        }
        return 3;
    }

    public void EndGameCheck(int state)
    {        
        if (state > 0)
        {
            gameOver = true;
            this.GetComponent<AudioSource>().Play();
            UIController.GameEnd(state);
        }
    }

    
    
}
