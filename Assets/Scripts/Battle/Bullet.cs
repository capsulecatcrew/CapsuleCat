using System;
using Battle;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage = 1;
    private float _speed = 10;
    
    private Vector3 _origin;
    private Vector3 _direction;
    private const float MaxDistance = 50;

    private Firer _firer;

    private Transform _transform;
    
    [SerializeField] private bool ignoreIFrames;
    [SerializeField] private TrailRenderer trailRenderer;

    private BattleManager _battleManager;

    private bool _isPlayer;

    public void Awake()
    {
        var gameController = GameObject.FindGameObjectWithTag("GameController");
        _battleManager = gameController.GetComponent<BattleManager>();
    }

    public void OnEnable()
    {
        _transform = transform;
    }

    public void Init(float damage, float speed, Vector3 direction, Firer firer)
    {
        _damage = Math.Clamp(damage, 1, float.MaxValue);
        _speed = speed;
        
        _origin = transform.position;
        _direction = direction.normalized;

        _firer = firer;
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

    // Update is called once per frame
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
        if (_battleManager.HitTarget(_firer, other.gameObject, _damage, ignoreIFrames)) Delete();
    }
}