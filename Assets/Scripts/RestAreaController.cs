using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestAreaController : MonoBehaviour
{
    public LevelLoader levelLoader;

    public HitboxTrigger nextLevelTeleport;
    public HitboxTrigger shopTeleport;
    
    public HealthBar playerHealthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayerHeal(int value)
    {
        PlayerStats.Hp.IncreaseCurrentValue(value);
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
        if (other.CompareTag("Player")) levelLoader.LoadLevel("Battle");
    }

    private void GoToShop(Collider other) {
        if (other.CompareTag("Player")) levelLoader.LoadLevel("Shop");
    }


    void UpdatePlayerHealthBar()
    {
        playerHealthBar.SetMax(PlayerStats.Hp.GetMaxValue());
        playerHealthBar.SetFill(PlayerStats.Hp.GetCurrentValue());
    }
}
