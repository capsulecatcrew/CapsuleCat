using System.Collections;
using System.Collections.Generic;

public static class PlayerStats
{
    // player's maximum HP
    public static int MaxHp = 10;

    // player's current HP
    public static int CurrentHp = 10;
    
    // number of levels completed by player
    public static int LevelsCompleted = 0;

    public static void ResetStats()
    {
        MaxHp = 10;
        CurrentHp = 10;
        LevelsCompleted = 0;
    }
    
}
