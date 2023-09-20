using System;
using System.Collections.Generic;
using System.Linq;
using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;
using Player.Stats.Persistent;
using Player.Stats.Templates;

public static class PlayerStats
{
    private static int _currentStage = 1;
    private const int CompletionMoney = 50;
    private const float SpecialMax = 50;
    private static readonly Random Random = new();

    private static int _money1;

    // p1 - Stat 1
    private static readonly UpgradeableLinearStat Damage1 = new("Attack Damage", 10, 2, 1, 50, 25, false);

    // p1 - Stat 2
    private static readonly UpgradeableLinearStat MaxEnergy1 = new("Max Energy", 10, 25, 10, 50, 25, false);

    // p1 - Stat 3
    private static readonly UpgradeableLinearStat EnergyAbsorb1 = new("Energy Absorb", 10, 1, 0.1f, 50, 25, false);
    private static readonly Stat Energy1 = new("Energy", MaxEnergy1);

    // p1 - Stat 4
    private static readonly UpgradeableLinearStat SpecialAbsorb1 = new("Special Absorb Rate", 10, 0.03f, 0.005f, 50, 75, false);

    // p1 - Stat 5
    private static readonly UpgradeableLinearStat SpecialDamage1 = new("Special Damage Rate", 10, 0.1f, 0.01f, 50, 75, false);

    // p1 - Stat 6
    private static readonly UpgradeableLinearStat
        SpecialDamaged1 = new("Special Damaged Rate", 10, 0.03f, 0.005f, 50, 75, false);

    private static readonly Stat Special1 = new("Special", SpecialMax, false);
    // !!!!!!!!!!!!!!!!!!!DEBUG CODE BELOW. REPLACE ONCE PLAYTEST DONE. !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // private static SpecialMove _specialMove1;
    private static SpecialMove _specialMove1 = new Heal(1);

    private static readonly List<UpgradeableStat> Player1Stats = new()
        { Damage1, MaxEnergy1, EnergyAbsorb1, SpecialAbsorb1, SpecialDamage1, SpecialDamaged1 };

    private static int _money2;

    // p2 - Stat 1
    private static readonly UpgradeableLinearStat Damage2 = new("Attack Damage", 10, 2, 1, 50, 25, false);

    // p2 - Stat 2
    private static readonly UpgradeableLinearStat MaxEnergy2 = new("Max Energy", 10, 25, 10, 50, 25, false);

    // p2 - Stat 3
    private static readonly UpgradeableLinearStat EnergyAbsorb2 = new("Energy Absorb", 10, 1, 0.1f, 50, 25, false);
    private static readonly Stat Energy2 = new("Energy", MaxEnergy2);

    // p2 - Stat 4
    private static readonly UpgradeableLinearStat SpecialAbsorb2 = new("Special Absorb Rate", 10, 0.03f, 0.005f, 50, 75, false);

    // p2 - Stat 5
    private static readonly UpgradeableLinearStat SpecialDamage2 = new("Special Damage Rate", 10, 0.1f, 0.01f, 50, 75, false);

    // p2 - Stat 6
    private static readonly UpgradeableLinearStat
        SpecialDamaged2 = new("Special Damaged Rate", 10, 0.03f, 0.005f, 50, 75, false);

    private static readonly Stat Special2 = new("Special", SpecialMax, false);
    // !!!!!!!!!!!!!!!!!!!DEBUG CODE BELOW. REPLACE ONCE PLAYTEST DONE. !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // private static SpecialMove _specialMove2;
    private static SpecialMove _specialMove2 = new Vampire(2);
    
    private static readonly List<UpgradeableStat> Player2Stats = new()
        { Damage2, MaxEnergy2, EnergyAbsorb2, SpecialAbsorb2, SpecialDamage2, SpecialDamaged2 };

    // both - Stat 5
    public static readonly UpgradeableLinearStat MaxHealth = new("Max Health", 10, 25, 10, 50, 25, true);
    private static readonly Stat Health = new("Health", MaxHealth);

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
        SpecialAbsorb1.Reset();
        SpecialDamage1.Reset();
        SpecialDamaged1.Reset();
        RemoveSpecialMove(1);
        _money2 = 0;
        Damage2.Reset();
        MaxEnergy2.Reset();
        EnergyAbsorb2.Reset();
        SpecialAbsorb2.Reset();
        SpecialDamage2.Reset();
        SpecialDamaged2.Reset();
        MaxHealth.Reset();
        RemoveSpecialMove(2);
        OnEnable();
    }

    public static int GetCurrentStage()
    {
        return _currentStage;
    }

    public static void Win()
    {
        _currentStage++;
        _money1 += GetCurrentStage() * CompletionMoney;
        _money2 += GetCurrentStage() * CompletionMoney;
    }

    public static void Lose()
    {
        Reset();
    }

    public static BattleStat CreateBattleHealthStat(ProgressBar healthBar)
    {
        MaxHealth.LinkProgressBar(healthBar);
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
                MaxEnergy1.LinkProgressBar(energyBar);
                Energy1.Reset();
                var battleEnergyStat1 = Energy1.CreateBattleStat();
                energyBar.SetValue(battleEnergyStat1.Value);
                battleEnergyStat1.OnStatChange += energyBar.SetValue;
                return battleEnergyStat1;
            case 2:
                MaxEnergy2.LinkProgressBar(energyBar);
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
}