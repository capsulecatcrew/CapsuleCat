// using UnityEngine;
//
// public class Heal : PlayerSpecialBurstMove
// {
//     private float _power = 10;
//
//     [SerializeField] private const int amount = 20; 
//     
//     public new bool Use()
//     {
//         if (!CheckCanStart()) return false;
//         Player.AddCurrentHp(amount);
//         return true;
//     }
//
//     private bool CheckCanStart()
//     {
//         return PlayerSpecial.UsePower(_power);
//     }
// }