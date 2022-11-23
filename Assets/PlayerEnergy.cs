using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    private bool _isMax;
    private bool _isEmpty;
    [SerializeField] float maxAmount;
    [SerializeField] float currentAmount;
    
    public List<DamageAbsorber> energyAbsorbers;
    
    public delegate void NoParams();

    public event NoParams EnergyUpdate;
    
    private void Start()
    {
        maxAmount = PlayerStats.Energy.GetMaxValue();
        currentAmount = PlayerStats.Energy.GetCurrentValue();
        foreach (var absorber in energyAbsorbers)
        {
            absorber.absorbMultiplier = PlayerStats.EnergyAbsorb.GetCurrentValue();
        }

        _isMax = Math.Abs(maxAmount - currentAmount) < float.Epsilon;
    }

    private void OnEnable()
    {
        foreach (var absorber in energyAbsorbers)
        {
            absorber.OnDamageAbsorb += AddAmount;
        }
    }

    private void OnDisable()
    {
        foreach (var absorber in energyAbsorbers)
        {
            absorber.OnDamageAbsorb -= AddAmount;
        }
    }

    
    public void AddAmount(float amount)
    {
        currentAmount += amount;
        if (currentAmount >= maxAmount)
        {
            currentAmount = maxAmount;
            _isMax = true;
            _isEmpty = false;
        } else if (currentAmount <= 0.0f)
        {
            currentAmount = 0.0f;
            _isMax = false;
            _isEmpty = true;
        }
        else
        {
            _isMax = false;
            _isEmpty = false;
        }
        
        EnergyUpdate?.Invoke();
    }
    
    public void RefillToMax()
    {
        currentAmount = 0;
        AddAmount(maxAmount);
    }

    public void SetMax(float value)
    {
        maxAmount = value;
    }

    public void SetAmount(float value)
    {
        currentAmount = 0;
        AddAmount(value);
    }

    public float GetMax()
    {
        return maxAmount;
    }

    public float GetCurrentAmount()
    {
        return currentAmount;
    }
    
    public bool IsMax()
    {
        return _isMax;
    }

    public bool IsEmpty()
    {
        return _isEmpty;
    }
}
