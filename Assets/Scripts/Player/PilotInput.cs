using Battle.Controllers.Player;
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
    [FormerlySerializedAs("player")] [Range(1, 2)] public int playerNum; // player 1 or 2
    [SerializeField] private ControlMode controlMode;
    
    [SerializeField] private GameObject wings;
    [SerializeField] private PlayerMovement movementController;
    [SerializeField] private GameObject weapons;
    [SerializeField] private PlayerShoot weaponController;
    [SerializeField] private ModeIcon modeIcon;
    private float _movement;
    private Vector2 _weaponMovement = Vector2.zero;
    
    public void Start()
    {
        controlMode = PlayerStats.GetPlayerControlMode(playerNum);
        ActivateParts();
        modeIcon?.SetSprite(controlMode);
    }
    
    public void Update()
    {
        if (controlMode == ControlMode.Movement)
        {
            movementController.SetPlayerMovement(playerNum, _movement);
            _weaponMovement = Vector2.zero;
        }
        else if (controlMode == ControlMode.Shooting)
        {
            movementController.SetPlayerMovement(playerNum, 0.0f);
            _movement = 0;
            weaponController.MoveGunsBy(_weaponMovement);
        }
        
    }

    public void OnMoveLR(InputAction.CallbackContext context)
    {
        if (controlMode == ControlMode.Movement)
        {
            _movement = context.ReadValue<float>();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (controlMode == ControlMode.Shooting)
        {
            _weaponMovement = context.ReadValue<Vector2>();
        }
    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        if (!context.action.triggered) return;
        if (controlMode == ControlMode.Shooting) OnPrimaryShoot(context);
        if (controlMode == ControlMode.Movement) OnPrimaryJump();
    }

    private void OnPrimaryShoot(InputAction.CallbackContext context)
    {
        weaponController.ShootBasicBullets();
    }

    private void OnPrimaryJump()
    {
        movementController.RequestPlayerJump(playerNum);
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
            weaponController.ChargeHeavyBullet();
            return;
        }
        var elapsedTime = context.time - context.startTime;
        weaponController.ShootHeavyBullet((float) elapsedTime);
    }

    private void OnSecondaryMove(InputAction.CallbackContext context)
    {
        if (context.started) movementController.PerformDash(playerNum);
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
        weaponController.UseSpecialMove();
    }

    private void OnSpecialMove()
    {
        if (PlayerStats.GetSpecialControlMode(playerNum) != ControlMode.Movement) return;
        PlayerMovement.UseSpecialMove(playerNum);
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
            case ControlMode.Shooting when weaponController.CanSwitchControlMode():
                return;
            case ControlMode.Shooting:
                controlMode = ControlMode.Movement;
                break;
            default:
                return;
        }

        ActivateParts();
        PlayerStats.SavePlayerControlMode(playerNum, controlMode);
        modeIcon?.SetSprite(controlMode);
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