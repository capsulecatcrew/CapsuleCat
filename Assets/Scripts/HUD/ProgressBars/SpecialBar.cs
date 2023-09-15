using UnityEngine;

namespace HUD.ProgressBars
{
    public class SpecialBar : ProgressBar
    {
        [SerializeField] [Range(1, 2)] private int playerNum;
        
        public SpecialBar()
        {
            var statSpecial = PlayerStats.GetSpecialStat(playerNum);
            SetMaxValue(0, PlayerStats.GetMaxSpecial(playerNum), 0);
            statSpecial.OnUseUpdate += MinusValue;
            statSpecial.OnGainUpdate += AddValue;
            SetValue(0);
        }
    }
}