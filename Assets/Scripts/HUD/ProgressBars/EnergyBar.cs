namespace HUD.ProgressBars
{
    public class EnergyBar : ProgressBar
    {
        public EnergyBar(LinearStat statMaxEnergy, EnergyStat energyStat)
        {
            statMaxEnergy.OnStatUpdate += SetMaxValue;
            energyStat.OnUseUpdate += MinusValue;
            energyStat.OnAbsorbUpdate += AddValue;
        }
    }
}