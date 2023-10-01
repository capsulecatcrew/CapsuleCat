using Battle;
using Battle.Hitboxes;
using UnityEngine;

[RequireComponent(typeof(ShieldKillable))]
[RequireComponent(typeof(Animator))]
public class EnemyShieldAnimation : MonoBehaviour
{
    private ShieldKillable _hitbox;

    private Animator _animator;

    private static readonly int Shake = Animator.StringToHash("Shake");

    // Start is called before the first frame update
    void Awake()
    {
        _hitbox = GetComponent<ShieldKillable>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        _hitbox.OnHitBox += TriggerHitAnim;
    }

    private void OnDisable()
    {
        _hitbox.OnHitBox -= TriggerHitAnim;
    }

    private void TriggerHitAnim(float unused1, DamageType unused2)
    {
        _animator.SetTrigger(Shake);
    }
    
}
