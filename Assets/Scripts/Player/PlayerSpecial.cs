using System;
using UnityEngine;

public class PlayerSpecial : MonoBehaviour
{
    [SerializeField] private float maxPower = 50;
    private float _power;

    private float _usageMultiplier;
    private IPlayerSpecialUser _specialUser;

    [SerializeField] private float damageMultiplier = 0.5f;
    [SerializeField] private float absorbMultiplier = 0.5f;
    [SerializeField] private float damagedMultiplier = 3.0f;

    public void GainDamagePower(float damageAmount)
    {
        _power += damageMultiplier * damageAmount;
        ClampMaxPower();
    }

    public void GainAbsorbPower(float absorbAmount)
    {
        _power += absorbMultiplier * absorbAmount;
        ClampMaxPower();
    }

    public void GainDamagedPower(float damagedAmount)
    {
        _power += damagedMultiplier * damagedAmount;
        ClampMaxPower();
    }

    private void ClampMaxPower()
    {
        _power = Math.Clamp(_power, 0, maxPower);
    }

    /// <summary>
    /// Use special power.
    /// Returns false if insufficient power to use.
    /// </summary>
    /// <param name="amount">Amount of special power to use.</param>
    /// <returns>Whether usage of special power is successful.</returns>
    public bool UsePower(float amount)
    {
        if (amount > _power) return false;
        _power -= amount;
        return true;
    }

    /// <summary>
    /// Use special power with continuous power drain.
    /// </summary>
    /// <param name="multiplier">Multiplier applied to deltaTime for power consumption.</param>
    /// <param name="user">User to return to when player runs out of special meter.</param>
    public void UseContinuousPower(float multiplier, IPlayerSpecialUser user)
    {
        if (_power <= 0) return;
        _usageMultiplier = multiplier;
        _specialUser = user;
    }

    private void UpdateContinuousPower(float deltaTime)
    {
        if (_usageMultiplier == 0) return;
        _power -= deltaTime + _usageMultiplier;
        if (_power > 0) return;
        EndContinuousPower();
    }

    private void EndContinuousPower()
    {
        _usageMultiplier = 0;
        _specialUser.OnPowerDepleted();
    }

    void Update()
    {
        UpdateContinuousPower(Time.deltaTime);
    }
}