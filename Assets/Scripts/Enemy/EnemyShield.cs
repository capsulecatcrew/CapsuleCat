using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private Color maxHpColor;
    [SerializeField] private Color minHpColor;

    [SerializeField] private EnemyShieldController enemyShieldController;

    private HealthStat _statHealth;
    [SerializeField] private Damageable damageable;
    private Renderer _renderer;

    private void OnEnable()
    {
        _statHealth = new HealthStat(enemyShieldController.GetMaxHealthStat());
        _renderer = gameObject.GetComponent<Renderer>();
        _statHealth.Reset();
        damageable.SetHealthStat(_statHealth);
        damageable.GetHealthStat().OnDamageUpdate += UpdateShieldColor;
        damageable.GetHealthStat().OnDeath += DisableDeadShield;
    }
    private void OnDisable()
    {
        damageable.GetHealthStat().OnDamageUpdate -= UpdateShieldColor;
        damageable.GetHealthStat().OnDeath -= DisableDeadShield;
    }

    void UpdateShieldColor(float damage)
    {
        _renderer.material.color =
            Color.Lerp(minHpColor, maxHpColor, damageable.GetHealthStat().GetHealthPercentage());
    }
    
    void DisableDeadShield()
    {
        gameObject.SetActive(false);
    }
}
