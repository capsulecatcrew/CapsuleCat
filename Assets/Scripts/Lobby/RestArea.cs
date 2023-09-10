using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class RestArea : MonoBehaviour
{
    [SerializeField]
    private bool _p1Ready, _p2Ready = false;
    private bool _startingNextLevel = false;
    public UnityEvent m_OnBothPlayersReady;

    void Start()
    {
        SetUpPlayerInputs();
    }
    void Update()
    {
        // Trigger code when both players are ready
        if (!_startingNextLevel && _p1Ready && _p2Ready)
        {
            _startingNextLevel = true;
            m_OnBothPlayersReady.Invoke();
        }
    }

    /// <summary>
    /// Marks a player as ready.
    /// </summary>
    /// <param name="playerNo">Player that is ready</param>
    public void SetPlayerReady(int playerNo)
    {
        switch (playerNo)
        {
            case 1:
                _p1Ready = true;
                break;
            case 2:
                _p2Ready = true;
                break;
            default:
                break;
        }
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
