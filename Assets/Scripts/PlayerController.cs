using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerController : NetworkBehaviour
{
    public ReferenceKeeping reference;
    public bool myturn = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        reference = GameObject.FindWithTag("Reference").GetComponent<ReferenceKeeping>();        
        reference.players.Add(this);
        if(IsOwner && IsServer)
        {
            ServerManager.Instance.AddPlayer(NetworkObjectId);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SendRay();
        }
    }

    public void SendRay()
    {
        if (!IsOwner) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Slot")))
        {
            Boardsection clicked = hit.collider.gameObject.GetComponent<Boardsection>();
            AttemptPlayServerRPC(clicked.row, clicked.collum);
        }
    }

    [ServerRpc]
    public void AttemptPlayServerRPC(int row, int collum)
    {
        Board.Instance.UpdateBoard(OwnerClientId,row, collum);
    }


    public void CallClientVisualChange(int row, int collum, int ID)
    {
        UpgradeVisualsClientRPC(row, collum, ID);
    }

    [ClientRpc]
    public void UpgradeVisualsClientRPC(int row, int collum, int ID)
    {
        Board.Instance.PlayVisuals(row, collum, ID);
    }


    public void CallGameEnd(int state)
    {
        GameEndClientRPC(state);
    }

    [ClientRpc]
    public void GameEndClientRPC(int state)
    {
        Board.Instance.GameOverBroadCast(state);
    }

    public void CallMenu()
    {
        CallMenuClientRPC();
    }

    [ClientRpc]
    public void CallMenuClientRPC()
    {
        GameUIController.Instance.BreakConnection(0);
    }

    public void CallRetry()
    {
        CallRetryClientRPC();
    }

    [ClientRpc]
    public void CallRetryClientRPC()
    {
        GameUIController.Instance.RetryCallResponse();
    }
}
