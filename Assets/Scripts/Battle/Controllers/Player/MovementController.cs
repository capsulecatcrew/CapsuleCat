using System;
using System.Collections;
using System.Linq;
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
        [SerializeField] private GameObject[] wallObjects;
        [SerializeField] private Transform mainBody;
        [SerializeField] private Transform pivot;

        [Header("Moving")]
        private float _speed = 35;
        [SerializeField] private float maxSpeed = 35;
        private float _currentVelocity;
        private readonly float[] _playerMovement = { 0.0f, 0.0f };
        private readonly float[] _playerLastMovement = { 1.0f, 1.0f };
        private float _lastRotation;
        private bool _isLeftWalled;
        private bool _isRightWalled;
        private Wall _currentWall;

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
            foreach (var wall in walls)
            {
                wall.OnEnterLeft += HandleHitLeftWall;
                wall.OnEnterRight += HandleHitRightWall;
                wall.OnExit += HandleExitWall;
            }
        }

        public void Start()
        {
            _dashEnergyCost[0] = PlayerStats.GetDashEnergyCost(1);
            _dashEnergyCost[1] = PlayerStats.GetDashEnergyCost(2);
        }

        private void HandleHitGround(Collider other)
        {
            if (other.gameObject != stage && !wallObjects.Contains(other.gameObject)) return;
            _isGrounded = true;
        }

        private void HandleExitGround(Collider other)
        {
            if (other.gameObject != stage && !wallObjects.Contains(other.gameObject)) return;
            _isGrounded = false;
        }

        private void HandleHitLeftWall(Wall wall)
        {
            _currentWall = wall;
            _isLeftWalled = true;
        }
        
        private void HandleHitRightWall(Wall wall)
        {
            _currentWall = wall;
            _isRightWalled = true;
        }

        private void HandleExitWall()
        {
            _currentWall = null;
            _isLeftWalled = false;
            _isRightWalled = false;
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
            CheckForBlocks();
            
            var movement = _playerMovement[0] + _playerMovement[1];

            if (_isRightWalled && movement > 0) movement = 0;
            if (_isLeftWalled && movement < 0) movement = 0;

            if (movement != 0)
            {
                _lastRotation = pivot.rotation.eulerAngles.y;
                _currentVelocity = _speed * movement;
                pivot.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);
            }
            else
            {
                _currentVelocity = 0.0f;
            }

            if (_dashCooldownTimer <= 0) return;
            _dashCooldownTimer -= Time.deltaTime;
        }

        private void CheckForBlocks()
        {
            _currentWall?.CheckWallExit(mainBody, pivot);
            foreach (var wall in walls)
            {
                wall.CheckWallEntry(mainBody, pivot, _lastRotation);
            }
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
            [SerializeField] private float top;
            [SerializeField] private float bottom;

            public delegate void EnterUpdate(Wall wall);
            public event EnterUpdate OnEnterLeft;
            public event EnterUpdate OnEnterRight;

            public delegate void ExitUpdate();
            public event ExitUpdate OnExit;

            public void CheckWallEntry(Transform mainBody, Transform pivot, float lastRotation)
            {
                if (!IsWithinHeight(mainBody)) return;
                var rot = pivot.rotation.eulerAngles.y;
                if (lastRotation <= left && lastRotation >= right) return;
                if (rot > left || rot < right) return;
                if (lastRotation <= left) OnEnterLeft?.Invoke(this);
                if (lastRotation >= right) OnEnterRight?.Invoke(this);
            }

            public void CheckWallExit(Transform mainBody, Transform pivot)
            {
                if (!IsWithinHeight(mainBody)) OnExit?.Invoke();
                if (!IsWithinWidth(pivot)) OnExit?.Invoke();
            }

            private bool IsWithinHeight(Transform mainBody)
            {
                var yPos = mainBody.position.y;
                if (yPos + 0.75 <= bottom) return false;
                return !(yPos - 0.75 >= top);
            }

            private bool IsWithinWidth(Transform pivot)
            {
                var rot = pivot.rotation.eulerAngles.y;
                if (rot > left) return false;
                return !(rot < right);
            }
        }
    }
}