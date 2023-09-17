using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerControls PlayerController;

    public Transform pivot;
    public Transform mainBody;
    
    [Header("Movement")]
    private float _speed = 35;

    public float maxSpeed = 35;
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
    
    private float _playerOneMovement;
    private float _playerTwoMovement;
    private bool _playerOneCanJump = true;
    private bool _playerTwoCanJump = true; 
    
    // Start is called before the first frame update
    void Awake()
    {
        PlayerController = new PlayerControls();
        _groundYPos = mainBody.position.y;
        _isGrounded = true;
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
        if (player == 1)
        {
            _playerOneMovement = value;
        }
        else if (player == 2)
        {
            _playerTwoMovement = value;
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

        if (player == 1 && _playerOneCanJump)
        {
            _playerOneCanJump = false;
            success = true;
        }
        else if (player == 2 && _playerTwoCanJump)
        {
            _playerTwoCanJump = false;
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
        var movement = _playerOneMovement + _playerTwoMovement;

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
                _playerOneCanJump = true;
                _playerTwoCanJump = true;
            }
        }
    }
    
    public void UseSpecialMove(int playerNum)
    {
        PlayerStats.UseSpecialMove(playerNum);
    }
}
