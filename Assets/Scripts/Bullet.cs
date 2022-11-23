using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;

    public string[] tagsToHit;
    
    public float travelSpeed;

    public bool ignoreIFrames = false;

    private Vector3 _direction = Vector3.forward;
    private Vector3 _normDirection;

    private Vector3 _origin;

    private Vector3 _maxTravelPoint;
    // maximum distance before bullet is destroyed
    public float maxDistance = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.position;
        _normDirection = Vector3.Normalize(_direction);
        _maxTravelPoint = _origin + _normDirection * maxDistance;
    }

    public void Init(int damage, Vector3 direction, float travelSpeed, float maxDistance, string[] tagsToHit)
    {
        _origin = transform.position;

        this.damage = damage;
        
        this._direction = direction;
        
        // recalculate normalized direction
        _normDirection = Vector3.Normalize(this._direction);
        
        this.travelSpeed = travelSpeed;
        
        this.maxDistance = maxDistance;
        
        // recalculate max travel point
        _maxTravelPoint = _origin + _normDirection * this.maxDistance;

        this.tagsToHit = tagsToHit;
    }
    
    // Update is called once per frame
    void Update()
    {
        // move bullet in direction of travel
        transform.position += travelSpeed * Time.deltaTime * _normDirection;
        
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
