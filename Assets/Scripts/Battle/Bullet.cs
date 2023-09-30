using System;
using Battle;
using Battle.Hitboxes;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage = 1;
    private float _speed = 10;
    
    private Vector3 _origin;
    private Vector3 _direction;
    private const float MaxDistance = 50;

    private Firer _firer;
    private DamageType _damageType;

    private Transform _transform;
    
    [SerializeField] private bool ignoreIFrames;
    [SerializeField] private TrailRenderer trailRenderer;
    
    public delegate void BulletHitUpdate(float damage);
    public event BulletHitUpdate OnBulletHitUpdate;

    public void OnEnable()
    {
        _transform = transform;
    }

    public void Init(float damage, float speed, Vector3 direction, Firer firer, DamageType damageType = DamageType.Normal)
    {
        _damage = Math.Clamp(damage, 1, float.MaxValue);
        _speed = speed;
        
        _origin = transform.position;
        _direction = direction.normalized;

        _firer = firer;
        _damageType = damageType;
    }

    public void InitHeavy(Vector3 direction, Firer firer)
    {
        _origin = transform.position;
        _direction = direction.normalized;
        
        _firer = firer;
    }
    
    public void HoldHeavy(Vector3 scale, Vector3 position)
    {
        _origin = position;
        _transform.position = position;
        _transform.localScale = scale;
        trailRenderer.widthMultiplier = scale.magnitude + 0.1f;
    }

    public void Fire(Vector3 direction, int damage, float speed)
    {
        _direction = direction;
        _damage = damage;
        if (_damage < 1) _damage = 1;
        _speed = speed;
    }

    public void Delete()
    {
        gameObject.SetActive(false);
    }
    
    public void Update()
    {
        transform.position += _speed * Time.deltaTime * _direction;
        
        if (Vector3.Distance(_origin, transform.position) <= MaxDistance) return;
        Delete();
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Debug.DrawLine(position, position + _direction.normalized * 10, Color.magenta);
    }

    public void OnTriggerEnter(Collider other)
    {
        var hitbox = other.gameObject.GetComponent<Hitbox>();
        if (hitbox == null) return;
        if (!hitbox.Hit(_firer, _damage, _damageType, ignoreIFrames)) return;
        Delete();
        OnBulletHitUpdate?.Invoke(_damage);
    }
}