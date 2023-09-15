// using UnityEngine;
//
// public class PlayerSpecialContinuousMove : PlayerSpecialMove
// {
//     protected bool IsActive;
//     protected float DeltaTime;
//     
//     public new void Init(PlayerSpecial playerSpecial, IPlayerSpecialUser user)
//     {
//         base.Init(playerSpecial, user);
//     }
//
//     public new void End()
//     {
//         IsActive = false;
//     }
//
//     void Update()
//     {
//         if (!IsActive) return;
//         DeltaTime += Time.deltaTime;
//         if (DeltaTime >= 1)
//         {
//             DeltaTime -= 1;
//             ApplyEffect();
//         }
//     }
// }