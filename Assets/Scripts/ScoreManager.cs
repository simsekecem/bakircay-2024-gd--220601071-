using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Skoru g�sterecek TextMeshPro ��esi
    private int currentScore = 0;

    // Skoru g�ncelleme metodu
    public void UpdateScore(int amount)
    {
        currentScore += amount;
        scoreText.text = $"Score: {currentScore}";
    }

    // Skoru s�f�rlama metodu
    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = $"Score: {currentScore}";
    }
}

