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

    private float _movement = 0;
    private Vector2 _weaponMovement = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        ActivateParts();
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
        if (context.action.triggered)
        {
            switch (controlMode)
            {
                case ControlMode.Movement:
                    movementController.RequestPlayerJump(player);
                    break;
                case ControlMode.Shooting:
                    weaponController.ShootBullet();
                    break;
                default:
                    break;
            }
        }
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
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
                controlMode = ControlMode.Movement;
            }

            ActivateParts();
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