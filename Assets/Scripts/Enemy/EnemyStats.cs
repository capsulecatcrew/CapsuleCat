using HUD.ProgressBars;
using UnityEngine;

public class EnemyStats : Damageable
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyBody;
    
    [SerializeField] private HealthBar healthBar;

    public void Start()
    {
        enemyBody.GetComponent<Renderer>().material.color = GetEnemyColor();
        GetHealthStat().OnDeath += OnDie;
    }
    
    private Color GetEnemyColor()
    {
        var r = Random.Range(0.1f, 1.0f);
        var g = Random.Range(0.1f, 1.0f);
        var b = Random.Range(0.1f, 1.0f);
        return new Color(r, g, b);
    }

    private void OnDie()
    {
        enemy.SetActive(false);
    }
}