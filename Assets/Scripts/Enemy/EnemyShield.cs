using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShield : MonoBehaviour
{
    [Header("Rotation Variables")]
    public float rotateSpeed = 20;
    public float baseRotateSpeed = 20;
    public float rotateSpeedLvlMultiplier = 1.2f;
    public float maxRotateSpeed = 100;

    [Header("Pause Variables")]
    public float minPauseTime = 3;
    public float baseMinPauseTime = 3;
    public float maxPauseTime = 10;
    public float baseMaxPauseTime = 10;
    public float pauseTimeLvlMultiplier = 0.8f;
    public float absoluteMinPauseTime = 0.5f;
    private bool _paused;
    public float minTimeBetweenPauses = 2;
    public float maxTimeBetweenPauses = 60;
    [SerializeField] private float _timeTillNextPause;
    
    private bool _clockwiseRotate;
    
    [Header("Shield Variables")]
    public GameObject[] shields;

    [Header("Shield HP")]
    public Color maxHpColor;
    public Color minHpColor;
    
    public int shieldStartingHp = 5;
    public int startingShieldCount = 3;

    private int _shieldSectors = 12;

    void Awake()
    {
        shieldStartingHp += PlayerStats.LevelsCompleted * 2;
        startingShieldCount += Random.Range(PlayerStats.LevelsCompleted / 4, PlayerStats.LevelsCompleted);
        _clockwiseRotate = Random.Range(0, 2) == 1;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateShield(shieldStartingHp, startingShieldCount);
        rotateSpeed = baseRotateSpeed;
        minPauseTime = baseMinPauseTime;
        maxPauseTime = baseMaxPauseTime;
        for (int i = 0; i < PlayerStats.LevelsCompleted; i++)
        {
            rotateSpeed *= rotateSpeedLvlMultiplier;
            minPauseTime *= pauseTimeLvlMultiplier;
            maxPauseTime *= pauseTimeLvlMultiplier;
        }
        if (rotateSpeed > maxRotateSpeed) rotateSpeed = maxRotateSpeed;
        if (minPauseTime < absoluteMinPauseTime) minPauseTime = absoluteMinPauseTime;
        if (maxPauseTime < absoluteMinPauseTime) maxPauseTime = absoluteMinPauseTime;
        _timeTillNextPause = Random.Range(minTimeBetweenPauses, maxTimeBetweenPauses);
    }

    private void OnEnable()
    {
        foreach (GameObject shield in shields)
        {
            Damageable damageable = shield.GetComponent<Damageable>();
            damageable.OnDamage += UpdateShieldColor;
            damageable.OnDeath += DisableDeadShield;
        }
    }
    private void OnDisable()
    {
        foreach (GameObject shield in shields)
        {
            Damageable damageable = shield.GetComponent<Damageable>();
            damageable.OnDamage += UpdateShieldColor;
            damageable.OnDeath -= DisableDeadShield;
        }
    }

    void DisableDeadShield(Damageable shield)
    {
        shield.gameObject.SetActive(false);
    }

    void UpdateShieldColor(Damageable shield)
    {
        shield.gameObject.GetComponent<Renderer>().material.color =
            Color.Lerp(minHpColor, maxHpColor, (float)shield.currentHp / shield.maxHp);
    }
    
    public void GenerateShield(int hp, int numOfShields)
    {

        // set HP of shields
        foreach (GameObject shield in shields)
        {
            Damageable damageable = shield.GetComponent<Damageable>();
            damageable.maxHp = hp;
            damageable.currentHp = hp;
            shield.SetActive(false);
        }
        
        if (numOfShields >= 12)
        {
            foreach (GameObject shield in shields)
            {
                shield.SetActive(true);
            }

            return;
        }
        
        ArrayList possibleShieldGroupNums = new ArrayList { 1 };

        if (numOfShields % 2 == 0)
        {
            possibleShieldGroupNums.Add(2);
            if (numOfShields % 4 == 0)
            {
                possibleShieldGroupNums.Add(4);
            }
        }
        if (numOfShields % 3 == 0) possibleShieldGroupNums.Add(3);
        
        int shieldGroups = (int) possibleShieldGroupNums[Random.Range(0, possibleShieldGroupNums.Count)];
        
        int shieldsPerGroup = numOfShields / shieldGroups;
        
        int maxShieldsPerGroup = _shieldSectors / shieldGroups;
        
        for (int i = 0; i < _shieldSectors; i += maxShieldsPerGroup)
        {
            for (int j = 0; j < shieldsPerGroup; j++)
            {
                GameObject shield = shields[i + j];
                shield.SetActive(true);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (_paused) return;
        
        if (_clockwiseRotate)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }

        _timeTillNextPause -= Time.deltaTime;
        
        if (_timeTillNextPause < float.Epsilon)
        {
            StartCoroutine(MomentaryPause());
        }
    }

    IEnumerator MomentaryPause()
    {
        // Pause rotation
        _paused = true;
        // 66.7% chance to change rotation direction
        if (Random.Range(0, 3) > 0) ChangeRotateDirection();
        // Wait for some seconds
        yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));
        // Unpause
        _timeTillNextPause = Random.Range(minTimeBetweenPauses, maxTimeBetweenPauses);
        _paused = false;
    }
    
    void ChangeRotateDirection()
    {
        _clockwiseRotate = !_clockwiseRotate;
    }
}
