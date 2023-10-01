using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using Battle.Hitboxes;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Hitbox hitbox;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private float flashSeconds = 0.2f;
    [SerializeField] private Color flashColor = Color.white;

    private List<Color> _colors = new List<Color>();
    private bool _isFlashing = false;
    private float _time;
    private void OnEnable()
    {
        hitbox.OnHitBox += StartFlashAnim;
    }
    private void OnDisable()
    {
        hitbox.OnHitBox -= StartFlashAnim;
    }

    private void StartFlashAnim(float dmg, DamageType unused)
    {
        if (dmg <= 0) return;
        if (_isFlashing) return;
        _isFlashing = true;
        UpdateColorBuffer();        
    }

    private void Update()
    {
        UpdateFlashTimer();
        FlashAnim();        
    }

    private void UpdateFlashTimer()
    {
        if (!_isFlashing) return;
        _time += Time.deltaTime;
    }

    private void ResetFlashTimer()
    {
        _time = 0;
    }

    private void UpdateColorBuffer()
    {
        _colors.Clear();
        foreach (var t in renderers)
        {
            _colors.Add(t.material.color);
        }
    }
    
    private void FlashAnim()
    {
        if (!_isFlashing) return;
        for (int i = 0; i < renderers.Length; i++)
        {
            if (!renderers[i].enabled) continue;
            renderers[i].material.color = Color.Lerp(flashColor * 5, _colors[i], _time / flashSeconds);
        }

        if (_time >= flashSeconds)
        {
            _isFlashing = false;
            ResetFlashTimer();
        }
    }
}
