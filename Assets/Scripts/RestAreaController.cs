using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestAreaController : MonoBehaviour
{
    public LevelLoader levelLoader;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) levelLoader.LoadLevel("Battle");
    }

    public void PlayerHeal(int value)
    {
        PlayerStats.CurrentHp = PlayerStats.CurrentHp + value > PlayerStats.MaxHp
                                ? PlayerStats.MaxHp
                                : PlayerStats.CurrentHp + value;
        UpdatePlayerHealthBar();
    }

    public void PlayerIncreaseMaxHealth(int value)
    {
        PlayerStats.MaxHp += value;
        PlayerStats.CurrentHp += value;
        UpdatePlayerHealthBar();
    }

    void UpdatePlayerHealthBar()
    {
        playerHealthBar.SetMax(PlayerStats.MaxHp);
        playerHealthBar.SetFill(PlayerStats.CurrentHp);
    }
}
