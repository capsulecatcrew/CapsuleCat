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
    public void Awake()
    {
        _hitbox = GetComponent<ShieldKillable>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void OnEnable()
    {
        _hitbox.OnHitBox += StartAnimation;
    }

    public void OnDisable()
    {
        _hitbox.OnHitBox -= StartAnimation;
    }

    private void StartAnimation(float unused1, DamageType unused2)
    {
        _animator.SetTrigger(Shake);
    }
    
}
