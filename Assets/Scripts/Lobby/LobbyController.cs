using Player.Stats;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LobbyController : MonoBehaviour
{
    public Avatar p1Avatar, p2Avatar;
    public TMP_Text p1Status, p2Status;
    private bool _p1Ready, _p2Ready;
    private bool _startingNextLevel;
    public UnityEvent m_OnBothPlayersReady;
    
    [SerializeField] private ProgressBar healthBar;

    public void OnEnable()
    {
        PlayerStats.InitPlayerHealthBarMax(healthBar);
        PlayerStats.InitPlayerHealthBarValue(healthBar);
    }

    public void Update()
    {
        // Trigger code when both players are ready
        if (_startingNextLevel || !_p1Ready || !_p2Ready) return;
        _startingNextLevel = true;
        m_OnBothPlayersReady.Invoke();
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
                p1Avatar.SetBackgroundSprite(1);
                p1Avatar.SetCharacterSprite(1);
                p1Status.text = "READY";
                break;
            case 2:
                _p2Ready = true;
                p2Avatar.SetBackgroundSprite(1);
                p2Avatar.SetCharacterSprite(1);
                p2Status.text = "READY";
                break;
        }
    }
    
}
