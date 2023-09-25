using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlMode
{
    Movement,
    Shooting
}

[RequireComponent(typeof(PlayerInput))]
public class PilotInput : MonoBehaviour
{
    [Range(1, 2)] public int player; // player 1 or 2
    public ControlMode controlMode;

    public GameObject wings;
    public PlayerMovement movementController;
    public GameObject weapons;
    public PlayerShoot weaponController;
    public ModeIcon modeIcon;
    private float _movement;
    private Vector2 _weaponMovement = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        controlMode = PlayerStats.GetLastPlayerControlMode(player);
        ActivateParts();
        modeIcon?.SetSprite(controlMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (controlMode == ControlMode.Movement)
        {
            movementController.SetPlayerMovement(player, _movement);
            _weaponMovement = Vector2.zero;
        }
        else if (controlMode == ControlMode.Shooting)
        {
            movementController.SetPlayerMovement(player, 0.0f);
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
        movementController.RequestPlayerJump(player);
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
        if (context.started) movementController.PerformDash(player);
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (!context.action.triggered) return;
        if (controlMode == ControlMode.Shooting) OnSpecialAttack();
        if (controlMode == ControlMode.Movement) OnSpecialMove();
    }

    public void OnSpecialAttack()
    {
        if (PlayerStats.GetSpecialControlMode(player) != ControlMode.Shooting) return;
        weaponController.UseSpecialMove();
    }

    public void OnSpecialMove()
    {
        if (PlayerStats.GetSpecialControlMode(player) != ControlMode.Movement) return;
        movementController.UseSpecialMove(player);
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (controlMode == ControlMode.Movement)
            {
                controlMode = ControlMode.Shooting;
            }
            else if (controlMode == ControlMode.Shooting)
            {
                if (weaponController.CanSwitchControlMode()) return;
                controlMode = ControlMode.Movement;
            }

            ActivateParts();
            PlayerStats.SavePlayerControlMode(player, controlMode);
            modeIcon?.SetSprite(controlMode);
        }
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