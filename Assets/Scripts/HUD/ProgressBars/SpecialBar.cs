namespace HUD.ProgressBars
{
    public class SpecialBar : ProgressBar
    {
        public SpecialBar(SpecialStat specialStat)
        {
            SetMaxValue(0, specialStat.GetMaxValue(), 0);
            specialStat.OnUseUpdate += MinusValue;
            specialStat.OnGainUpdate += AddValue;
        }
    }
}