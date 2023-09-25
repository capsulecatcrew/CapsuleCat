using System.Collections.Generic;
using System.Linq;
using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;
using Player.Stats.Persistent;
using Player.Stats.Templates;
using Random = System.Random;

public static class PlayerStats
{
    private static int _currentStage = 1;
    private const int CompletionMoney = 50;
    private const int CompletionMoneyIncr = 25;
    private const float SpecialMax = 50;
    private static readonly Random Random = new();

    private static int _money1;

    // p1 - Control Mode
    private static ControlMode p1ControlMode = ControlMode.Movement;
    // p1 - Stat 1
    private static readonly UpgradeableLinearStat Damage1 = new("Attack Damage", 10, 2, 0.5f, 50, 25);
    // p1 - Stat 2
    private static readonly UpgradeableLinearStat MaxEnergy1 = new("Max Energy", 10, 25, 10, 50, 25);
    // p1 - Stat 3
    private static readonly UpgradeableLinearStat EnergyAbsorb1 = new("Energy Absorb", 10, 1f, 0.1f, 50, 25);
    private static readonly Stat Energy1 = new("Energy", MaxEnergy1);
    // p1 - Stat 4
    private static readonly UpgradeableLinearStat SpecialAbsorb1 = new("Special Absorb Rate", 10, 1f, 0.1f, 50, 75);
    // p1 - Stat 5
    private static readonly UpgradeableLinearStat SpecialDamage1 = new("Special Damage Rate", 10, 0.03f, 0.005f, 50, 75);
    // p1 - Stat 6
    private static readonly UpgradeableLinearStat SpecialDamaged1 = new("Special Damaged Rate", 10, 1f, 0.1f, 50, 75);
    // p1 - Stat 7
    private static readonly UpgradeableLinearStat DashEnergyCost1 = new("Dash Energy Cost", 9, 10, -2, 100, 75);
    
    // p1 - Stat 8
    private static readonly UpgradeableLinearStat EnergyShare1 = new("Energy Share", 10, 0.1f, 0.1f, 100, 100);

    private static readonly Stat Special1 = new("Special", SpecialMax, false);
    private static SpecialMove _specialMove1;

    private static readonly List<UpgradeableStat> Player1Stats = new()
        { Damage1, MaxEnergy1, EnergyAbsorb1, SpecialAbsorb1, SpecialDamage1, SpecialDamaged1, DashEnergyCost1, EnergyShare1 };

    private static int _money2;

    // p2 - Control Mode
    private static ControlMode p2ControlMode = ControlMode.Shooting;
    // p2 - Stat 1
    private static readonly UpgradeableLinearStat Damage2 = new("Attack Damage", 10, 2, 0.5f, 50, 25);
    // p2 - Stat 2
    private static readonly UpgradeableLinearStat MaxEnergy2 = new("Max Energy", 10, 25, 10, 50, 25);
    // p2 - Stat 3
    private static readonly UpgradeableLinearStat EnergyAbsorb2 = new("Energy Absorb", 10, 1f, 0.1f, 50, 25);
    private static readonly Stat Energy2 = new("Energy", MaxEnergy2);
    // p2 - Stat 4
    private static readonly UpgradeableLinearStat SpecialAbsorb2 = new("Special Absorb Rate", 10, 1f, 0.1f, 50, 75);
    // p2 - Stat 5
    private static readonly UpgradeableLinearStat SpecialDamage2 = new("Special Damage Rate", 10, 0.03f, 0.005f, 50, 75);
    // p2 - Stat 6
    private static readonly UpgradeableLinearStat SpecialDamaged2 = new("Special Damaged Rate", 10, 1f, 0.1f, 50, 75);
    // p2 - Stat 7
    private static readonly UpgradeableLinearStat DashEnergyCost2 = new("Dash Energy Cost", 9, 10, -2, 100, 75);
    
    // p2 - Stat 8
    private static readonly UpgradeableLinearStat EnergyShare2 = new("Energy Share", 10, 0.1f, 0.1f, 100, 100);

    private static readonly Stat Special2 = new("Special", SpecialMax, false);
    private static SpecialMove _specialMove2;

    private static readonly List<UpgradeableStat> Player2Stats = new()
        { Damage2, MaxEnergy2, EnergyAbsorb2, SpecialAbsorb2, SpecialDamage2, SpecialDamaged2, DashEnergyCost2, EnergyShare2 };

    // both - Stat 8
    public static readonly UpgradeableLinearStat MaxHealth = new("Max Health", 10, 25, 10, 50, 25, true);
    private static readonly Stat Health = new("Health", MaxHealth);

    private static int _prevStage;

    static PlayerStats()
    {
        OnEnable();
    }

    private static void OnEnable()
    {
        Special1.SetValue(0);
        Special2.SetValue(0);
    }

    private static void Reset()
    {
        _currentStage = 1;
        _money1 = 0;
        Damage1.Reset();
        MaxEnergy1.Reset();
        EnergyAbsorb1.Reset();
        Energy1.Reset();
        SpecialAbsorb1.Reset();
        SpecialDamage1.Reset();
        SpecialDamaged1.Reset();
        DashEnergyCost1.Reset();
        RemoveSpecialMove(1);
        
        _money2 = 0;
        Damage2.Reset();
        MaxEnergy2.Reset();
        EnergyAbsorb2.Reset();
        Energy2.Reset();
        SpecialAbsorb2.Reset();
        SpecialDamage2.Reset();
        SpecialDamaged2.Reset();
        DashEnergyCost2.Reset();
        RemoveSpecialMove(2);
        
        MaxHealth.Reset();
        OnEnable();
    }

    public static int GetCurrentStage()
    {
        return _currentStage;
    }

    public static int GetPrevStage()
    {
        return _prevStage;
    }

    public static void Win()
    {
        _prevStage = _currentStage;
        _currentStage++;
        _money1 += CompletionMoney + (GetCurrentStage() - 1) * CompletionMoneyIncr;
        _money2 += CompletionMoney + (GetCurrentStage() - 1) * CompletionMoneyIncr;
    }

    public static void Lose()
    {
        _prevStage = _currentStage;
        Reset();
    }

    public static BattleStat CreateBattleHealthStat(ProgressBar healthBar)
    {
        MaxHealth.InitProgressBar(healthBar);
        var battleHealthStat = Health.CreateBattleStat();
        healthBar.SetValue(battleHealthStat.Value);
        battleHealthStat.OnStatChange += healthBar.SetValue;
        return battleHealthStat;
    }

    public static BattleStat CreateBattleEnergyStat(int playerNum, ProgressBar energyBar)
    {
        switch (playerNum)
        {
            case 1:
                MaxEnergy1.InitProgressBar(energyBar);
                Energy1.Reset();
                var battleEnergyStat1 = Energy1.CreateBattleStat();
                energyBar.SetValue(battleEnergyStat1.Value);
                battleEnergyStat1.OnStatChange += energyBar.SetValue;
                return battleEnergyStat1;
            case 2:
                MaxEnergy2.InitProgressBar(energyBar);
                Energy2.Reset();
                var battleEnergyStat2 = Energy2.CreateBattleStat();
                energyBar.SetValue(battleEnergyStat2.Value);
                battleEnergyStat2.OnStatChange += energyBar.SetValue;
                return battleEnergyStat2;
        }

        return null;
    }

    public static BattleStat CreateBattleSpecialStat(int playerNum, ProgressBar specialBar)
    {
        specialBar.SetMaxValue(0, SpecialMax, 0);
        switch (playerNum)
        {
            case 1:
                var battleSpecialStat1 = Special1.CreateBattleStat();
                specialBar.SetValue(battleSpecialStat1.Value);
                battleSpecialStat1.OnStatChange += specialBar.SetValue;
                return battleSpecialStat1;
            case 2:
                var battleSpecialStat2 = Special2.CreateBattleStat();
                specialBar.SetValue(battleSpecialStat2.Value);
                battleSpecialStat2.OnStatChange += specialBar.SetValue;
                return battleSpecialStat2;
        }

        return null;
    }

    public static float ApplyEnergyAbsorbMultiplier(int playerNum, float amount)
    {
        return playerNum switch
        {
            1 => EnergyAbsorb1.ApplyValue(amount),
            2 => EnergyAbsorb2.ApplyValue(amount),
            _ => amount
        };
    }

    public static float ApplySpecialAbsorbMultipler(int playerNum, float amount)
    {
        return playerNum switch
        {
            1 => SpecialAbsorb1.ApplyValue(amount),
            2 => SpecialAbsorb2.ApplyValue(amount),
            _ => amount
        };
    }

    public static float ApplySpecialDamageMultipler(int playerNum, float amount)
    {
        return playerNum switch
        {
            1 => SpecialDamage1.ApplyValue(amount),
            2 => SpecialDamage2.ApplyValue(amount),
            _ => amount
        };
    }

    public static float ApplySpecialDamagedMultipler(int playerNum, float amount)
    {
        return playerNum switch
        {
            1 => SpecialDamaged1.ApplyValue(amount),
            2 => SpecialDamaged2.ApplyValue(amount),
            _ => amount
        };
    }

    public static bool RemoveMoney(int playerNum, int amount)
    {
        switch (playerNum)
        {
            case 1:
                if (_money1 < amount) return false;
                _money1 -= amount;
                return true;
            case 2:
                if (_money2 < amount) return false;
                _money2 -= amount;
                return true;
        }

        return false;
    }

    public static string GetMoneyString(int playerNum)
    {
        return playerNum switch
        {
            1 => "$" + _money1,
            2 => "$" + _money2,
            _ => "$0"
        };
    }

    public static float GetDamage(int playerNum)
    {
        return playerNum switch
        {
            1 => Damage1.Value,
            2 => Damage2.Value,
            _ => 1.5f
        };
    }

    public static float GetDashEnergyCost(int playerNum)
    {
        return playerNum switch
        {
            1 => DashEnergyCost1.GetValue(),
            2 => DashEnergyCost2.GetValue(),
            _ => 0f
        };

    }

    public static float GetEnergyShare(int playerNum)
    {
        return playerNum switch
        {
            1 => EnergyShare1.GetValue(),
            2 => EnergyShare2.GetValue(),
            _ => 0f
        };

    }

    public static List<UpgradeableStat> GetStatsToUpgrade(int playerNum)
    {
        return playerNum switch
        {
            1 => Player1Stats.OrderBy(_ => Random.Next()).Take(3).ToList(),
            2 => Player2Stats.OrderBy(_ => Random.Next()).Take(3).ToList(),
            _ => new List<UpgradeableStat>()
        };
    }

    /// <summary>
    /// Sets player's current special move.
    /// </summary>
    /// <param name="playerNum">Player to set special move for.</param>
    /// <param name="specialMove">Special move to set for player.</param>
    public static void SetSpecialMove(int playerNum, SpecialMoveEnum specialMove)
    {
        switch (playerNum)
        {
            case 1:
                switch (specialMove)
                {
                    case SpecialMoveEnum.MoveHeal:
                        _specialMove1 = new Heal(playerNum);
                        return;
                    case SpecialMoveEnum.MoveShield:
                        return;
                    case SpecialMoveEnum.ShootVampiric:
                        _specialMove1 = new Vampire(playerNum);
                        return;
                    default:
                        return;
                }
            case 2:
                switch (specialMove)
                {
                    case SpecialMoveEnum.MoveHeal:
                        _specialMove2 = new Heal(playerNum);
                        return;
                    case SpecialMoveEnum.MoveShield:
                        _specialMove2 = new Vampire(playerNum);
                        return;
                    case SpecialMoveEnum.ShootVampiric:
                        return;
                    default:
                        return;
                }
        }
    }

    /// <summary>
    /// Removes specified player's current special move.
    /// </summary>
    public static void RemoveSpecialMove(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                _specialMove1 = null;
                return;
            case 2:
                _specialMove2 = null;
                return;
        }
    }

    public static void UpdateSpecialMoveBattleManagers(BattleManager battleManager)
    {
        _specialMove1?.UpdateBattleManager(battleManager);
        _specialMove2?.UpdateBattleManager(battleManager);
    }

    public static void UseSpecialMove(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                _specialMove1?.Start();
                return;
            case 2:
                _specialMove2?.Start();
                return;
        }
    }

    public static ControlMode GetSpecialControlMode(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                if (_specialMove1 is Heal) return ControlMode.Movement;
                if (_specialMove1 is Vampire) return ControlMode.Shooting;
                break;
            case 2:
                if (_specialMove2 is Heal) return ControlMode.Movement;
                if (_specialMove2 is Vampire) return ControlMode.Shooting;
                break;
        }
        return ControlMode.Movement;
    }

    public static void SavePlayerControlMode(int playerNum, ControlMode controlMode)
    {
        switch (playerNum)
        {
            case 1:
                p1ControlMode = controlMode;
                break;
            case 2:
                p2ControlMode = controlMode;
                break;
        }
    }

    public static ControlMode GetLastPlayerControlMode(int playerNum)
    {
        return (playerNum) switch
        {
            1 => p1ControlMode,
            2 => p2ControlMode,
            _ => ControlMode.Movement
        };
    }

}