using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AISettings
{
    suicide, random, semi, impossible
};
public static class GlobalInfo
{
    
    public static int current_player;
    public static bool[] isAi = new bool[2];
    public static AISettings aiSettings;
}
