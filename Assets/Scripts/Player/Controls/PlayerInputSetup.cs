using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

namespace Player.Controls
{
    public class PlayerInputSetup : MonoBehaviour
    {
        [SerializeField] private PlayerInput p1Input;
        [SerializeField] private PlayerInput p2Input;
        // Start is called before the first frame update
        void Start()
        {
            SetUpPlayerInputs();
        }

        /// <summary>
        /// Sets the local multiplayer input devices according to how many gamepads/controllers are connected.
        /// </summary>
        void SetUpPlayerInputs()
        {
            // Discard existing assignments.
            p1Input.user.UnpairDevices();
            p2Input.user.UnpairDevices();
    
            // Assign devices and control schemes.
            var gamepadCount = Gamepad.all.Count;
            switch (gamepadCount)
            {
                case >= 2:
                    InputUser.PerformPairingWithDevice(Gamepad.all[0], user: p1Input.user);
                    InputUser.PerformPairingWithDevice(Gamepad.all[1], user: p2Input.user);
    
                    p1Input.user.ActivateControlScheme("Gamepad");
                    p2Input.user.ActivateControlScheme("Gamepad");
                    break;
                case 1:
                    InputUser.PerformPairingWithDevice(Gamepad.all[0], user: p1Input.user);
                    InputUser.PerformPairingWithDevice(Keyboard.current, user: p2Input.user);
    
                    p1Input.user.ActivateControlScheme("Gamepad");
                    p2Input.user.ActivateControlScheme("SplitKeyboardL");
                    break;
                default:
                    InputUser.PerformPairingWithDevice(Keyboard.current, user: p1Input.user);
                    InputUser.PerformPairingWithDevice(Keyboard.current, user: p2Input.user);
    
                    p1Input.user.ActivateControlScheme("SplitKeyboardL");
                    p2Input.user.ActivateControlScheme("SplitKeyboardR");
                    break;
            }

        }
    }
}
