using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VariableScript
{
    private static int level;
    private static int difficulty;
    
    public static int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }
    public static int Difficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }
}
