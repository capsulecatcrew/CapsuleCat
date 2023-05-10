using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;

    public string textBefore, textAfter = "";
    public int offset = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = textBefore + (PlayerStats.LevelsCompleted + offset).ToString() + textAfter;
    }
}
