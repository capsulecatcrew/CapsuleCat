using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public HitboxTrigger hitbox;
    public AudioClip healingSound;
    public RestAreaController restAreaController;
    public int healAmount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnEnable()
    {
        hitbox.HitboxEnter += OnHitboxEnter;
    }

    private void OnDisable()
    {
        hitbox.HitboxEnter -= OnHitboxEnter;
    }

    public void SetHealAmount(int amount)
    {
        healAmount = amount;
    }

    private void OnHitboxEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            restAreaController.PlayerHeal(healAmount);
            GlobalAudio.AudioSource.PlayOneShot(healingSound);
            gameObject.SetActive(false);
        }
    }
}
