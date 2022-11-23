using System.Collections;
using System.Collections.Generic;

public static class PlayerStats
{
    public static PlayerStat Hp = new PlayerStat(10, 10, 50, 75);

    public static PlayerStat FiringRate = new PlayerStat(0.3f, -0.025f, 100, 75);

    public static PlayerStat Energy = new PlayerStat(25, 25, 100, 100);

    public static PlayerStat EnergyAbsorb = new PlayerStat(0.1f, 0.1f, 100, 100);
    
    public static int Money = 0;

    public static int LevelsCompleted = 0;

    public static void ResetStats()
    {
        Hp.ResetStat();
        FiringRate.ResetStat();
        Energy.ResetStat();
        EnergyAbsorb.ResetStat();
        Money = 0;
        LevelsCompleted = 0;
    }
}
