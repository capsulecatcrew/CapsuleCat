using UnityEngine;

namespace HUD.ProgressBars
{
    public class EnergyBar : ProgressBar
    {
        [SerializeField] [Range(1, 2)] private int playerNum;
        
        public EnergyBar()
        {
            var statMaxEnergy = PlayerStats.GetMaxEnergyStat(playerNum);
            var statEnergy = PlayerStats.GetEnergyStat(playerNum);
            statMaxEnergy.OnStatUpdate += SetMaxValue;
            SetMaxValue(0, statMaxEnergy.GetValue(), 0);
            statEnergy.OnUseUpdate += MinusValue;
            statEnergy.OnAbsorbUpdate += AddValue;
            SetValue(statMaxEnergy.GetValue());
        }
    }
}