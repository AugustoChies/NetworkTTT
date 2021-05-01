using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Row[] rows = new Row[3];
    public GameObject[] tiles;
    public Material[] materials;
    public GameUIController UIController;

    public bool gameOver;
    public bool timeout;

    public GameObject spawner;
    public DeleteList deleteList;
    private void Start()
    {        
        timeout = false;
        Setup();        
    }

    public void Setup()
    {
        deleteList.DestroyList();
        deleteList.Setup();
        gameOver = false;
        GlobalInfo.current_player = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                rows[i].element[k] = SectionStatus.empty;
                tiles[k + i * 3].GetComponent<Renderer>().material = materials[0];
            }
        }
    }

    void Update()
    {
        if (!timeout)
        {
            if (GlobalInfo.isAi[GlobalInfo.current_player] && !gameOver)
            {                
                RunMinMax();
            }
            else if (Input.GetMouseButtonDown(0) && !gameOver)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit,Mathf.Infinity, 1 << LayerMask.NameToLayer("Sections")))
                {
                    if (hit.transform.gameObject.GetComponent<Boardsection>())
                    {
                        hit.transform.gameObject.GetComponent<Boardsection>().GotClicked();
                    }
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R)) //Restart
        {
            Setup();
        }
    }

    public void UpdateBoard(int row,int collum)
    {
        if (rows[row].element[collum] == SectionStatus.empty)
        {
            rows[row].element[collum] = (SectionStatus)(GlobalInfo.current_player + 1);
            tiles[collum + row * 3].GetComponent<Renderer>().material = materials[GlobalInfo.current_player + 1];
            int result = CheckBoardState();
            ActivateSpawner(tiles[collum + row * 3].transform.position + new Vector3(0, 12, 0));
            EndGameCheck(result);            
            GlobalInfo.current_player += 1;
            GlobalInfo.current_player %= 2;
            if(GlobalInfo.isAi[GlobalInfo.current_player])
            {
                StartCoroutine(TimeoutTime());
            }
        }
    }

    public void ActivateSpawner(Vector3 spawnPos)
    {
        GameObject spawnerO = Instantiate(spawner, spawnPos, Quaternion.identity);
        spawnerO.GetComponent<SpawnerScript>().player = GlobalInfo.current_player;
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

    public void RunMinMax()
    {
        AiPlay play = MinMax.StartMinMax(this);
        UpdateBoard(play.row, play.collum);
    }

    WaitForSeconds waittime = new WaitForSeconds(1f);
    IEnumerator TimeoutTime()
    {
        timeout = true;
        yield return waittime;
        timeout = false;
    }
}
