using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPlay
{
    public AiPlay(int r, int c)
    {
        row = r;
        collum = c;
    }

    public int row;
    public int collum;
    public int score;
}

public static class MinMax
{
    private static int currentAI;
    
    public static AiPlay StartMinMax(Board board)
    {
        AiPlay bestMove = new AiPlay(-1,-1);
        List<AiPlay> playcount = new List<AiPlay>();
        currentAI = GlobalInfo.current_player;
        int treeDepth = 0;
        for (int i = 0; i < board.rows.Length; i++)
        {
            for (int k = 0; k < board.rows[i].element.Length; k++)
            {
                if (board.rows[i].element[k] == SectionStatus.empty)
                {
                    if (GlobalInfo.aiSettings == AISettings.random || GlobalInfo.aiSettings == AISettings.semi)
                    {
                        playcount.Add(new AiPlay(i, k));
                    }
                    treeDepth++;
                }
            }
        }

        bool dorand = false;
        if(GlobalInfo.aiSettings == AISettings.random)
        {
            dorand = true;
        }
        else if(GlobalInfo.aiSettings == AISettings.semi)
        {            
            if(Random.Range(0, 2) == 1)
            {
                dorand = true;
            }
            else
            {
                dorand = false;
            }
        }


        if(dorand)
        {
            bestMove = RandomMove(playcount);
        }
        else
        {
            RecursiveMax(board, treeDepth, currentAI, out AiPlay minmaxedmove);
            bestMove = minmaxedmove;
        }        
       
        return bestMove;
    }

   
    public static void RecursiveMax(Board board, int layers, int currentPlayer, out AiPlay this_move)
    {        
        this_move = new AiPlay(-1, -1);
        currentPlayer %= 2;
        if (currentPlayer == currentAI)
        {
            this_move.score = -999;
        }
        else
        {
            this_move.score = 999;
        }

        if(GlobalInfo.aiSettings == AISettings.suicide)
        {
            this_move.score *= -1;
        }

        int result = board.CheckBoardState();
        if (result != 0)
        {
            if (result == 3)
            {
                this_move.score = 0;
            }
            else
            {
                if (result - 1 == currentAI)
                {
                    this_move.score = 10 * layers;//faster victories are better                    
                }
                else
                {
                    this_move.score = -10;
                }
            }
        }

        for (int i = 0; i < board.rows.Length; i++)
        {
            for (int k = 0; k < board.rows[i].element.Length; k++)
            {
                if(board.rows[i].element[k] == SectionStatus.empty)
                {
                    board.rows[i].element[k] = (SectionStatus)(currentPlayer + 1);

                    RecursiveMax(board, layers - 1, currentPlayer + 1, out AiPlay nextMove);

                    if (GlobalInfo.aiSettings == AISettings.suicide) //suicidal bot just because I can
                    {
                        if ((currentAI == currentPlayer && nextMove.score < this_move.score)
                            || (currentAI != currentPlayer && nextMove.score > this_move.score))
                        {
                            this_move.score = nextMove.score;
                            this_move.collum = k;
                            this_move.row = i;
                        }
                    }
                    else
                    {
                        if ((currentAI == currentPlayer && nextMove.score > this_move.score)
                            || (currentAI != currentPlayer && nextMove.score < this_move.score))
                        {
                            this_move.score = nextMove.score;
                            this_move.collum = k;
                            this_move.row = i;
                        }
                    }

                    if(nextMove.score == this_move.score) // randomize chosen move if equal score
                    {
                        if(Random.Range(0,2) == 1)
                        {
                            this_move.score = nextMove.score;
                            this_move.collum = k;
                            this_move.row = i;
                        }
                    }

                    board.rows[i].element[k] = SectionStatus.empty;
                }
            }
        }
    }

    public static AiPlay RandomMove(List<AiPlay> aiPossiblePlays)
    {
        int randPos = Random.Range(0, aiPossiblePlays.Count);
        return aiPossiblePlays[randPos];
    }


}
