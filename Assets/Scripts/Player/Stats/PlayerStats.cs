using UnityEngine;

public static class PlayerStats
{
    private static SplitPlayerStats _statsPlayer1 = new();
    private static SplitPlayerStats _statsPlayer2 = new();
    private static Damageable _player;
    private static readonly LinearStat StatMaxHealth = new ("Max Health", 10, 25, 10, 50, 25);
    private static readonly HealthStat StatHealth = new (StatMaxHealth);
    private static Damageable _damageable;
    private static int _currentStage = 1;
    private const int CompletionMoney = 50;

    private static LevelLoader _levelLoader;

    public static void SetLevelLoader(LevelLoader levelLoader)
    {
        if (!_levelLoader) return;
        _levelLoader.OnLevelChange -= SoftReset;
        _levelLoader = levelLoader;
        _levelLoader.OnLevelChange += SoftReset;
    }

    /// <summary>
    /// Hard reset method. Called when player loses the game.
    /// </summary>
    private static void HardReset()
    {
        _statsPlayer1 = new SplitPlayerStats();
        _statsPlayer1.Reset(true);
        _statsPlayer2 = new SplitPlayerStats();
        _statsPlayer2.Reset(true);
        StatMaxHealth.Reset();
        _currentStage = 1;
        StatHealth.Reset();
        Reset(true);
    }

    private static void Reset(bool isLoss)
    {
        _statsPlayer1.Reset(isLoss);
        _statsPlayer2.Reset(isLoss);
    }

    private static void SoftReset(string sceneName)
    {
        Reset(false);
    }

    public static void UpdateDamageable(Damageable damageable)
    {
        _damageable = damageable;
        _damageable.SetHealthStat(StatHealth);
    }

    public static float GetDamage(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                return _statsPlayer1.GetDamage();
            case 2:
                return _statsPlayer2.GetDamage();
        }
        return 0;
    }

    public static float GetMaxHealth()
    {
        return StatMaxHealth.GetValue();
    }

    public static bool CanUseEnergy(int playerNum, float amount)
    {
        return playerNum switch
        {
            1 => _statsPlayer1.CanUseEnergy(amount),
            2 => _statsPlayer2.CanUseEnergy(amount),
            _ => false
        };
    }

    public static void AbsorbEnergy(int playerNum, float amount)
    {
        switch (playerNum)
        {
            case 1:
                _statsPlayer1.AbsorbEnergy(amount);
                break;
            case 2:
                _statsPlayer2.AbsorbEnergy(amount);
                break;
        }
    }

    public static void UpgradeHealth()
    {
        StatMaxHealth.Upgrade();
    }

    public static void UpgradeDamage(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                _statsPlayer1.UpgradeDamage();
                break;
            case 2:
                _statsPlayer2.UpgradeDamage();
                break;
        }
    }

    public static void UpgradeMaxEnergy(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                _statsPlayer1.UpgradeMaxEnergy();
                break;
            case 2:
                _statsPlayer2.UpgradeMaxEnergy();
                break;
        }
    }

    public static void UpgradeEnergyAbsorb(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                _statsPlayer1.UpgradeEnergyAbsorb();
                break;
            case 2:
                _statsPlayer2.UpgradeEnergyAbsorb();
                break;
        }
    }

    public static void UpgradeSpecialStats(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                _statsPlayer1.UpgradeSpecialStats();
                break;
            case 2:
                _statsPlayer2.UpgradeSpecialStats();
                break;
        }
    }

    public static void Damage(float amount, bool ignoreIFrames)
    {
        _damageable.TakeDamage(amount, ignoreIFrames);
    }
    
    public static void Heal(float amount)
    {
        StatHealth.Heal(amount);
    }

    public static HealthStat GetHealthStat()
    {
        return StatHealth;
    }

    public static LinearStat GetMaxHealthStat()
    {
        return StatMaxHealth;
    }

    public static EnergyStat GetEnergyStat(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                return _statsPlayer1.GetEnergyStat();
            case 2:
                return _statsPlayer2.GetEnergyStat();
        }
        return null;
    }
    
    public static LinearStat GetMaxEnergyStat(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                return _statsPlayer1.GetMaxEnergyStat();
            case 2:
                return _statsPlayer2.GetMaxEnergyStat();
        }
        return null;
    }

    public static float GetMaxSpecial(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                return _statsPlayer1.GetMaxSpecial();
            case 2:
                return _statsPlayer2.GetMaxSpecial();
        }
        return 0;
    }

    public static SpecialStat GetSpecialStat(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
            return _statsPlayer1.GetSpecialStat();
            case 2:
            return _statsPlayer2.GetSpecialStat();
        }
        return null;
    }

    public static void AddPlayerMoney(int playerNum, int amount)
    {
        switch (playerNum)
        {
            case 1:
                _statsPlayer1.AddMoney(amount);
                break;
            case 2:
                _statsPlayer2.AddMoney(amount);
                break;
        }
    }

    public static bool RemovePlayerMoney(int playerNum, int amount)
    {
        switch (playerNum)
        {
            case 1:
                return _statsPlayer1.RemoveMoney(amount);
            case 2:
                return _statsPlayer2.RemoveMoney(amount);
        }
        return false;
    }

    public static string GetPlayerMoneyString(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                return _statsPlayer1.GetMoneyString();
            case 2:
                return _statsPlayer2.GetMoneyString();
        }

        return "$0";
    }

    public static void SetBattleManager(BattleManager battleManager)
    {
        battleManager.OnPlayerWin += Win;
        battleManager.OnPlayerLose += HardReset;
    }

    private static void Win()
    {
        _currentStage++;
        _statsPlayer1.AddMoney(GetCurrentStage() * CompletionMoney);
        _statsPlayer2.AddMoney(GetCurrentStage() * CompletionMoney);
    }

    public static int GetCurrentStage()
    {
        return _currentStage;
    }
}