using System;
using System.Collections.Generic;
using UnityEngine;

public class WingGlow : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] private int playerNum;
    public List<Renderer> glowingParts;

    private List<Material> _materials;
    
    public Color glowColor;

    public float timeTillFade = 2.0f;
    public float glowFadeTime = 1.0f;

    private float _timer;
    private float _fadeTimer;

    private enum GlowState
    {
        On,
        Fade,
        Off
    }

    private GlowState _glowState;
    
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void OnEnable()
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
    
    public void Update()
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
            default:
                return;
        }
    }

    public void TurnOnGlow(float unused = 0)
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

    private void FadeGlow()
    {
        foreach (var mat in _materials)
        {
            mat.SetColor(EmissionColor, Color.Lerp(glowColor, Color.black, Math.Min(1.0f, _fadeTimer / glowFadeTime)));
        }
    }

    private void TurnOffGlow()
    {
        foreach (var mat in _materials)
        {
            mat.SetColor(EmissionColor, Color.black);
        }

    }
}
