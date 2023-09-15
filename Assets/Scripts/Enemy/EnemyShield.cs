using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField] private Color maxHpColor;
    [SerializeField] private Color minHpColor;

    private HealthStat _statHealth;
    [SerializeField] private Damageable damageable;
    private Renderer _renderer;

    void Awake()
    {
        _statHealth = new HealthStat(EnemyShieldController.GetMaxHealthStat());
        _renderer = gameObject.GetComponent<Renderer>();
    }
    
    private void OnEnable()
    {
        _statHealth.Reset();
        damageable.SetHealthSet(_statHealth);
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
