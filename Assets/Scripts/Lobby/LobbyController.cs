using Player.Stats;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LobbyController : MonoBehaviour
{
    private static bool _fromShop = false;
    [Header("Player GameObjects")]
    public GameObject p1;
    public GameObject p2;

    [Header("Player Spawn Points")]
    public Transform p1DefaultSpawn;
    public Transform p2DefaultSpawn;
    
    public Transform p1FromShopSpawn;
    public Transform p2FromShopSpawn;
    
    [Header("Player UI Avatars")]
    public Avatar p1Avatar;
    public Avatar p2Avatar;
    [Header("Player Status")] 
    public TMP_Text p1Status;
    public TMP_Text p2Status;
    private bool _p1Ready, _p2Ready;
    private bool _startingNextLevel;
    public UnityEvent m_OnBothPlayersReady;
    
    [Header("Players Health Bar")]
    [SerializeField] private ProgressBar healthBar;

    private void OnEnable()
    {
        PlayerStats.InitPlayerHealthBarMax(healthBar);
        PlayerStats.InitPlayerHealthBarValue(healthBar);
    }

    private void Start()
    {
        if (_fromShop)
        {
            p1.transform.position = p1FromShopSpawn.position;
            p2.transform.position = p2FromShopSpawn.position;
        }
        else
        {
            p1.transform.position = p1DefaultSpawn.position;
            p2.transform.position = p2DefaultSpawn.position;
        }
    }
    
    private void Update()
    {
        // Trigger code when both players are ready
        if (_startingNextLevel || !_p1Ready || !_p2Ready) return;
        _startingNextLevel = true;
        SetSpawnToDefault();
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
                p1Status.text = "<color=#7DFBAE>READY";
                break;
            case 2:
                _p2Ready = true;
                p2Avatar.SetBackgroundSprite(1);
                p2Avatar.SetCharacterSprite(1);
                p2Status.text = "<color=#7DFBAE>READY";
                break;
        }
    }
    
    public void SetSpawnToShop()
    {
        _fromShop = true;
    }

    public void SetSpawnToDefault()
    {
        _fromShop = false;
    }
}
