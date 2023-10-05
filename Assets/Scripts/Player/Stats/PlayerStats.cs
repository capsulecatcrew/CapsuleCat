using System.Collections.Generic;
using System.Linq;
using Battle.Controllers.Player;
using Battle.Hitboxes;
using HUD;
using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;
using Player.Stats.Persistent;
using Player.Stats.Templates;
using Random = System.Random;

namespace Player.Stats
{
    public static class PlayerStats
    {
        private static int _currentStage = 1;
        private const int CompletionMoney = 50;
        private const int CompletionMoneyIncr = 25;
        private const float SpecialMax = 50;
        private static readonly Random Random = new();

        private static int _money1;

        // p1 - Control Mode
        private static ControlMode _p1ControlMode = ControlMode.Movement;

        // p1 - Stat 1
        private static readonly UpgradeableLinearStat Damage1 = new("Attack Damage", 10, 2, 0.5f, 50, 25);

        // p1 - Stat 2
        private static readonly UpgradeableLinearStat EnergyMax1 = new("Max Energy", 10, 30, 10, 50, 25);

        // p1 - Stat 3
        private static readonly UpgradeableLinearStat EnergyAbsorb1 = new("Energy Absorb", 10, 1f, 0.1f, 50, 25);
        private static readonly Stat Energy1 = new("Energy", EnergyMax1);

        private static readonly UpgradeableLinearStat SpecialAbsorb1 = new("Special Absorb Rate", 10, 2, 0.2f, 50, 75);

        private static readonly UpgradeableLinearStat SpecialDamage1 =
            new("Special Damage Rate", 10, 0.15f, 0.015f, 50, 75);

        private static readonly UpgradeableLinearStat SpecialDamaged1 =
            new("Special Damaged Rate", 10, 0.5f, 0.05f, 50, 75);

        // p1 - Stat 4
        private static readonly GroupedUpgradeableStat SpecialGain1 =
            new("Special Gain", 10, 150, 150, false, SpecialAbsorb1, SpecialDamage1, SpecialDamaged1);

        // p1 - Stat 5
        private static readonly UpgradeableLinearStat EnergyCostDash1 = new("Dash Energy Cost", 9, 15, -1f, 100, 75);

        // p1 - Stat 6
        private static readonly UpgradeableLinearStat EnergyShare1 = new("Energy Share", 10, 0.1f, 0.1f, 100, 100);

        private static readonly Stat Special1 = new("Special", SpecialMax, false);
        private static SpecialMove _specialMove1;
        private static List<SpecialMoveEnum> _specialPool1 = SpecialMoveEnumController.CopyAllSpecialMoves();

        private static readonly List<UpgradeableStat> Player1Stats = new()
            { Damage1, EnergyMax1, EnergyAbsorb1, SpecialGain1, EnergyCostDash1, EnergyShare1 };

        private static int _money2;

        // p2 - Control Mode
        private static ControlMode _p2ControlMode = ControlMode.Shooting;

        // p2 - Stat 1
        private static readonly UpgradeableLinearStat Damage2 = new("Attack Damage", 10, 2, 0.5f, 50, 25);

        // p2 - Stat 2
        private static readonly UpgradeableLinearStat EnergyMax2 = new("Max Energy", 10, 30, 10, 50, 25);
        // p2 - Stat 3
        private static readonly UpgradeableLinearStat EnergyAbsorb2 = new("Energy Absorb", 10, 1f, 0.1f, 50, 25);
        private static readonly Stat Energy2 = new("Energy", EnergyMax2);

        private static readonly UpgradeableLinearStat SpecialAbsorb2 = new("Special Absorb Rate", 10, 2, 0.2f, 50, 75);
        private static readonly UpgradeableLinearStat SpecialDamage2 =
            new("Special Damage Rate", 10, 0.15f, 0.015f, 50, 75);
        private static readonly UpgradeableLinearStat SpecialDamaged2 =
            new("Special Damaged Rate", 10, 0.5f, 0.05f, 50, 75);
        // p2 - Stat 4
        private static readonly GroupedUpgradeableStat SpecialGain2 =
            new("Special Gain", 10, 150, 150, false, SpecialAbsorb2, SpecialDamage2, SpecialDamaged2);

        // p2 - Stat 5
        private static readonly UpgradeableLinearStat EnergyCostDash2 = new("Dash Energy Cost", 9, 15, -1f, 100, 75);

        // p2 - Stat 6
        private static readonly UpgradeableLinearStat EnergyShare2 = new("Energy Share", 10, 0.1f, 0.1f, 100, 100);

        private static readonly Stat Special2 = new("Special", SpecialMax, false);
        private static SpecialMove _specialMove2;
        private static List<SpecialMoveEnum> _specialPool2 = SpecialMoveEnumController.CopyAllSpecialMoves();

        private static readonly List<UpgradeableStat> Player2Stats = new()
            { Damage2, EnergyMax2, EnergyAbsorb2, SpecialGain2, EnergyCostDash2, EnergyShare2 };

        // both - Stat 7
        public static readonly UpgradeableLinearStat MaxHealth = new("Max Health", 10, 25, 10, 50, 25, true);
        private static readonly Stat Health = new("Health", MaxHealth);

        private static int _prevStage;

        static PlayerStats()
        {
            Special1.SetValue(0);
            Special2.SetValue(0);
        }
        
        /**========================================== Scene Change Methods ==========================================*/
        /// <summary>
        /// On loss reset method. Do not call, simply subscribe from loss event.
        /// </summary>
        public static void Lose()
        {
            StorePreviousStage();
            _currentStage = 1;
            MaxHealth.Reset();
            Health.Reset();
            
            _money1 = 0;
            Damage1.Reset();
            EnergyMax1.Reset();
            EnergyAbsorb1.Reset();
            Energy1.Reset();
            EnergyCostDash1.Reset();
            SpecialAbsorb1.Reset();
            SpecialDamage1.Reset();
            SpecialDamaged1.Reset();
            Special1.SetValue(0);
            _specialMove1 = null;

            _money2 = 0;
            Damage2.Reset();
            EnergyMax2.Reset();
            EnergyAbsorb2.Reset();
            Energy2.Reset();
            EnergyCostDash2.Reset();
            SpecialAbsorb2.Reset();
            SpecialDamage2.Reset();
            SpecialDamaged2.Reset();
            Special2.SetValue(0);
            _specialMove2 = null;
        }
        
        /// <summary>
        /// On win progression method. Do not call, simply subscribe from win event.
        /// </summary>
        public static void Win()
        {
            StorePreviousStage();
            _currentStage++;
            _money1 += CompletionMoney + (GetCurrentStage() - 1) * CompletionMoneyIncr;
            _money2 += CompletionMoney + (GetCurrentStage() - 1) * CompletionMoneyIncr;
        }

        /// <summary>
        /// Stores previous stage for stage completion/loss scene.
        /// </summary>
        private static void StorePreviousStage()
        {
            _prevStage = _currentStage;
        }
        
        /// <summary>
        /// Enables special moves for both players.
        /// </summary>
        public static void EnableSpecialMoves()
        {
            _specialMove1?.Enable();
            _specialMove2?.Enable();
        }

        /// <summary>
        /// Disables special moves for both players.
        /// </summary>
        public static void DisableSpecialMoves()
        {
            _specialMove1?.Disable();
            _specialMove2?.Disable();
        }

        /// <summary>
        /// Updates PlayerController class instance to specified instance when loading battle scene.
        /// </summary>
        public static void UpdatePlayerController(PlayerController playerController, PlayerSoundController playerSoundController)
        {
            _specialMove1?.UpdateControllers(playerController, playerSoundController);
            _specialMove2?.UpdateControllers(playerController, playerSoundController);
        }
        
        /**=========================================== Stage Info Methods ===========================================*/
        /// <summary>
        /// Gets current stage.
        /// </summary>
        public static int GetCurrentStage()
        {
            return _currentStage;
        }

        /// <summary>
        /// Gets previously loaded stage.
        /// </summary>
        public static int GetPrevStage()
        {
            return _prevStage;
        }
        
        /**======================================= Battle Scene Setup Methods =======================================*/
        /// <summary>
        /// Initialises player Killable instance.
        /// </summary>
        public static void InitPlayerKillable(Killable killable)
        {
            killable.Init(Health);
        }

        /// <summary>
        /// Initialises player health bar max value.
        /// </summary>
        public static void InitPlayerHealthBarMax(ProgressBar healthBar)
        {
            healthBar.SetMaxValue(MaxHealth.Value);
        }

        /// <summary>
        /// Initialises player health bar remaining value.
        /// </summary>
        public static void InitPlayerHealthBarValue(ProgressBar healthBar)
        {
            healthBar.SetValue(Health.Value);
        }

        /// <summary>
        /// Creates battle energy stat for specified player.
        /// </summary>
        public static BattleStat CreateBattleEnergyStat(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    Energy1.Reset();
                    var battleEnergyStat1 = Energy1.CreateBattleStat();
                    return battleEnergyStat1;
                case 2:
                    Energy2.Reset();
                    var battleEnergyStat2 = Energy2.CreateBattleStat();
                    return battleEnergyStat2;
            }

            return null;
        }

        /// <summary>
        /// Initialises player energy bar max value for specified player.
        /// </summary>
        public static void InitPlayerEnergyBarMax(int playerNum, ProgressBar energyBar)
        {
            switch (playerNum)
            {
                case 1:
                    energyBar.SetMaxValue(EnergyMax1.Value);
                    return;
                case 2:
                    energyBar.SetMaxValue(EnergyMax2.Value);
                    return;
            }
        }

        /// <summary>
        /// Creates battle special stat for specified player.
        /// </summary>
        public static BattleStat CreateBattleSpecialStat(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    var battleSpecialStat1 = Special1.CreateBattleStat();
                    return battleSpecialStat1;
                case 2:
                    var battleSpecialStat2 = Special2.CreateBattleStat();
                    return battleSpecialStat2;
            }

            return null;
        }

        /// <summary>
        /// Initialises player special bar max value for specified player.
        /// </summary>
        public static void InitPlayerSpecialBarMax(int playerNum, ProgressBar specialBar)
        {
            switch (playerNum)
            {
                case 1:
                    specialBar.SetMaxValue(SpecialMax);
                    return;
                case 2:
                    specialBar.SetMaxValue(SpecialMax);
                    return;
            }
        }
        
        /// <summary>
        /// Gets basic bullet shot damage for specified player.
        /// </summary>
        public static float GetDamage(int playerNum)
        {
            return playerNum switch
            {
                1 => Damage1.Value,
                2 => Damage2.Value,
                _ => 0f
            };
        }

        /// <summary>
        /// Gets dash energy cost for specified player.
        /// </summary>
        public static float GetDashEnergyCost(int playerNum)
        {
            return playerNum switch
            {
                1 => EnergyCostDash1.GetValue(),
                2 => EnergyCostDash2.GetValue(),
                _ => 0f
            };
        }
        
        /// <summary>
        /// Saves the specified player's control modes.
        /// TODO consider making this a one off in the disable method in the battle scene. Less processing :)
        /// </summary>
        public static void SavePlayerControlMode(int playerNum, ControlMode controlMode)
        {
            switch (playerNum)
            {
                case 1:
                    _p1ControlMode = controlMode;
                    break;
                case 2:
                    _p2ControlMode = controlMode;
                    break;
            }
        }

        /// <summary>
        /// Gets the specified player's last saved control mode.
        /// </summary>
        /// <param name="playerNum"></param>
        /// <returns></returns>
        public static ControlMode GetPlayerControlMode(int playerNum)
        {
            return (playerNum) switch
            {
                1 => _p1ControlMode,
                2 => _p2ControlMode,
                _ => ControlMode.Movement
            };
        }

        /// <summary>
        /// Initialises player special move icon to the currently owned special move.
        /// </summary>
        public static void InitPlayerSpecialIcon(int playerNum, SpecialIcon specialIcon)
        {
            switch (playerNum)
            {
                case 1:
                    specialIcon.SetSprite(_specialMove1);
                    _specialMove1?.SetIcon(specialIcon);
                    return;
                case 2:
                    specialIcon.SetSprite(_specialMove2);
                    _specialMove2?.SetIcon(specialIcon);
                    return;
            }
        }
        
        /**====================================== Battle Scene Special Methods ======================================*/
        /// <summary>
        /// Uses the special move for the specified player.
        /// </summary>
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
        
        /// <summary>
        /// Stops the special move for the specified player.
        /// </summary>
        public static void StopSpecialMove(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    _specialMove1?.Stop();
                    return;
                case 2:
                    _specialMove2?.Stop();
                    return;
            }
        }

        /// <summary>
        /// Gets the control mode of the special attack of the specified player.
        /// </summary>
        public static ControlMode GetSpecialControlMode(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    switch (_specialMove1)
                    {
                        case Heal:
                            return ControlMode.Movement;
                        case AbsorbShield:
                            return ControlMode.Movement;
                        case Vampire:
                            return ControlMode.Shooting;
                    }

                    break;
                case 2:
                    switch (_specialMove2)
                    {
                        case Heal:
                            return ControlMode.Movement;
                        case AbsorbShield:
                            return ControlMode.Movement;
                        case Vampire:
                            return ControlMode.Shooting;
                    }

                    break;
            }

            return ControlMode.Movement;
        }

        /**======================================== Battle Stat Apply Methods ========================================*/
        /// <summary>
        /// Applies energy absorption multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplyEnergyAbsorbMultiplier(int playerNum, float amount)
        {
            return playerNum switch
            {
                1 => EnergyAbsorb1.ApplyValue(amount),
                2 => EnergyAbsorb2.ApplyValue(amount),
                _ => amount
            };
        }

        /// <summary>
        /// Applies energy sharing multiplier to absorbed amount for specified player.
        /// </summary>
        public static float ApplyEnergyShareMultiplier(int playerNum, float amount)
        {
            return playerNum switch
            {
                1 => EnergyShare1.ApplyValue(amount),
                2 => EnergyShare2.ApplyValue(amount),
                _ => 0f
            };
        }

        /// <summary>
        /// Applies special absorption multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplySpecialAbsorbMultipler(int playerNum, float amount)
        {
            return playerNum switch
            {
                1 => SpecialAbsorb1.ApplyValue(amount),
                2 => SpecialAbsorb2.ApplyValue(amount),
                _ => amount
            };
        }

        /// <summary>
        /// Applies special shoot damage multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplySpecialDamageMultipler(int playerNum, float amount)
        {
            return playerNum switch
            {
                1 => SpecialDamage1.ApplyValue(amount),
                2 => SpecialDamage2.ApplyValue(amount),
                _ => amount
            };
        }

        /// <summary>
        /// Applies special player damaged multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplySpecialDamagedMultipler(int playerNum, float amount)
        {
            return playerNum switch
            {
                1 => SpecialDamaged1.ApplyValue(amount),
                2 => SpecialDamaged2.ApplyValue(amount),
                _ => amount
            };
        }
        
        /**=========================================== Shop Scene Methods ===========================================*/
        /// <summary>
        /// Removes specified amount of money from specified player.
        /// </summary>
        /// <returns>Whether true if money is successfully removed, false otherwise.</returns>
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

        /// <summary>
        /// Gets amount of money specified player has as a string.
        /// Appends $ sign to the start of the string.
        /// </summary>
        public static string GetMoneyString(int playerNum)
        {
            return playerNum switch
            {
                1 => "$" + _money1,
                2 => "$" + _money2,
                _ => "$0"
            };
        }

        /// <summary>
        /// Gets list of random stats to sell in the shop for the specified player.
        /// </summary>
        public static List<UpgradeableStat> GetShopStats(int playerNum)
        {
            return playerNum switch
            {
                1 => Player1Stats.OrderBy(_ => Random.Next()).Take(3).ToList(),
                2 => Player2Stats.OrderBy(_ => Random.Next()).Take(3).ToList(),
                _ => new List<UpgradeableStat>()
            };
        }

        /// <summary>
        /// Gets random special move to sell in the shop for the specified player.
        /// </summary>
        public static SpecialMoveEnum GetShopSpecialMove(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    var chosen1 = _specialPool1[Random.Next(_specialPool1.Count)];
                    _specialPool1 = SpecialMoveEnumController.CopyAllSpecialMoves();
                    _specialPool1.Remove(chosen1);
                    return chosen1;
                case 2:
                    var chosen2 = _specialPool2[Random.Next(_specialPool2.Count)];
                    _specialPool2 = SpecialMoveEnumController.CopyAllSpecialMoves();
                    _specialPool2.Remove(chosen2);
                    return chosen2;
            }

            return SpecialMoveEnum.MoveHeal;
        }

        /// <summary>
        /// Sets specified player's current special move.
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
                        case SpecialMoveEnum.MoveAbsorbShield:
                            _specialMove1 = new AbsorbShield(playerNum);
                            return;
                        case SpecialMoveEnum.ShootVampire:
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
                        case SpecialMoveEnum.MoveAbsorbShield:
                            _specialMove2 = new AbsorbShield(playerNum);
                            return;
                        case SpecialMoveEnum.ShootVampire:
                            _specialMove2 = new Vampire(playerNum);
                            return;
                        default:
                            return;
                    }
            }
        }
    }
}