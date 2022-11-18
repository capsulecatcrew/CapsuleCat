using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = PlayerStats.LevelsCompleted.ToString();
    }

    public void UpdateScore()
    {
        scoreText.text = PlayerStats.LevelsCompleted.ToString();
    }
}
