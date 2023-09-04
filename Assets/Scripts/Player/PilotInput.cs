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

    private float _movement;
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

    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        print("bye");
        if (!context.action.triggered) return;
        print("bye1");
        if (controlMode != ControlMode.Shooting) return;
        print("bye2");
        weaponController.ShootBullet();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        print("hi");
        if (!context.action.triggered) return;
        print("hi1");
        if (controlMode != ControlMode.Movement) return;
        print("hi2");
        movementController.RequestPlayerJump(player);
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