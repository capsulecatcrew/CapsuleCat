using System;
using System.Collections;
using Battle.Controllers.Player;
using Player.Stats;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerControls _playerControls;
    
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerSoundController playerSoundController;

    public Transform pivot;
    public Transform mainBody;
    
    [Header("Movement")]
    private float _speed = 35;
    public float maxSpeed = 35;

    [SerializeField] private float dashMultiplier = 3.0f;
    private const float DashDuration = 0.15f;
    private readonly float[] _dashEnergyCost = {10f, 10f};
    private const float DashCooldown = 0.25f;
    private float _dashCooldownTimer;

    [Range(0, 360)]
    public int movementRange = 360;
    
    [Header("Jump")]
    public float jumpTime = 0.2f;
    public float jumpSpeed = 3;
    public float weight = 1;
    public float jumpDecel = 100;
    private const float GravityConstant = -9.81f;
    private bool _isGrounded;

    private float _groundYPos;
    private float _yVelocity;
    private float _airTime;
    private float _jumpTime;

    private float _currentVelocity;
    
    private readonly float[] _playerMovement = {0.0f, 0.0f};
    private readonly float[] _playerLastMovement = {1.0f, 1.0f};
    private readonly bool[] _playerCanJump = {true, true};
    private readonly bool[] _pausePlayerInput = {false, false};

    [SerializeField] private ParticleSystem[] dashParticleSystems;

    public void Awake()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleController>();
        _playerControls = new PlayerControls();
        _groundYPos = mainBody.position.y;
        _isGrounded = true;
    }

    public void Start()
    {
        _dashEnergyCost[0] = PlayerStats.GetDashEnergyCost(1);
        _dashEnergyCost[1] = PlayerStats.GetDashEnergyCost(2);
    }

    public void Update()
    {
        if (_dashCooldownTimer <= 0) return;
        _dashCooldownTimer -= Time.deltaTime;
    }

    public void SlowSpeed(float multiplier)
    {
        _speed = maxSpeed * multiplier;
    }

    public void ResetMaxSpeed()
    {
        _speed = maxSpeed;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }
    
    private void OnDisable()
    {
        _playerControls.Disable();
    }

    /**
     * Code to run to render gizmos in the Scene view for visual debugging.
     * Shows the arc in which the player is allowed to move.
     */
    private void OnDrawGizmos()
    {
        var length = Math.Abs(mainBody.localPosition.z) + 5;
        var halfAngle = movementRange * 0.5f * Mathf.Deg2Rad;
        var endX = length * Mathf.Sin(halfAngle);
        var endZ = length * Mathf.Cos(halfAngle);
        var position = pivot.position;
        Debug.DrawLine(position, new Vector3(position.x + endX, position.y, position.z + endZ), Color.cyan);
        Debug.DrawLine(position, new Vector3(position.x - endX, position.y, position.z + endZ), Color.cyan);
    }

    /**
     * =================
     * SetPlayerMovement
     * =================
     * Takes in a movement input and adds it to the net movement value.
     * 
     * @param player    which player is inputting (1 or 2)
     * @param value     value of player input
     */
    public void SetPlayerMovement(int player, float value)
    {
        var p = player - 1;
        if (_pausePlayerInput[p]) return;
        _playerMovement[p] = value;
        if (Math.Abs(value) > float.Epsilon) _playerLastMovement[p] = value;
    }

    /// <summary>
    /// Requests for specified player to jump.
    /// </summary>
    /// <param name="playerNum">Number of player sending the jump command.</param>
    /// <returns>true if jump is successful; false otherwise.</returns>
    public bool RequestPlayerJump(int playerNum)
    {
        var success = false;
        var p = playerNum - 1;
        if (_playerCanJump[p])
        {
            _playerCanJump[p] = false;
            success = true;
            playerSoundController.PlayJumpSound(playerNum);
        }

        if (!success) return false;
        if (_isGrounded)
        {
            _isGrounded = false;
        }
        _yVelocity = jumpSpeed;
        _airTime = 0;
        _jumpTime = jumpTime;

        return true;
    }

    // LateUpdate is called once per frame, after all Update calls
    public void LateUpdate()
    {
        /*
         * Rotational Movement
         */
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

        if (movementRange < 360) {
            var halfAngle = movementRange * 0.5f;
            var rot = pivot.rotation.eulerAngles.y;
            if (rot > halfAngle && rot <= 180)
            {
                pivot.rotation = Quaternion.Euler(0, halfAngle, 0);
            }
            else if (rot < 360 - halfAngle && rot > 180)
            {
                pivot.rotation = Quaternion.Euler(0, 360 - halfAngle, 0);
            }
        }

        /*
         * Jumping
         */
        if (_isGrounded) return;
        _airTime += Time.deltaTime;
        if (_airTime > _jumpTime)
        {
            if (_yVelocity > 0.0f)
            {
                _yVelocity += -jumpDecel * weight * Time.deltaTime;
                if (_yVelocity < 0.0f) _yVelocity = 0.0f;
            }
            else
            {
                _yVelocity += GravityConstant * weight * Time.deltaTime;
            }
        }
            
        mainBody.position += new Vector3(0, _yVelocity, 0) * Time.deltaTime;
        if (mainBody.position.y > _groundYPos) return;
        var position = mainBody.position;
        position = new Vector3(position.x, _groundYPos, position.z);
        mainBody.position = position;
        _yVelocity = 0.0f;
        _isGrounded = true;
        _playerCanJump[0] = true;
        _playerCanJump[1] = true;
    }
    
    public static void UseSpecialMove(int playerNum)
    {
        PlayerStats.UseSpecialMove(playerNum);
    }

    public void PerformDash(int playerNum)
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
        yield return new WaitForSeconds(DashDuration);
        _playerMovement[p] = 0;
        _pausePlayerInput[p] = false;
        dashParticleSystems[p].transform.forward = _playerLastMovement[p] > 0 ? mainBody.right : -mainBody.right;
        _dashCooldownTimer = DashCooldown;
        dashParticleSystems[p].Play();
        playerSoundController.PlayDashSound();
    }
}
