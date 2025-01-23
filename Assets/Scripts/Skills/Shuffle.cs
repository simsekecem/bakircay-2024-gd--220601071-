using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Shuffle : MonoBehaviour
{
    public GameObject lightSphere;
    public Button skillButton;
    public float rotationDuration = 3f;
    public float radius = 2f;
    public float shuffleDuration = 0.5f;

    private GameObject[] moveableObjects;

    private void Start()
    {

        if (lightSphere != null)
        {
            lightSphere.SetActive(false);
        }


        if (skillButton != null)
        {
            skillButton.onClick.AddListener(ActivateSkill);
        }
    }

    public void ActivateSkill()
    {
        skillButton.interactable = false;

        moveableObjects = GameObject.FindGameObjectsWithTag("Moveable");

        if (moveableObjects.Length == 0)
        {
            Debug.LogWarning("No Moveable objects found in the scene!");
            return;
        }

        Debug.Log($"Found {moveableObjects.Length} Moveable objects. Activating skill...");
        StartCoroutine(ShuffleObjects());
    }

    private IEnumerator ShuffleObjects()
    {
        // Karýþtýrma sesi çal
        AudioManager.Instance.PlaySound(AudioManager.Instance.shuffleSound);

        if (lightSphere != null)
        {
            lightSphere.SetActive(true);
            Debug.Log("Light sphere activated!");
        }

        float angleStep = 360f / moveableObjects.Length;

        for (int i = 0; i < moveableObjects.Length; i++)
        {
            float angle = angleStep * i;
            float radians = angle * Mathf.Deg2Rad;

            Vector3 targetPosition = lightSphere.transform.position + new Vector3(
                Mathf.Cos(radians) * radius,
                0f,
                Mathf.Sin(radians) * radius
            );

            moveableObjects[i].transform.DOMove(targetPosition, 1f).SetEase(Ease.InOutQuad);
        }

        yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = elapsedTime / rotationDuration * 360f;

            for (int i = 0; i < moveableObjects.Length; i++)
            {
                float angle = currentAngle + (angleStep * i);
                float radians = angle * Mathf.Deg2Rad;

                Vector3 newPosition = lightSphere.transform.position + new Vector3(
                    Mathf.Cos(radians) * radius,
                    0f,
                    Mathf.Sin(radians) * radius
                );

                moveableObjects[i].transform.position = newPosition;
            }

            yield return null;
        }

        foreach (var obj in moveableObjects)
        {
            Vector3 randomPosition = lightSphere.transform.position + new Vector3(
                Random.Range(-radius, radius),
                0f,
                Random.Range(-radius, radius)
            );

            obj.transform.DOMove(randomPosition, shuffleDuration).SetEase(Ease.InOutQuad);
        }

        yield return new WaitForSeconds(shuffleDuration);

        if (lightSphere != null)
        {
            lightSphere.SetActive(false);
            Debug.Log("Light sphere deactivated!");
        }

        yield return new WaitForSeconds(5f);
        skillButton.interactable = true;
        Debug.Log("Skill button reactivated!");
    }
}
