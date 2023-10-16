using System;
using Player.Stats;
using Scriptable_Objects;
using UnityEngine;

namespace Shop
{
    public class StatsPageController: MonoBehaviour
    {
        [SerializeField] private StatUpgradeSpriteScriptableObject statSprites;
        [SerializeField] private SpecialMoveSpriteScriptableObject specialMoveSprites;
        
        [SerializeField] private ShopItemButton p1Special, p2Special;
        [SerializeField] private ShopItemButton[] p1Stats, p2Stats;
        [SerializeField] private ShopItemButton maxHealthStat, energyShareStat;

        private void OnEnable()
        {
            var p1SpecialMove = PlayerStats.GetPlayerSpecialMove(1);
            p1Special.Init(new SpecialMoveStatsPageItem(p1SpecialMove), specialMoveSprites.GetSprite(p1SpecialMove));
            var p2SpecialMove = PlayerStats.GetPlayerSpecialMove(2);
            p2Special.Init(new SpecialMoveStatsPageItem(p2SpecialMove), specialMoveSprites.GetSprite(p2SpecialMove));

            var stat = PlayerStats.GetMaxEnergyStat(1);
            p1Stats[0].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetEnergyAbsorbStat(1);
            p1Stats[1].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetSpecialGainStat(1);
            p1Stats[2].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetAttackDamageStat(1);
            p1Stats[3].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetDashEnergyCostStat(1);
            p1Stats[4].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));

            stat = PlayerStats.GetMaxEnergyStat(2);
            p2Stats[0].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetEnergyAbsorbStat(2);
            p2Stats[1].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetSpecialGainStat(2);
            p2Stats[2].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetAttackDamageStat(2);
            p2Stats[3].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            stat = PlayerStats.GetDashEnergyCostStat(2);
            p2Stats[4].Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));

            stat = PlayerStats.GetMaxHealthStat();
            maxHealthStat.Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
            // TODO: update when energy share is changed to a shared stat
            stat = PlayerStats.GetEnergyShareStat(1);
            energyShareStat.Init(new StatInfoStatsPageItem(stat), statSprites.GetSprite(stat));
        }
    }
}