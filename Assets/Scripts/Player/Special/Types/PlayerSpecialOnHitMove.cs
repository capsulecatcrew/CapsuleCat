// public class PlayerSpecialOnHitMove : PlayerSpecialMove
// {
//     private float _power;
//     
//     public new void Init(PlayerSpecial playerSpecial, IPlayerSpecialUser user)
//     {
//         base.Init(playerSpecial, user);
//         Enemy.OnDamage += OnDamageApplyEffect;
//     }
//     
//     private void OnDamageApplyEffect(Damageable enemy, int damage, bool isPlayer)
//     {
//         SetEnemy(enemy);
//         ApplyEffect();
//     }
// }