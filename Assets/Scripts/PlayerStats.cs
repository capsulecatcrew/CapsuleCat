using System.Collections;
using System.Collections.Generic;

public static class PlayerStats
{
    public static PlayerStat Hp = new PlayerStat(10, 10, 50, 50);

    public static PlayerStat FiringRate = new PlayerStat(0.3f, -0.025f, 50, 50);

    public static int Money = 0;

    public static int LevelsCompleted = 0;

    public static void ResetStats()
    {
        Hp = new PlayerStat(10, 10, 50, 50);
        FiringRate = new PlayerStat(0.3f, -0.025f, 50, 50);
        Money = 0;
        LevelsCompleted = 0;
    }
}
