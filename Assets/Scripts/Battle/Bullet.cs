using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    private float _damage = 1;
    private Vector3 _direction;
    private float _speed = 10;
    private Vector3 _maxTravelPoint;
    public string[] tagsToHit;
    
    private Vector3 _origin;
    [SerializeField] private bool ignoreIFrames;
    
    [SerializeField] private TrailRenderer trailRenderer;

    public delegate void BulletHitUpdate(GameObject hitObject, float damage, bool ignoreIFrames);
    
    public event BulletHitUpdate OnBulletHitUpdate;

    // maximum distance before bullet is destroyed
    public float maxDistance = 10;

    void Start()
    {
        _origin = transform.position;
    }

    public void OnEnable()
    {
        var controller = GameObject.FindGameObjectWithTag("GameController");
        var battleManager = controller.GetComponent<BattleManager>();
        battleManager.RegisterBullet(this);
    }
    
    void OnDisable()
    {
        var controller = GameObject.FindGameObjectWithTag("GameController");
        var battleManager = controller.GetComponent<BattleManager>();
        battleManager.DeregisterBullet(this);
    }

    public void Init(float damage, Vector3 direction, float spd, float maxDist, string[] tags)
    {
        _origin = transform.position;
        _damage = damage;
        if (_damage < 1) _damage = 1;
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
        if (_damage < 1) _damage = 1;
        _speed = speed;
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
        foreach (var tag in tagsToHit)
        {
            if (!other.CompareTag(tag)) continue;
            OnBulletHitUpdate?.Invoke(other.gameObject, _damage, ignoreIFrames);
            Delete();
        }
    }
}