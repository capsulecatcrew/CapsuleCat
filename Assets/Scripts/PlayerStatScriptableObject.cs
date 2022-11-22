using UnityEngine;

[CreateAssetMenu(menuName = "Player Stat")]
public class PlayerStatScriptableObject:ScriptableObject
{
    public float baseStat;
    public float statIncrementPerLevel;
    public float baseUpgradeCost;
    public float costIncrementPerLevel;

}
