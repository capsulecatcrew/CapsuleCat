using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestAreaController : MonoBehaviour
{
    private static bool _healthBoxUsed = false;
    private static bool _comingFromShop = false;
    
    public Transform defaultSpawn;
    public Transform fromShopSpawn;

    public GameObject player;
    
    public GameObject healthBox;
    public LevelLoader levelLoader;

    public HitboxTrigger nextLevelTeleport;
    public HitboxTrigger shopTeleport;
    
    public HealthBar playerHealthBar;
    
    // Start is called before the first frame update
    void Awake()
    {
        healthBox.SetActive(!_healthBoxUsed);
        UpdatePlayerHealthBar();
        Transform spawnPoint;
        if (_comingFromShop)
        {
            spawnPoint = fromShopSpawn;
        }
        else
        {
            spawnPoint = defaultSpawn;
        }

        player.SetActive(false);
        player.transform.position = spawnPoint.position;
        var rotation = spawnPoint.rotation;
        player.transform.Rotate(rotation.x, rotation.y, rotation.z);
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayerHeal(int value)
    {
        PlayerStats.Hp.IncreaseCurrentValue(value);
        _healthBoxUsed = true;
        UpdatePlayerHealthBar();
    }

    private void OnEnable()
    {
        nextLevelTeleport.HitboxEnter += GoToNextLevel;
        shopTeleport.HitboxEnter += GoToShop;
    }

    private void OnDisable()
    {
        nextLevelTeleport.HitboxEnter += GoToNextLevel;
        shopTeleport.HitboxEnter -= GoToShop;
    }
    
    private void GoToNextLevel(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        levelLoader.LoadLevel("Battle");
        _healthBoxUsed = false;
        _comingFromShop = false;
    }

    private void GoToShop(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        levelLoader.LoadLevel("Shop");
        _comingFromShop = true;
    }


    void UpdatePlayerHealthBar()
    {
        playerHealthBar.SetMax(PlayerStats.Hp.GetMaxValue());
        playerHealthBar.SetFill(PlayerStats.Hp.GetCurrentValue());
    }
}
