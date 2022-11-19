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

    public bool rotate = true;

    public float rotateSpeed = 5;

    public delegate void IntParam(int value);

    public event IntParam PlayerHeal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate) transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
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
