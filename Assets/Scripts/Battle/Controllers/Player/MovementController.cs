using System;
using System.Collections;
using Player.Stats;
using UnityEngine;

namespace Battle.Controllers.Player
{
    public class MovementController : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerSoundController playerSoundController;
        
        [Header("Physics")]
        [SerializeField] private HitboxTrigger playerBodyHitbox;
        [SerializeField] private Rigidbody playerBody;
        [SerializeField] private GameObject stage;
        [SerializeField] private Wall[] walls;
        [SerializeField] private Transform mainBody;
        [SerializeField] private Transform pivot;

        [Header("Moving")]
        private float _speed = 35;
        [SerializeField] private float maxSpeed = 35;
        private float _currentVelocity;
        private readonly float[] _playerMovement = { 0.0f, 0.0f };
        private readonly float[] _playerLastMovement = { 1.0f, 1.0f };

        [Header("Jumping")]
        // btw gravity is set to 30, that's why this value is this high
        [SerializeField] private float jumpSpeed = 18;
        
        [Header("Dash")]
        [SerializeField] private ParticleSystem[] dashParticleSystems;
        [SerializeField] private float dashMultiplier = 3.0f;
        [SerializeField] private float dashDuration = 0.15f;
        [SerializeField] private float dashCooldown = 0.25f;
        private float _dashCooldownTimer;
        private readonly float[] _dashEnergyCost = {10f, 10f};

        private bool _isGrounded;
        private readonly bool[] _pausePlayerInput = {false, false};
        
        public void OnEnable()
        {
            playerBodyHitbox.HitboxEnter += HandleHitGround;
            playerBodyHitbox.HitboxExit += HandleExitGround;
        }

        public void Start()
        {
            _dashEnergyCost[0] = PlayerStats.GetDashEnergyCost(1);
            _dashEnergyCost[1] = PlayerStats.GetDashEnergyCost(2);
        }

        private void HandleHitGround(Collider other)
        {
            if (other.gameObject != stage) return;
            _isGrounded = true;
        }

        private void HandleExitGround(Collider other)
        {
            if (other.gameObject != stage) return;
            _isGrounded = false;
        }

        public void Jump()
        {
            if (!_isGrounded) return;
            playerBody.velocity = new Vector3(0, jumpSpeed, 0);
        }

        public void SetMoveAmount(int player, float value)
        {
            var p = player - 1;
            if (_pausePlayerInput[p]) return;
            _playerMovement[p] = value;
            if (Math.Abs(value) > float.Epsilon) _playerLastMovement[p] = value;
        }

        public void SlowSpeed(float multiplier)
        {
            _speed = maxSpeed * multiplier;
        }

        public void ResetMaxSpeed()
        {
            _speed = maxSpeed;
        }

        public void FixedUpdate()
        {
            var movement = _playerMovement[0] + _playerMovement[1];

            if (movement != 0)
            {
                _currentVelocity = _speed * movement;
                pivot.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);
            }
            else
            {
                _currentVelocity = 0.0f;
            }
            
            var rot = pivot.rotation.eulerAngles.y;
            print(rot);

            if (_dashCooldownTimer <= 0) return;
            _dashCooldownTimer -= Time.deltaTime;
        }

        private bool IsBlockedLeft()
        {
            var rot = pivot.rotation.eulerAngles.y;
            return false;
            // foreach (var wall in walls)
            // {
            //     if ()
            // }
        }
        
        public void Dash(int playerNum)
        {
            if (_dashCooldownTimer > 0) return;
            var p = playerNum - 1;
            if (_pausePlayerInput[p]) return;
            if (!playerController.HasEnergy(playerNum, _dashEnergyCost[p])) return;

            playerController.UseEnergy(playerNum, _dashEnergyCost[p]);
            StartCoroutine(DashCoroutine(playerNum));
        
        }

        private IEnumerator DashCoroutine(int playerNum)
        {
            var p = playerNum - 1;
            _pausePlayerInput[p] = true;
            _playerMovement[p] = _playerLastMovement[p] > 0 ? dashMultiplier : -dashMultiplier;
            yield return new WaitForSeconds(dashDuration);
            _playerMovement[p] = 0;
            _pausePlayerInput[p] = false;
            _dashCooldownTimer = dashCooldown;
            PlayDashParticles(playerNum);
            playerSoundController.PlayDashSound();
        }

        private void PlayDashParticles(int playerNum)
        {
            var p = playerNum - 1;
            dashParticleSystems[p].transform.forward = _playerLastMovement[p] > 0 ? mainBody.right : -mainBody.right;
            dashParticleSystems[p].Play();
        }
        
        public static void UseSpecialMove(int playerNum)
        {
            PlayerStats.UseSpecialMove(playerNum);
        }

        [Serializable]
        private class Wall
        {
            [SerializeField] private float left;
            [SerializeField] private float right;
            [SerializeField] private float height;
            [SerializeField] private float leftEnter;
            [SerializeField] private float rightEnter;

            public bool IsInWall(Transform mainBody, float rotation)
            {
                if (mainBody.position.y >= height) return false;
                return rotation >= leftEnter && rotation <= rightEnter;
            }
        }
    }
}