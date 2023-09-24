using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;

    public string textBefore, textAfter = "";

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = textBefore + PlayerStats.GetPrevStage() + textAfter;
    }
}
