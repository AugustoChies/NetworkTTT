using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ServerManager : MonoBehaviour
{
    public static ServerManager Instance { get; private set; }

    public List<ulong> playerIDList = new List<ulong>();
    public int currentPlayerIndex;

    public Board board;
    private void Awake()
    {
        Instance = this;
        currentPlayerIndex = 0;
    }

    public void AddPlayer(ulong newplayer)
    {        
        if(NetworkManager.Singleton.IsServer)
        {
            if(playerIDList.Count > 2)
            {
                return;
            }
            if(playerIDList.Count == 0)
            {
                playerIDList.Add(newplayer);
            }
            else if (playerIDList[0] != newplayer)
            {
                playerIDList.Add(newplayer);
            }
        }
    }

    public ulong GetCurrentPlayer()
    {
        return playerIDList[currentPlayerIndex];
    }


    public void ChangeCurrentPlayer()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            currentPlayerIndex++;
            currentPlayerIndex %= 2;
        }
    }
}
