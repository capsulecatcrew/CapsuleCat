using UnityEngine;
using Battle.Hitboxes;
using Player.Controls;
using Player.Stats;

namespace Battle.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Killable playerBody;
        [SerializeField] private PlayerEnergyController p1Energy, p2Energy;
        [SerializeField] private PlayerSpecialController p1Special, p2Special;
        [SerializeField] private ShootingController p1Shoot, p2Shoot;
        [SerializeField] private Hitbox p1Shield, p2Shield;

        public delegate void StatChange(float change);

        public event StatChange OnHealthChange;
        public event StatChange OnP1EnergyChange, OnP2EnergyChange;
        public event StatChange OnP1SpecialChange, OnP2SpecialChange;

        public delegate void PlayerBulletShoot(Bullet bullet);
        public event PlayerBulletShoot OnP1BulletShoot, OnP2BulletShoot;
        
        public delegate void ShieldHit(float damage);
        public event ShieldHit OnP1ShieldHit, OnP2ShieldHit;

        public delegate void PlayerDeath();
        public event PlayerDeath OnPlayerDeath;

        public delegate void DeltaTimeUpdate(float deltaTime);
        public event DeltaTimeUpdate OnDeltaTimeUpdate;

        public void Awake()
        {
            PlayerStats.InitPlayerKillable(playerBody);
        }

        public void OnEnable()
        {
            SubscribeToAllEvents();
        }
        
        public void Update()
        {
            OnDeltaTimeUpdate?.Invoke(Time.deltaTime);
        }

        public void OnDisable()
        {
            UnsubscribeFromAllEvents();
        }

        public void InitBars(
            ProgressBar healthBar,
            ProgressBar p1EnergyBar, ProgressBar p2EnergyBar,
            ProgressBar p1SpecialBar, ProgressBar p2SpecialBar)
        {
            playerBody.InitHealthBar(healthBar);
            p1Energy.InitBar(p1EnergyBar);
            p2Energy.InitBar(p2EnergyBar);
            p1Special.InitBar(p1SpecialBar);
            p2Special.InitBar(p2SpecialBar);
        }

        private void SubscribeToAllEvents()
        {
            playerBody.OnHealthChanged += HandlePlayerHealthChanged;
            playerBody.OnDeath += HandlePlayerDeath;
            p1Energy.OnEnergyChange += HandleP1EnergyChange;
            p2Energy.OnEnergyChange += HandleP2EnergyChange;
            p1Special.OnSpecialChange += HandleP1SpecialChange;
            p2Special.OnSpecialChange += HandleP2SpecialChange;
            p1Shoot.OnBulletShoot += HandleP1BulletShoot;
            p2Shoot.OnBulletShoot += HandleP2BulletShoot;
        }

        private void UnsubscribeFromAllEvents()
        {
            playerBody.OnHealthChanged -= HandlePlayerHealthChanged;
            playerBody.OnDeath -= HandlePlayerDeath;
            p1Energy.OnEnergyChange -= HandleP1EnergyChange;
            p2Energy.OnEnergyChange -= HandleP2EnergyChange;
            p1Special.OnSpecialChange -= HandleP1SpecialChange;
            p2Special.OnSpecialChange -= HandleP2SpecialChange;
            p1Shoot.OnBulletShoot -= HandleP1BulletShoot;
            p2Shoot.OnBulletShoot -= HandleP2BulletShoot;
        }

        private void HandlePlayerHealthChanged(float amount)
        {
            OnHealthChange?.Invoke(amount);
        }
        
        private void HandlePlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }

        private void HandleP1EnergyChange(float amount)
        {
            OnP1EnergyChange?.Invoke(amount);
        }

        private void HandleP2EnergyChange(float amount)
        {
            OnP2EnergyChange?.Invoke(amount);
        }

        private void HandleP1SpecialChange(float amount)
        {
            OnP1SpecialChange?.Invoke(amount);
        }

        private void HandleP2SpecialChange(float amount)
        {
            OnP2SpecialChange?.Invoke(amount);
        }
        
        private void HandleP1ShieldHit(float amount, DamageType unused)
        {
            OnP1ShieldHit?.Invoke(amount);
        }
        
        private void HandleP2ShieldHit(float amount, DamageType unused)
        {
            OnP2ShieldHit?.Invoke(amount);
        }

        private void HandleP1BulletShoot(Bullet bullet)
        {
            OnP1BulletShoot?.Invoke(bullet);
        }
        
        private void HandleP2BulletShoot(Bullet bullet)
        {
            OnP2BulletShoot?.Invoke(bullet);
        }

        public bool HasEnergy(int playerNum, float amount)
        {
            return playerNum switch
            {
                1 => p1Energy.HasEnergy(amount),
                2 => p2Energy.HasEnergy(amount),
                _ => false
            };
        }
        
        public void UseEnergy(int playerNum, float amount)
        {
            switch (playerNum)
            {
                case 1:
                    p1Energy.UseEnergy(amount);
                    return;
                case 2:
                    p2Energy.UseEnergy(amount);
                    return;
            }
        }

        public void AddEnergy(int playerNum, float amount)
        {
            switch (playerNum)
            {
                case 1:
                    p1Energy.AddEnergy(amount);
                    return;
                case 2:
                    p2Energy.AddEnergy(amount);
                    return;
            }
        }

        public bool EnableShield(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    if (p2Shield.gameObject.activeSelf) return false;
                    p1Shield.gameObject.SetActive(true);
                    playerBody.EnableShield();
                    p1Shield.OnHitBox += HandleP1ShieldHit;
                    return true;
                case 2:
                    if (p1Shield.gameObject.activeSelf) return false;
                    p2Shield.gameObject.SetActive(true);
                    playerBody.EnableShield();
                    p2Shield.OnHitBox += HandleP2ShieldHit;
                    return true;
            }

            return false;
        }

        public void DisableShield(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    p1Shield.gameObject.SetActive(false);
                    p1Shield.OnHitBox -= HandleP1ShieldHit;
                    break;
                case 2:
                    p2Shield.gameObject.SetActive(false);
                    p2Shield.OnHitBox -= HandleP2ShieldHit;
                    break;
            }
            playerBody.DisableShield();
        }

        public bool HasSpecial(int playerNum, float amount)
        {
            return playerNum switch
            {
                1 => p1Special.HasSpecial(amount),
                2 => p2Special.HasSpecial(amount),
                _ => false
            };
        }

        public void UseSpecial(int playerNum, float amount)
        {
            switch (playerNum)
            {
                case 1:
                    p1Special.UseSpecial(amount);
                    return;
                case 2:
                    p2Special.UseSpecial(amount);
                    return;
            }
        }
        
        public static void StopSpecialMove(int playerNum)
        {
            PlayerStats.StopSpecialMove(playerNum);
        }

        public void Heal(float amount)
        {
            playerBody.Heal(amount);
        }
    }
}