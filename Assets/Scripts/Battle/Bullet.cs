using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _damage = 1;
    private Vector3 _direction;
    private float _speed = 10;
    private Vector3 _maxTravelPoint;
    public string[] tagsToHit;

    public bool ignoreIFrames;
    private Vector3 _origin;
    
    [SerializeField] private TrailRenderer trailRenderer;

    // maximum distance before bullet is destroyed
    public float maxDistance = 10;

    void Start()
    {
        _origin = transform.position;
    }

    public void Init(float damage, Vector3 direction, float spd, float maxDist, string[] tags)
    {
        _origin = transform.position;
        _damage = damage;
        _direction = direction;
        _speed = spd;
        maxDistance = maxDist;
        _maxTravelPoint = _origin + _direction.normalized * maxDistance;
        tagsToHit = tags;
    }
    public void Hold(Vector3 scale, Vector3 position)
    {
        transform.localScale = scale;
        trailRenderer.widthMultiplier = scale.magnitude + 0.1f;
        transform.position = position;
        _origin = position;
    }

    public void Fire(Vector3 direction, int damage, float speed)
    {
        _direction = direction;
        _damage = damage;
        this._speed = speed;
    }

    public void Delete()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Time.deltaTime * _direction.normalized;

        // if bullet has reached its maximum travel point, destroy it
        if (Vector3.Distance(_origin, _maxTravelPoint) <= Vector3.Distance(_origin, transform.position))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Debug.DrawLine(position, position + _direction.normalized * 10, Color.magenta);
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (var t in tagsToHit)
        {
            if (!other.CompareTag(t)) continue;
            if (other.gameObject.TryGetComponent<Damageable>(out var damageable))
            {
                damageable.TakeDamage(_damage, ignoreIFrames);
            }
            Delete();
        }
    }
}