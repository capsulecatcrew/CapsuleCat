using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerControls PlayerController;

    public float maxSpeed = 50;
    public float stoppingSpeed = 150;

    public float jumpTime = 1;
    public float jumpSpeed = 3;
    public float weight = 1;
    public float jumpDecel = 10;
    private static float _gravityConstant = -9.81f;
    private bool _isGrounded;

    private float _lastGroundedY;
    private float _yVelocity;
    private float _airTime;

    private float _currentVelocity;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        PlayerController = new PlayerControls();
        _lastGroundedY = transform.position.y;
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

    // Update is called once per frame
    void Update()
    {
        float movement = PlayerController.Land.MoveLR.ReadValue<float>();
        if (movement != 0)
        {
            _currentVelocity = maxSpeed * movement;
        }
        else if (Math.Abs(_currentVelocity) > float.Epsilon)
        {
            _currentVelocity =  _currentVelocity > 0 ? _currentVelocity - stoppingSpeed * Time.deltaTime 
                                                     : _currentVelocity + stoppingSpeed * Time.deltaTime;
            _currentVelocity = Math.Abs(_currentVelocity) < float.Epsilon ? 0 : _currentVelocity;
        }
        transform.Rotate(new Vector3(0, 1, 0), -1 * _currentVelocity * Time.deltaTime);

        if (_isGrounded && PlayerController.Land.Jump.triggered)
        {
            _isGrounded = false;
            _lastGroundedY = transform.position.y;
            _yVelocity = jumpSpeed;
            _airTime = 0;
        }

        if (!_isGrounded)
        {
            _airTime += Time.deltaTime;
            if (_airTime > jumpTime)
            {
                if (_yVelocity > 0.0f)
                {
                    _yVelocity += _gravityConstant * jumpDecel * weight * Time.deltaTime;
                    if (_yVelocity < 0.0f) _yVelocity = 0.0f;
                }
                else
                {
                    _yVelocity += _gravityConstant * weight * Time.deltaTime;
                }
            }
            
            transform.position += new Vector3(0, _yVelocity, 0) * Time.deltaTime;
            if (transform.position.y <= _lastGroundedY)
            {
                transform.position = new Vector3(transform.position.x, _lastGroundedY, transform.position.z);
                _yVelocity = 0.0f;
                _isGrounded = true;
            }
        }

    }
}
