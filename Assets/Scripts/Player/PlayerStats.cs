using System.Collections.Generic;

public static class PlayerStats
{
    public class Player
    {
        public int Money = 0;
        public List<PlayerStat> stats = new List<PlayerStat>();
        public PlayerStat AttackPower = new PlayerStat("Attack Power", 1, 1, 100, 75);
        public PlayerStat Energy = new PlayerStat("Energy", 25, 25, 100, 100);
        public PlayerStat EnergyAbsorb = new PlayerStat("Energy Absorb", 0.1f, 0.1f, 100, 100);
        public PlayerStat EnergySharing = new PlayerStat("Energy Sharing", 0.2f, 0.2f, 100, 100, 5);
        public PlayerStat Sprint = new PlayerStat("Sprint", 1, 1, 200, 100, 4);

        public Player()
        {
            stats.Add(AttackPower);
            stats.Add(Energy);
            stats.Add(EnergyAbsorb);
            // stats.Add(EnergySharing);
            // stats.Add(Sprint);
        }
    }

    public static Player Player1 = new Player();
    public static Player Player2 = new Player();
    public static PlayerStat Hp = new PlayerStat("HP", 10, 10, 50, 75);
    public static int LevelsCompleted = 0;

    // DEPRECATED
    // DEPRECATED


    public static Player GetPlayer(int playerNo)
    {
        if (playerNo == 1)
        {
            return Player1;
        }
        else
        {
            return Player2;
        }
    }

    public static void ResetStats()
    {
        Hp.ResetStat();
        LevelsCompleted = 0;
        Player1 = new Player();
        Player2 = new Player();
    }
}
