using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;
    private int scoreMultiplier = 1;
    private bool isDoubleScoreActive = false;

    public void UpdateScore(int amount)
    {
        currentScore += amount * scoreMultiplier;
        scoreText.text = $"Score: {currentScore}";
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreMultiplier = 1;
        scoreText.text = $"Score: {currentScore}";
    }

    public void SetScoreMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
    }

    public void ActivateDoubleScore()
    {
        if (!isDoubleScoreActive)
        {
            isDoubleScoreActive = true;
            SetScoreMultiplier(2);
        }
    }

    public void DeactivateDoubleScore()
    {
        if (isDoubleScoreActive)
        {
            isDoubleScoreActive = false;
            SetScoreMultiplier(1);
        }
    }
}
