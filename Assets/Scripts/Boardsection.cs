using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boardsection : MonoBehaviour
{
    public int row, collum;

    public void GotClicked()
    {        
        this.GetComponentInParent<Board>().UpdateBoard(row, collum);
    }
}
