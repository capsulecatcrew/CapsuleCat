using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    private Vector3 _direction;
    private Vector3 _normDirection;
    public float speed = 10;
    private Vector3 _maxTravelPoint;
    public string[] tagsToHit;
    
    public bool ignoreIFrames;
    private Vector3 _origin;

    // maximum distance before bullet is destroyed
    public float maxDistance = 10;

    // Tim I swear to god your Start() call was causing the weirdest fucking issue and I wanted to kms :))))))))))))))))
    
    public void Init(int damage, Vector3 direction, float speed, float maxDistance, string[] tagsToHit)
    {
        _origin = transform.position;
        
        this.damage = damage;
        _direction = direction;
        _normDirection = _direction.normalized;
        this.speed = speed;
        this.maxDistance = maxDistance;
        _maxTravelPoint = _origin + _normDirection * this.maxDistance;
        this.tagsToHit = tagsToHit;
    }

    public void Fire(Vector3 direction, int damage, float speed)
    {
        _direction = direction;
        _normDirection = _direction.normalized;
        this.damage = damage;
        this.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        // move bullet in direction of travel
        // transform.position += speed * Time.deltaTime * _normDirection;
        transform.position += speed * Time.deltaTime * _direction.normalized;

        // if bullet has reached its maximum travel point, destroy it
        if (Vector3.Distance(_origin, _maxTravelPoint) <= Vector3.Distance(_origin, transform.position))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Debug.DrawLine(position, position + _normDirection * 10, Color.magenta);
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (string t in tagsToHit)
        {
            if (other.CompareTag(t))
            {
                var damageable = other.gameObject.GetComponent(typeof(Damageable)) as Damageable;

                if (damageable != null)
                {
                    damageable.TakeDamage(damage, ignoreIFrames);
                }

                gameObject.SetActive(false);
            }
        }
    }
}