using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 1;
    private Vector3 _direction;
    public float speed = 10;
    private Vector3 _maxTravelPoint;
    public string[] tagsToHit;

    public bool ignoreIFrames;
    private Vector3 _origin;

    private PlayerSpecial playerSpecial;
    private bool isPlayerBullet;
    [SerializeField] private TrailRenderer trailRenderer;

    // maximum distance before bullet is destroyed
    public float maxDistance = 10;

    void Start()
    {
        _origin = transform.position;
    }

    public void Init(int dmg, Vector3 dir, float spd, float maxDist, string[] tags)
    {
        _origin = transform.position;

        damage = dmg;
        _direction = dir;
        speed = spd;
        maxDistance = maxDist;
        _maxTravelPoint = _origin + _direction.normalized * maxDistance;
        tagsToHit = tags;
    }

    public void Init(int dmg, Vector3 dir, float spd, float maxDist, string[] tags, PlayerSpecial playerSpec)
    {
        Init(dmg, dir, spd, maxDist, tags);
        isPlayerBullet = true;
        playerSpecial = playerSpec;
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
        this.damage = damage;
        this.speed = speed;
    }

    public void Delete()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * _direction.normalized;

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
                damageable.TakeDamage(damage, ignoreIFrames);
                if (isPlayerBullet) playerSpecial.GainDamagePower(damage);
            }

            Delete();
        }
    }
}