using System;
using Battle.Controllers.Player;
using Player.Controls;
using Player.Stats;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum ControlMode
{
    Movement,
    Shooting
}

[RequireComponent(typeof(PlayerInput))]
public class PilotInput : MonoBehaviour
{
    [Header("Trackers")]
    [FormerlySerializedAs("player")] [Range(1, 2)] public int playerNum; // player 1 or 2
    [SerializeField] private ControlMode controlMode;
    
    [Header("Objects")]
    [SerializeField] private GameObject wings;
    [SerializeField] private GameObject weapons;
    [SerializeField] private ModeIcon modeIcon;
    
    [Header("Controllers")]
    [SerializeField] private ShootingController shootingController;
    [SerializeField] private MovementController movementController;
    
    private float _moveAmount;
    private Vector2 _weaponMoveAmount = Vector2.zero;
    
    public void Start()
    {
        controlMode = PlayerStats.GetPlayerControlMode(playerNum);
        ActivateParts();
        if (!modeIcon.isActiveAndEnabled) return;
        modeIcon.SetSprite(controlMode);
    }
    
    public void Update()
    {
        switch (controlMode)
        {
            case ControlMode.Movement:
                movementController.SetMoveAmount(playerNum, _moveAmount);
                _weaponMoveAmount = Vector2.zero;
                break;
            case ControlMode.Shooting:
                movementController.SetMoveAmount(playerNum, 0.0f);
                _moveAmount = 0;
                shootingController.MoveGunsBy(_weaponMoveAmount);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnMoveLR(InputAction.CallbackContext context)
    {
        if (controlMode == ControlMode.Movement)
        {
            _moveAmount = context.ReadValue<float>();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (controlMode == ControlMode.Shooting)
        {
            _weaponMoveAmount = context.ReadValue<Vector2>();
        }
    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        if (!context.action.triggered) return;
        if (controlMode == ControlMode.Shooting) OnPrimaryShoot(context);
        if (controlMode == ControlMode.Movement) OnPrimaryJump();
    }

    private void OnPrimaryShoot(InputAction.CallbackContext ignored)
    {
        shootingController.ShootBasicBullets();
    }

    private void OnPrimaryJump()
    {
        movementController.Jump();
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        if (controlMode == ControlMode.Shooting) OnSecondaryShoot(context);
        if (controlMode == ControlMode.Movement) OnSecondaryMove(context);
    }

    private void OnSecondaryShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shootingController.ChargeHeavyBullet();
            return;
        }
        var elapsedTime = context.time - context.startTime;
        shootingController.ShootHeavyBullet((float) elapsedTime);
    }

    private void OnSecondaryMove(InputAction.CallbackContext context)
    {
        if (context.started) movementController.Dash(playerNum);
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (!context.action.triggered) return;
        if (controlMode == ControlMode.Shooting) OnSpecialAttack();
        if (controlMode == ControlMode.Movement) OnSpecialMove();
    }

    private void OnSpecialAttack()
    {
        if (PlayerStats.GetSpecialControlMode(playerNum) != ControlMode.Shooting) return;
        shootingController.UseSpecialMove();
    }

    private void OnSpecialMove()
    {
        if (PlayerStats.GetSpecialControlMode(playerNum) != ControlMode.Movement) return;
        MovementController.UseSpecialMove(playerNum);
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (!context.action.triggered) return;
        PlayerController.StopSpecialMove(playerNum);
        switch (controlMode)
        {
            case ControlMode.Movement:
                controlMode = ControlMode.Shooting;
                break;
            case ControlMode.Shooting when shootingController.CanSwitchControlMode():
                return;
            case ControlMode.Shooting:
                controlMode = ControlMode.Movement;
                break;
            default:
                return;
        }

        ActivateParts();
        PlayerStats.SavePlayerControlMode(playerNum, controlMode);
        if (!modeIcon.isActiveAndEnabled) return;
        modeIcon.SetSprite(controlMode);
    }

    /**
     * =============
     * ActivateParts
     * =============
     * Sets the wings/cannons active based on what the current control mode is.
     * 
     */
    void ActivateParts()
    {
        if (controlMode == ControlMode.Movement)
        {
            wings.SetActive(true);
            weapons.SetActive(false);
        }
        else if (controlMode == ControlMode.Shooting)
        {
            wings.SetActive(false);
            weapons.SetActive(true);
        }
    }
}