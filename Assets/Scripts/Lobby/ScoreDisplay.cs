using Player.Stats;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private string textBefore, textAfter = "";
    
    public void Start()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = textBefore + PlayerStats.GetPrevStage() + textAfter;
    }
}
