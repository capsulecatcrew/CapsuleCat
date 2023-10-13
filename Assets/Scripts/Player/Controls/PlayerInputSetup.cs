using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Player.Controls
{
    public class PlayerInputSetup : MonoBehaviour
    {
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
            var player1 = PlayerInput.all[0];
            var player2 = PlayerInput.all[1];
    
            // Discard existing assignments.
            player1.user.UnpairDevices();
            player2.user.UnpairDevices();
    
            // Assign devices and control schemes.
            var gamepadCount = Gamepad.all.Count;
            if (gamepadCount >= 2)
            {
                InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
                InputUser.PerformPairingWithDevice(Gamepad.all[1], user: player2.user);
    
                player1.user.ActivateControlScheme("Gamepad");
                player2.user.ActivateControlScheme("Gamepad");
            }
            else if (gamepadCount == 1)
            {
                InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
                InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);
    
                player1.user.ActivateControlScheme("Gamepad");
                player2.user.ActivateControlScheme("SplitKeyboardL");
            }
            else
            {
                InputUser.PerformPairingWithDevice(Keyboard.current, user: player1.user);
                InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);
    
                player1.user.ActivateControlScheme(player1.defaultControlScheme);
                player2.user.ActivateControlScheme(player2.defaultControlScheme);
            }

        }
    }
}
