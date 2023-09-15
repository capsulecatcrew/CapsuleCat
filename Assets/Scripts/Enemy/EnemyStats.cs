using UnityEngine;

public class EnemyStats : Damageable
{
    [SerializeField] private GameObject enemy;

    [SerializeField] private EnemyHealthBar healthBar;
    private static readonly LinearStat StatMaxHealth = new ("Max Health", int.MaxValue, 50, 20, 0, 0);

    public void OnEnable()
    {
        StatMaxHealth.SetLevel(PlayerStats.GetCurrentStage());
        Health = new HealthStat(StatMaxHealth);
        GetHealthStat().OnDeath += OnDie;
        healthBar.SetStats(StatMaxHealth, Health);
    }

    public void OnDisable()
    {
        GetHealthStat().OnDeath -= OnDie;
    }
    
    public void SetEnemyColor(GameObject enemyBody)
    {
        var r = Random.Range(0.1f, 1.0f);
        var g = Random.Range(0.1f, 1.0f);
        var b = Random.Range(0.1f, 1.0f);
        enemyBody.GetComponent<Renderer>().material.color = new Color(r, g, b);
    }

    private void OnDie()
    {
        enemy.SetActive(false);
    }
}