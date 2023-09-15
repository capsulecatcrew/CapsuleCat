using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;

    public string textBefore, textAfter = "";
    private const int Offset = -1;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = textBefore + (PlayerStats.GetCurrentStage() + Offset) + textAfter;
    }
}
