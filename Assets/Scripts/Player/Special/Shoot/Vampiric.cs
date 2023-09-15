// using UnityEngine;
//
// public class Vampiric : PlayerSpecialOnHitMove
// {
//     [SerializeField] private const int Multiplier = 3;
//     [SerializeField] private const int MinPower = 15;
//     private bool _isActive;
//
//     public new bool Use()
//     {
//         if (!CheckCanStart()) return false;
//         _isActive = true;
//         return true;
//     }
//     
//     private bool CheckCanStart()
//     {
//         return PlayerSpecial.UseContinuousMinPower(Multiplier, MinPower, User);
//     }
//
//     public new void ApplyEffect()
//     {
//         if (!_isActive) return;
//         Player.AddCurrentHp(Multiplier);
//     }
// }