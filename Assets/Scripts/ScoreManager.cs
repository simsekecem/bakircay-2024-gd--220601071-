using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Skoru gösterecek TextMeshPro öðesi
    private int currentScore = 0;

    // Skoru güncelleme metodu
    public void UpdateScore(int amount)
    {
        currentScore += amount;
        scoreText.text = $"Score: {currentScore}";
    }

    // Skoru sýfýrlama metodu
    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = $"Score: {currentScore}";
    }
}

