using System;
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

        private class Player
        {
            public int Money { get; set; }

            // Control Mode
            public ControlMode ControlMode { get; set; } = ControlMode.Movement;

            // Stat 1
            public UpgradeableLinearStat Damage { get; }

            // Stat 2
            public UpgradeableLinearStat EnergyMax { get; }

            // Stat 3
            public UpgradeableLinearStat EnergyAbsorb { get; }
            public Stat Energy { get; }

            public UpgradeableLinearStat SpecialAbsorb { get; }
            public UpgradeableLinearStat SpecialDamage { get; }

            public UpgradeableLinearStat SpecialDamaged { get; }

            // Stat 4
            public GroupedUpgradeableStat SpecialGain { get; }

            // Stat 5
            public UpgradeableLinearStat EnergyCostDash { get; }
            
            public Stat Special { get; }
            public SpecialMove SpecialMove { get; set; }
            public List<SpecialMoveEnum> SpecialPool { get; } = SpecialMoveEnumController.CopyAllSpecialMoves();

            public List<UpgradeableStat> PlayerStats { get; }

            public Player()
            {
                Damage = new("Attack Damage", 10, 2, 0.5f, 50, 25, description:"");
                EnergyMax = new("Max Energy", 10, 30, 10, 50, 25, description:"");
                EnergyAbsorb = new("Energy Absorb", 10, 1f, 0.1f, 50, 25, description:"");
                Energy = new("Energy", EnergyMax);
                SpecialAbsorb = new("Special Absorb Rate", 10, 2, 0.2f, 50, 75, description:"");
                SpecialDamage = new("Special Damage Rate", 10, 0.15f, 0.015f, 50, 75, description:"");
                SpecialDamaged = new("Special Damaged Rate", 10, 0.5f, 0.05f, 50, 75, description:"");
                SpecialGain = new("Special Gain", 10, 150, 150, false, "", SpecialAbsorb, SpecialDamage, SpecialDamaged);
                EnergyCostDash = new("Dash Energy Cost", 9, 15, -1f, 100, 75, description:"");
                Special = new("Special", SpecialMax, false);
                PlayerStats = new() { Damage, EnergyMax, EnergyAbsorb, SpecialGain, EnergyCostDash };
                Special.SetValue(0);
            }

            public void ResetPlayer()
            {
                Money = 0;
                Damage.Reset();
                EnergyMax.Reset();
                EnergyAbsorb.Reset();
                Energy.Reset();
                EnergyCostDash.Reset();
                SpecialAbsorb.Reset();
                SpecialDamage.Reset();
                SpecialDamaged.Reset();
                SpecialGain.Reset();
                Special.SetValue(0);
                SpecialMove?.Stop(true);
                SpecialMove = null;
            }
        }

        private static readonly Player Player1 = new ();
        private static readonly Player Player2 = new ();

        private static Player GetPlayer(int playerNum)
        {
            return playerNum switch
            {
                1 => Player1,
                _ => Player2
            };
        }
        
        // both - Stat 6
        public static readonly UpgradeableLinearStat EnergyShare = new("Energy Share", 10, 0.1f, 0.1f, 100, 100, description:"");

        // both - Stat 7
        public static readonly UpgradeableLinearStat MaxHealth = new("Max Health", 10, 25, 10, 50, 25, true, description:"");
        private static readonly Stat Health = new("Health", MaxHealth);

        private static int _prevStage;

        static PlayerStats()
        {
            Player2.ControlMode = ControlMode.Shooting;
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
            EnergyShare.Reset();
            Player1.ResetPlayer();
            Player2.ResetPlayer();
        }
        
        /// <summary>
        /// On win progression method. Do not call, simply subscribe from win event.
        /// </summary>
        public static void Win()
        {
            StorePreviousStage();
            _currentStage++;
            Player1.Money += CompletionMoney + (GetCurrentStage() - 1) * CompletionMoneyIncr;
            Player2.Money += CompletionMoney + (GetCurrentStage() - 1) * CompletionMoneyIncr;
            Player1.SpecialMove?.Stop(true);
            Player2.SpecialMove?.Stop(true);
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
            Player1.SpecialMove?.Enable();
            Player2.SpecialMove?.Enable();
        }

        /// <summary>
        /// Disables special moves for both players.
        /// </summary>
        public static void DisableSpecialMoves()
        {
            Player1.SpecialMove?.Disable();
            Player2.SpecialMove?.Disable();
        }

        /// <summary>
        /// Updates PlayerController class instance to specified instance when loading battle scene.
        /// </summary>
        public static void UpdatePlayerController(
            PlayerController playerController,
            PlayerSoundController playerSoundController,
            PlayerLaserController playerLaserController)
        {
            Player1.SpecialMove?.UpdateControllers(playerController, playerSoundController);
            Player2.SpecialMove?.UpdateControllers(playerController, playerSoundController);
            if (Player1.SpecialMove is Laser castMove1)
            {
                castMove1.InitPlayerLaserController(playerLaserController);
            }
            if (Player2.SpecialMove is Laser castMove2)
            {
                castMove2.InitPlayerLaserController(playerLaserController);
            }
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
            GetPlayer(playerNum).Energy.Reset();
            var battleEnergyStat = GetPlayer(playerNum).Energy.CreateBattleStat();
            return battleEnergyStat;
        }

        /// <summary>
        /// Initialises player energy bar max value for specified player.
        /// </summary>
        public static void InitPlayerEnergyBarMax(int playerNum, ProgressBar energyBar)
        {
            energyBar.SetMaxValue(GetPlayer(playerNum).EnergyMax.Value);
        }

        /// <summary>
        /// Creates battle special stat for specified player.
        /// </summary>
        public static BattleStat CreateBattleSpecialStat(int playerNum)
        {
            var battleEnergyStat = GetPlayer(playerNum).Special.CreateBattleStat();
            return battleEnergyStat;
        }

        /// <summary>
        /// Initialises player special bar max value for specified player.
        /// </summary>
        public static void InitPlayerSpecialBarMax(int playerNum, ProgressBar specialBar)
        {
            specialBar.SetMaxValue(SpecialMax);
        }
        
        /// <summary>
        /// Gets basic bullet shot damage for specified player.
        /// </summary>
        public static float GetDamage(int playerNum)
        {
            return GetPlayer(playerNum).Damage.GetValue();
        }

        /// <summary>
        /// Gets dash energy cost for specified player.
        /// </summary>
        public static float GetDashEnergyCost(int playerNum)
        {
            return GetPlayer(playerNum).EnergyCostDash.GetValue();
        }
        
        /// <summary>
        /// Saves the specified player's control modes.
        /// TODO consider making this a one off in the disable method in the battle scene. Less processing :)
        /// </summary>
        public static void SavePlayerControlMode(int playerNum, ControlMode controlMode)
        {
            GetPlayer(playerNum).ControlMode = controlMode;
        }

        /// <summary>
        /// Gets the specified player's last saved control mode.
        /// </summary>
        /// <param name="playerNum"></param>
        /// <returns></returns>
        public static ControlMode GetPlayerControlMode(int playerNum)
        {
            return GetPlayer(playerNum).ControlMode;
        }

        /// <summary>
        /// Initialises player special move icon to the currently owned special move.
        /// </summary>
        public static void InitPlayerSpecialIcon(int playerNum, SpecialIcon specialIcon)
        {
            var specialMove = GetPlayer(playerNum).SpecialMove;
            specialIcon.SetSprite(specialMove);
            specialMove?.SetIcon(specialIcon);
        }
        
        /**====================================== Battle Scene Special Methods ======================================*/
        // TODO can we move these into its own script & component?
        /// <summary>
        /// Uses the special move for the specified player.
        /// </summary>
        public static void UseSpecialMove(int playerNum)
        {
            GetPlayer(playerNum).SpecialMove?.Start();
        }
        
        /// <summary>
        /// Stops the special move for the specified player.
        /// </summary>
        public static void StopSpecialMove(int playerNum)
        {
            GetPlayer(playerNum).SpecialMove?.Stop();
        }

        /// <summary>
        /// Gets the control mode of the special attack of the specified player.
        /// </summary>
        public static ControlMode GetSpecialControlMode(int playerNum)
        {
            var specialMove = GetPlayer(playerNum).SpecialMove;
            return specialMove switch
            {
                Heal => ControlMode.Movement,
                AbsorbShield => ControlMode.Movement,
                Vampire => ControlMode.Shooting,
                Laser => ControlMode.Shooting,
                _ => ControlMode.Movement
            };
        }

        /**======================================== Battle Stat Apply Methods ========================================*/
        /// <summary>
        /// Applies energy absorption multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplyEnergyAbsorbMultiplier(int playerNum, float amount)
        {
            return GetPlayer(playerNum).EnergyAbsorb.ApplyValue(amount);
        }

        /// <summary>
        /// Applies energy sharing multiplier to absorbed amount for specified player.
        /// </summary>
        public static float ApplyEnergyShareMultiplier(float amount)
        {
            return EnergyShare.ApplyValue(amount);
        }

        /// <summary>
        /// Applies special absorption multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplySpecialAbsorbMultiplier(int playerNum, float amount)
        {
            return GetPlayer(playerNum).SpecialAbsorb.ApplyValue(amount);
        }

        /// <summary>
        /// Applies special shoot damage multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplySpecialDamageMultiplier(int playerNum, float amount)
        {
            return GetPlayer(playerNum).SpecialDamage.ApplyValue(amount);
        }

        /// <summary>
        /// Applies special player damaged multiplier to damage amount for specified player.
        /// </summary>
        public static float ApplySpecialDamagedMultiplier(int playerNum, float amount)
        {
            return GetPlayer(playerNum).SpecialDamaged.ApplyValue(amount);
        }
        
        /**=========================================== Shop Scene Methods ===========================================*/
        /// <summary>
        /// Removes specified amount of money from specified player.
        /// </summary>
        /// <returns>Whether true if money is successfully removed, false otherwise.</returns>
        public static bool RemoveMoney(int playerNum, int amount)
        {
            if (GetPlayer(playerNum).Money < amount) return false;
            GetPlayer(playerNum).Money -= amount;
            return true;
        }

        /// <summary>
        /// Gets amount of money specified player has as a string.
        /// Appends $ sign to the start of the string.
        /// </summary>
        public static string GetMoneyString(int playerNum)
        {
            return "$" + GetPlayer(playerNum).Money;
        }

        /// <summary>
        /// Gets list of random stats to sell in the shop for the specified player.
        /// </summary>
        public static List<UpgradeableStat> GetShopStats(int playerNum)
        {
            return GetPlayer(playerNum).PlayerStats.OrderBy(_ => Random.Next()).Take(3).ToList();
        }

        /// <summary>
        /// Gets random special move to sell in the shop for the specified player.
        /// </summary>
        public static SpecialMoveEnum GetShopSpecialMove(int playerNum)
        {
            var specialPool = GetPlayer(playerNum).SpecialPool;
            var specialMove = specialPool[Random.Next(specialPool.Count)];
            specialPool = SpecialMoveEnumController.CopyAllSpecialMoves();
            specialPool.Remove(specialMove);
            return specialMove;
        }

        /// <summary>
        /// Sets specified player's current special move.
        /// </summary>
        /// <param name="playerNum">Player to set special move for.</param>
        /// <param name="specialMove">Special move to set for player.</param>
        public static void SetSpecialMove(int playerNum, SpecialMoveEnum specialMove)
        {
            var player = GetPlayer(playerNum);
            switch (specialMove)
            {
                case SpecialMoveEnum.MoveHeal:
                    player.SpecialMove = new Heal(playerNum);
                    return;
                case SpecialMoveEnum.MoveAbsorbShield:
                    player.SpecialMove = new AbsorbShield(playerNum);
                    return;
                case SpecialMoveEnum.ShootVampire:
                    player.SpecialMove = new Vampire(playerNum);
                    return;
                case SpecialMoveEnum.ShootLaser:
                    player.SpecialMove = new Laser(playerNum);
                    return;
                default:
                    return;
            }
        }

        public static SpecialMove GetPlayerSpecialMove(int playerNum) => GetPlayer(playerNum).SpecialMove;
        public static UpgradeableStat GetMaxHealthStat() => MaxHealth;
        public static UpgradeableStat GetEnergyShareStat() => EnergyShare;
        public static UpgradeableStat GetAttackDamageStat(int playerNum) => GetPlayer(playerNum).Damage;
        public static UpgradeableStat GetMaxEnergyStat(int playerNum) => GetPlayer(playerNum).EnergyMax;
        public static UpgradeableStat GetEnergyAbsorbStat(int playerNum) => GetPlayer(playerNum).EnergyAbsorb;
        public static UpgradeableStat GetSpecialGainStat(int playerNum) => GetPlayer(playerNum).SpecialGain;
        public static UpgradeableStat GetDashEnergyCostStat(int playerNum) => GetPlayer(playerNum).EnergyCostDash;

    }
}