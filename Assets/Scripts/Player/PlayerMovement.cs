using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerControls PlayerController;
    [SerializeField] private BattleManager battleManager;

    public Transform pivot;
    public Transform mainBody;
    
    [Header("Movement")]
    private float _speed = 35;
    public float maxSpeed = 35;


    [SerializeField] private float dashMultiplier = 3.0f;
    private float _dashDuration = 0.15f;
    private float[] _dashEnergyCost = new float[2] {10f, 10f};

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
    private bool _isHoldingJump;

    private float _currentVelocity;
    
    private float[] _playerMovement = new float[2] {0.0f, 0.0f};
    private float[] _playerLastMovement = new float[2] {1.0f, 1.0f};
    private bool[] _playerCanJump = new bool[2] {true, true};
    private bool[] _pausePlayerInput = new bool[2] {false, false};
    void Awake()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleManager>();
        PlayerController = new PlayerControls();
        _groundYPos = mainBody.position.y;
        _isGrounded = true;
    }

    void Start()
    {
        _dashEnergyCost[0] = PlayerStats.GetDashEnergyCost(1);
        _dashEnergyCost[1] = PlayerStats.GetDashEnergyCost(2);
    }

    public void slowSpeed(float multiplier)
    {
        _speed = maxSpeed * multiplier;
    }

    public void resetMaxSpeed()
    {
        _speed = maxSpeed;
    }

    private void OnEnable()
    {
        PlayerController.Enable();
    }
    
    private void OnDisable()
    {
        PlayerController.Disable();
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
        Debug.DrawLine(pivot.position, new Vector3(pivot.position.x + endX, pivot.position.y, pivot.position.z + endZ), Color.cyan);
        Debug.DrawLine(pivot.position, new Vector3(pivot.position.x - endX, pivot.position.y, pivot.position.z + endZ), Color.cyan);
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
        int p = player - 1;
        if (!_pausePlayerInput[p])
        {
            _playerMovement[p] = value;
            if (Math.Abs(value) > float.Epsilon) _playerLastMovement[p] = value;
        }
    }

    /**
     * =================
     * RequestPlayerJump
     * =================
     * For players to request a jump, results determined if player is able to jump
     *
     * @param player    which player is sending the jump command
     * @return          true if jump is successful; false otherwise.
     */
    public bool RequestPlayerJump(int player)
    {
        var success = false;
        int p = player - 1;
        if (_playerCanJump[p])
        {
            _playerCanJump[p] = false;
            success = true;
        }

        if (success)
        {
            if (_isGrounded)
            {
                _isGrounded = false;
            }
            _yVelocity = jumpSpeed;
            _airTime = 0;
            _jumpTime = jumpTime;
            _isHoldingJump = true;
        }

        return success;
    }

    // LateUpdate is called once per frame, after all Update calls
    void LateUpdate()
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
        // else if (Math.Abs(_currentVelocity) > float.Epsilon) // Slowdown before stopping after button is released
        // {
        //     _currentVelocity =  _currentVelocity > 0 ? Math.Max(_currentVelocity - stoppingSpeed * Time.deltaTime, 0) 
        //                                              : Math.Min(_currentVelocity + stoppingSpeed * Time.deltaTime, 0);
        //     pivot.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);
        // }
        else
        {
            _currentVelocity = 0.0f;
        }

        if (movementRange < 360) {
            var halfAngle = (float) movementRange * 0.5f;
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
        if (!_isGrounded)
        {   
            // Short hop, implementation unfinished
            // if (_airTime < shortJumpTime && _isHoldingJump && !PlayerController.Player1.Jump.IsPressed()) 
            // {
            //     _isHoldingJump = false;
            //     _jumpTime = shortJumpTime;
            // }

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
            if (mainBody.position.y <= _groundYPos)
            {
                var position = mainBody.position;
                position = new Vector3(position.x, _groundYPos, position.z);
                mainBody.position = position;
                _yVelocity = 0.0f;
                _isGrounded = true;
                _playerCanJump[0] = true;
                _playerCanJump[1] = true;
            }
        }
    }
    
    public void UseSpecialMove(int playerNum)
    {
        PlayerStats.UseSpecialMove(playerNum);
    }

    public void PerformDash(int playerNo)
    {
        int p = playerNo - 1;
        if (_pausePlayerInput[p]) return;
        if (!battleManager.HasEnergy(playerNo, _dashEnergyCost[p])) return;

        battleManager.UseEnergy(playerNo, _dashEnergyCost[p]);
        StartCoroutine(DashCoroutine(playerNo));
    }

    public IEnumerator DashCoroutine(int playerNo)
    {
        int p = playerNo - 1;
        _pausePlayerInput[p] = true;
        _playerMovement[p] = _playerLastMovement[p] > 0 ? dashMultiplier : -dashMultiplier;
        yield return new WaitForSeconds(_dashDuration);
        _playerMovement[p] = 0;
        _pausePlayerInput[p] = false;
    }
}
