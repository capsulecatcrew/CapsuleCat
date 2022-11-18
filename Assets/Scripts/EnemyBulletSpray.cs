using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpray : MonoBehaviour
{
    private static float PI = 3.1415926535f;
    
    public Transform origin;

    public ObjectPool bulletPool;
    private GameObject _bullet;

    public AudioClip firingSound;

    public float bulletDespawnDist = 50;

    public float startingMinTimeBetweenShots = 3;
    private float _minTimeBetweenShots;
    public float startingMaxTimeBetweenShots = 10;
    private float _maxTimeBetweenShots;
    public float absoluteMinTimeBetweenShots = 0.5f;
    public float timerDecreaseByLevel = 0.5f;
    
    public int startingDamage = 1;
    private int _damage;
    public int dmgIncreaseLvlInterval = 5;
    public int dmgIncrease = 1;
    
    public string[] tagsToHit;

    public int[] bulletAmounts;
    private int _amountsCount;
    public float[] bulletHeights;
    private int _heightsCount;
    
    public float bulletSpeed;

    private float _timeSinceLastAttack;

    private float _timeTillNextAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        if (origin == null)
        {
            origin = transform;
        }

        _damage = startingDamage + dmgIncrease * PlayerStats.LevelsCompleted / dmgIncreaseLvlInterval;
        _timeSinceLastAttack = 0;
        _minTimeBetweenShots = startingMinTimeBetweenShots - timerDecreaseByLevel * PlayerStats.LevelsCompleted;
        if (_minTimeBetweenShots < absoluteMinTimeBetweenShots) _minTimeBetweenShots = absoluteMinTimeBetweenShots;
        _maxTimeBetweenShots = startingMaxTimeBetweenShots - timerDecreaseByLevel * PlayerStats.LevelsCompleted;
        if (_maxTimeBetweenShots < absoluteMinTimeBetweenShots) _maxTimeBetweenShots = absoluteMinTimeBetweenShots;

        _timeTillNextAttack = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
        _heightsCount = bulletHeights.Length;
        _amountsCount = bulletAmounts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;
        if (_timeSinceLastAttack > _timeTillNextAttack)
        {
            // Attack
            float height = bulletHeights[Random.Range((int)0, _heightsCount)];
            float amount = bulletAmounts[Random.Range(0, _amountsCount)];
            float theta = 2 * PI / amount;
            // random starting angle so the "empty spots"
            // without bullets aren't the same when the bullet count is the same
            float startingAngle = Random.Range(0.0f, theta);
            Vector3 position = origin.position;
            position = new Vector3(position.x, height, position.z);
            for (int i = 0; i < amount; i++)
            {
                Vector3 dir = new Vector3(Mathf.Sin(startingAngle + theta * i), 0, Mathf.Cos(startingAngle + theta * i));
                _bullet = bulletPool.GetPooledObject();
                _bullet.transform.position = position;
                _bullet.GetComponent<Bullet>().Init(_damage, dir, bulletSpeed, bulletDespawnDist, tagsToHit);
                _bullet.SetActive(true);

            }
            GlobalAudio.AudioSource.PlayOneShot(firingSound);
            _timeTillNextAttack = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
            _timeSinceLastAttack = 0.0f;
        }
    }
}
