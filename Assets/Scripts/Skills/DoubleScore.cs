using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoublePoint : MonoBehaviour
{
    public ScoreManager scoreManager;
    public SpawnFoods spawnFoods;
    public Button doublePointButton;
    public Color disabledColor = Color.red; // �lk disable rengi
    public Color respawnedDisabledColor = Color.grey; // Respawn sonras� disable rengi
    private Color originalColor;

    private void Start()
    {
        if (doublePointButton != null)
        {
            originalColor = doublePointButton.image.color;

            if (spawnFoods != null)
            {
                spawnFoods.scoreManager = scoreManager;
                spawnFoods.OnObjectsSpawned += HandleRespawnedObjects;
            }
        }
    }

    public void ActivateDoubleScore()
    {
        if (scoreManager != null)
        {
            scoreManager.ActivateDoubleScore();
        }
        else
        {
            Debug.LogError("ScoreManager referans� atanmad�.");
        }

        if (doublePointButton != null)
        {
            doublePointButton.interactable = false;
            doublePointButton.image.color = disabledColor; // �lk disable rengine ayarla
        }
    }

    private void HandleRespawnedObjects()
    {
        if (doublePointButton != null)
        {
            doublePointButton.image.color = respawnedDisabledColor; // Respawn sonras� disable rengine ayarla
        }

        StartCoroutine(ReactivateButtonAfterDelay());
    }

    private IEnumerator ReactivateButtonAfterDelay()
    {
        yield return new WaitForSeconds(5f); // 5 saniye bekle
        if (doublePointButton != null)
        {
            doublePointButton.interactable = true;
            doublePointButton.image.color = originalColor; // Orijinal renge d�nd�r
        }
    }

    private void OnDestroy()
    {
        if (spawnFoods != null)
        {
            spawnFoods.OnObjectsSpawned -= HandleRespawnedObjects;
        }
    }
}
