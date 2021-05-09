using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ReferenceKeeping : MonoBehaviour
{

    public List<PlayerController> players = new List<PlayerController>();


    public PlayerController GetLocalPlayer()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].IsOwner)
            {
                return players[i];
            }
        }
        return null;
    }   
}
