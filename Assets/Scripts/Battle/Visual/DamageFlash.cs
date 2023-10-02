using System.Collections.Generic;
using Battle;
using Battle.Hitboxes;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Hitbox[] hitboxes;
    [SerializeField] private float flashTime = 0.2f;
    [SerializeField] private Color flashColor;

    [SerializeField] private Renderer[] renderers;
    private readonly Dictionary<Renderer, Color> _originalColors = new ();
    private float _flashTimer;
    
    public void OnEnable()
    {
        foreach (var hitbox in hitboxes)
        {
            hitbox.OnHitBox += StartFlashAnim;
        }
    }
    public void OnDisable()
    {
        foreach (var hitbox in hitboxes)
        {
            hitbox.OnHitBox -= StartFlashAnim;
        }
    }

    private void StartFlashAnim(float dmg, DamageType unused)
    {
        if (dmg <= 0) return;
        if (!IsFlashing()) StoreOriginalColors();
        ResetFlashTimer();
    }
    
    private void ResetFlashTimer()
    {
        _flashTimer = flashTime;
    }
    
    private void StoreOriginalColors()
    {
        foreach (var render in renderers)
        {
            _originalColors[render] = render.material.color;
        }
    }

    public void Update()
    {
        if (!IsFlashing()) return;
        UpdateFlashTimer();
        LerpFlashValue();
    }

    private void UpdateFlashTimer()
    {
        _flashTimer -= Time.deltaTime;
    }

    private void LerpFlashValue()
    {
        foreach (var render in renderers)
        {
            if (!render.enabled) continue;
            render.material.color = Color.Lerp(_originalColors[render], flashColor * 5, _flashTimer / flashTime);
        }
    }
    
    private bool IsFlashing()
    {
        return _flashTimer > 0;
    }
}
