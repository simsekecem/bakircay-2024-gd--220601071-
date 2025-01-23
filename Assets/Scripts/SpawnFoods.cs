using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnFoods : MonoBehaviour
{
    public event Action OnObjectsSpawned; 

    public ItemRepository itemRepository;
    public ScoreManager scoreManager;
    [Range(1, 8)] public int spawnCount = 8;
    public Vector3 spawnArea = new Vector3(5, 1, 5);
    private const float spawnDistance = 1f;
    public List<Transform> spawnedObjects = new List<Transform>();
    public AutoMatchManager autoMatchManager;

    private void Start()
    {
        SpawnObjects();
    }

    private void Update()
    {
        if (!GameObject.FindGameObjectsWithTag("Moveable").Any())
        {
            SpawnObjects();
        }
    }

    [ContextMenu("Spawn Objects")]
    private void SpawnObjects()
    {
        spawnedObjects.ForEach(x =>
        {
            if (x != null)
                Destroy(x.gameObject);
        });
        spawnedObjects.Clear();

        if (autoMatchManager != null)
        {
            autoMatchManager.items.Clear();
        }

        int maxTries = 100;
        int currentTryCount = 0;

        var itemDatas = itemRepository.GetRandomItems(spawnCount);
        if (itemDatas.Count == 0)
        {
            Debug.LogError("No items in the repository");
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 firstSpawnPosition;
            Vector3 secondSpawnPosition;
            bool validFirstPositionFound = false;
            bool validSecondPositionFound = false;

            do
            {
                firstSpawnPosition = GetRandomPos();
                currentTryCount++;

                if (!spawnedObjects.Any(x => x != null && Vector3.Distance(x.position, firstSpawnPosition) < spawnDistance))
                {
                    validFirstPositionFound = true;
                }
                else if (currentTryCount > maxTries)
                {
                    Debug.LogWarning("Max tries reached for spawning the first object.");
                    break;
                }

            } while (!validFirstPositionFound);

            if (!validFirstPositionFound)
                continue;

            currentTryCount = 0;

            do
            {
                secondSpawnPosition = GetRandomPos();
                currentTryCount++;

                if (!spawnedObjects.Any(x => x != null && Vector3.Distance(x.position, secondSpawnPosition) < spawnDistance))
                {
                    validSecondPositionFound = true;
                }
                else if (currentTryCount > maxTries)
                {
                    Debug.LogWarning("Max tries reached for spawning the second object.");
                    break;
                }

            } while (!validSecondPositionFound);

            if (!validSecondPositionFound)
                continue;

            currentTryCount = 0;

            var itemPrefab = itemDatas[i].itemPrefab;

            var firstInstance = Instantiate(itemPrefab, firstSpawnPosition, Quaternion.identity);
            var secondInstance = Instantiate(itemPrefab, secondSpawnPosition, Quaternion.identity);

            firstInstance.GetComponent<Item>().matchID = i;
            secondInstance.GetComponent<Item>().matchID = i;

            spawnedObjects.Add(firstInstance.transform);
            spawnedObjects.Add(secondInstance.transform);

            if (autoMatchManager != null)
            {
                autoMatchManager.items.Add(firstInstance);
                autoMatchManager.items.Add(secondInstance);
            }
        }


        if (scoreManager != null)
        {
            scoreManager.DeactivateDoubleScore();
        }

        OnObjectsSpawned?.Invoke();
    }

    private Vector3 GetRandomPos()
    {
        return new Vector3(
            UnityEngine.Random.Range(-spawnArea.x * 0.5f, spawnArea.x * 0.5f), 
            UnityEngine.Random.Range(-spawnArea.y * 0.5f, spawnArea.y * 0.5f),
            UnityEngine.Random.Range(-spawnArea.z * 0.5f, spawnArea.z * 0.5f)
        ) + transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnArea);
    }
}
