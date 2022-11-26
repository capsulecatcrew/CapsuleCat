using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingGlow : MonoBehaviour
{
    public List<DamageAbsorber> absorbers;
    public List<Renderer> glowingParts;

    private List<Material> _materials;
    
    public Color glowColor;

    public float timeTillFade = 2.0f;
    public float glowFadeTime = 1.0f;

    private float _timer = 0.0f;
    private float _fadeTimer = 0.0f;

    private enum GlowState
    {
        On,
        Fade,
        Off
    }

    private GlowState _glowState;
    
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    // Start is called before the first frame update
    void Start()
    {
        _glowState = GlowState.Off;

        _materials = new List<Material>();
        foreach (var part in glowingParts)
        {
            var mat = part.material;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor(EmissionColor, Color.black);
            _materials.Add(mat);
        }
    }

    private void OnEnable()
    {
        foreach (var absorber in absorbers)
        {
            absorber.OnDamageAbsorb += TurnOnGlow;
        }
    }
    private void OnDisable()
    {
        foreach (var absorber in absorbers)
        {
            absorber.OnDamageAbsorb -= TurnOnGlow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_glowState)
        {
            case GlowState.On:
                _timer += Time.deltaTime;
                if (_timer >= timeTillFade) _glowState = GlowState.Fade;
                break;
            case GlowState.Fade:
                FadeGlow();
                _fadeTimer += Time.deltaTime;
                if (_fadeTimer >= glowFadeTime)
                {
                    _glowState = GlowState.Off;
                    TurnOffGlow();
                }
                break;
            case GlowState.Off:
                break;
        }
    }

    void TurnOnGlow(float unused)
    {
        if (_glowState != GlowState.On)
        {
            foreach (var mat in _materials)
            {
                mat.SetColor(EmissionColor, glowColor);
            }

            _glowState = GlowState.On;
        }
        
        _timer = 0.0f;
        _fadeTimer = 0.0f;
    }

    void FadeGlow()
    {
        foreach (var mat in _materials)
        {
            mat.SetColor(EmissionColor, Color.Lerp(glowColor, Color.black, Math.Min(1.0f, _fadeTimer / glowFadeTime)));
        }
    }

    void TurnOffGlow()
    {
        foreach (var mat in _materials)
        {
            mat.SetColor(EmissionColor, Color.black);
        }

    }
}
