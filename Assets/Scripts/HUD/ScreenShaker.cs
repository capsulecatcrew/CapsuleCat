using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Shakes the camera when Shake is called.
/// Place this script on a normal camera.
/// </summary>
public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private float stopShakeThreshold = 0.005f;
    private Vector3 _cameraStartPos;
    private float _shakeAmount;
    private float _shakeMultiplier;
    private float _shakeTimer;
    private float _shakeBeginTime;
    private float _maxShakeTime = float.MaxValue;
    private float _maxShakeAmount = float.MaxValue;
    private float _decayRatio = 0.2f;

    /// <summary>
    /// Save original cam position to displace from
    /// </summary>
    private void Start()
    {
        _cameraStartPos = transform.localPosition;
    }

    private void Update()
    {
        UpdateShakeValues();
        
        // Shake amount too little, do not bother shaking
        if (_shakeAmount < stopShakeThreshold) return;
        // Shake time/amount exceeds parameters, end shaking
        if (_shakeTimer > _maxShakeTime) EndShake();
        
        DoShake();
    }
    
    private void UpdateShakeValues()
    {
        if (_shakeMultiplier > 0)
        {
            _shakeTimer += Time.deltaTime;
            if (_shakeTimer >= _shakeBeginTime) _shakeAmount = _shakeTimer * _shakeMultiplier;
            if (_shakeAmount > _maxShakeAmount) _shakeAmount = _maxShakeAmount;
        }
        else
        {
            _shakeAmount = Mathf.Lerp(_shakeAmount, 0, _decayRatio);
        }
    }

    private void DoShake()
    {
        var shakeX = Random.Range(-0.5f, 0.5f);
        var shakeY = Random.Range(-0.2f, 0.2f);
        var shakeZ = Random.Range(-0.5f, 0.5f);
        var shakeVector = new Vector3(shakeX, shakeY, shakeZ);
        transform.localPosition = _cameraStartPos + shakeVector * _shakeAmount;
    }

    /// <summary>
    /// Shake the screen once. Screen shake amount decays over time.
    /// </summary>
    /// <param name="shakeAmount">Amount to shake screen by.</param>
    public void Shake(float shakeAmount)
    {
        _shakeAmount = shakeAmount;
    }

    /// <summary>
    /// Shake the screen for specified amount of time.
    /// </summary>
    /// <param name="shakeAmount">Amount to shake screen by.</param>
    /// <param name="shakeTime">Amount of time to shake the screen for.</param>
    public void TimedShake(float shakeAmount, float shakeTime)
    {
        _shakeAmount = shakeAmount;
        _maxShakeTime = shakeTime;
    }
    
    /// <summary>
    /// Slowly shake the screen more and more based on specified parameters.
    /// </summary>
    /// <param name="shakeBeginTime">Time elapsed before screen shake amount before ramping.</param>
    /// <param name="shakeMultiplier">Multiplier applied to time passed to get screen shake amount.</param>
    /// <param name="maxShakeAmount">Maximum screen shake amount. Values over this are clamped.</param>
    public void ChargedShake(float shakeBeginTime, float shakeMultiplier, int maxShakeAmount)
    {
        _shakeBeginTime = shakeBeginTime;
        _shakeMultiplier = shakeMultiplier;
        _maxShakeAmount = maxShakeAmount;
    }
    
    /// <summary>
    /// Stop charging screen shake and begin Lerp decrease of shake amount.
    /// </summary>
    public void EndShake()
    {
        _shakeTimer = 0;
        _shakeMultiplier = 0;
        _shakeBeginTime = 0;
        _maxShakeTime = float.MaxValue;
        _maxShakeAmount = float.MaxValue;
    }
}
