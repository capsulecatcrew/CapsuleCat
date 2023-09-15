using UnityEngine;

public class EnergyStat
{
    private float _energy;
    private readonly LinearStat _statMaxEnergy;
    private readonly LinearStat _statEnergyAbsorb;

    public delegate void EnergyUpdate(float value);

    public event EnergyUpdate OnAbsorbUpdate;
    public event EnergyUpdate OnUseUpdate;

    public EnergyStat(LinearStat statMaxEnergy, LinearStat statEnergyAbsorb)
    {
        _energy = statMaxEnergy.GetValue();
        _statMaxEnergy = statMaxEnergy;
        _statEnergyAbsorb = statEnergyAbsorb;
    }
    public void Reset(bool isLoss)
    {
        if (isLoss)
        {
            _statMaxEnergy.Reset();
            _statEnergyAbsorb.Reset();
        }
        _energy = _statMaxEnergy.GetValue();
    }

    public bool CanUse(float amount)
    {
        return _energy >= amount;
    }

    public void Use(float amount)
    {
        if (amount > _energy) return;
        _energy -= amount;
        OnUseUpdate?.Invoke(amount);
    }

    public void Absorb(float amount)
    {
        float absorbAmount = amount * _statEnergyAbsorb.GetValue();
        if (float.MaxValue - absorbAmount < _energy) return;
        var newEnergy = _energy + absorbAmount;
        var oldEnergy = _energy;
        if (!_statMaxEnergy.IsWithinBounds(newEnergy))
        {
            newEnergy = _statMaxEnergy.GetValue();
        }
        _energy = newEnergy;
        OnAbsorbUpdate?.Invoke(_energy - oldEnergy);
    }
}