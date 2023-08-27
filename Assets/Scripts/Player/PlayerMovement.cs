using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerControls PlayerController;

    public Transform pivot;
    public Transform mainBody;
    
    [Header("Movement")]
    public float maxSpeed = 50;
    public float stoppingSpeed = 150;
    [Range(0, 360)]
    public int movementRange = 360;
    
    [Header("Jump")]
    public float shortJumpTime = 0.1f;
    public float longJumpTime = 0.2f;
    public float jumpSpeed = 3;
    public float weight = 1;
    public float jumpDecel = 100;
    private static float _gravityConstant = -9.81f;
    private bool _isGrounded;

    private float _lastGroundedY;
    private float _yVelocity;
    private float _airTime;
    private float _jumpTime;
    private bool _isHoldingJump = false;

    private float _currentVelocity;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        PlayerController = new PlayerControls();
        _lastGroundedY = mainBody.position.y;
        _isGrounded = true;
    }

    private void OnEnable()
    {
        PlayerController.Enable();
    }
    
    private void OnDisable()
    {
        PlayerController.Disable();
    }

    private void OnDrawGizmos()
    {
        float length = Math.Abs(mainBody.localPosition.z) + 5;
        float halfAngle = movementRange * 0.5f * Mathf.Deg2Rad;
        float endX = length * Mathf.Sin(halfAngle);
        float endZ = length * Mathf.Cos(halfAngle);
        Debug.DrawLine(pivot.position, new Vector3(pivot.position.x + endX, pivot.position.y, pivot.position.z + endZ), Color.cyan);
        Debug.DrawLine(pivot.position, new Vector3(pivot.position.x - endX, pivot.position.y, pivot.position.z + endZ), Color.cyan);
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Rotational Movement
         */
        float movement = PlayerController.Player1.MoveLR.ReadValue<float>();
        if (movement != 0)
        {
            _currentVelocity = maxSpeed * movement;
            pivot.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);
        }
        else if (Math.Abs(_currentVelocity) > float.Epsilon)
        {
            _currentVelocity =  _currentVelocity > 0 ? Math.Max(_currentVelocity - stoppingSpeed * Time.deltaTime, 0) 
                                                     : Math.Min(_currentVelocity + stoppingSpeed * Time.deltaTime, 0);
            pivot.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);
        }
        else
        {
            _currentVelocity = 0.0f;
        }

        if (movementRange < 360) {
            float halfAngle = (float) movementRange * 0.5f;
            float rot = pivot.rotation.eulerAngles.y;
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
        if (_isGrounded && PlayerController.Player1.Jump.triggered)
        {
            _isGrounded = false;
            _lastGroundedY = mainBody.position.y;
            _yVelocity = jumpSpeed;
            _airTime = 0;
            _jumpTime = longJumpTime;
            _isHoldingJump = true;
        }

        if (!_isGrounded)
        {   
            if (_airTime < shortJumpTime && _isHoldingJump && !PlayerController.Player1.Jump.IsPressed()) 
            {
                _isHoldingJump = false;
                _jumpTime = shortJumpTime;
            }

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
                    _yVelocity += _gravityConstant * weight * Time.deltaTime;
                }
            }
            
            mainBody.position += new Vector3(0, _yVelocity, 0) * Time.deltaTime;
            if (mainBody.position.y <= _lastGroundedY)
            {
                var position = mainBody.position;
                position = new Vector3(position.x, _lastGroundedY, position.z);
                mainBody.position = position;
                _yVelocity = 0.0f;
                _isGrounded = true;
            }
        }

    }
}
