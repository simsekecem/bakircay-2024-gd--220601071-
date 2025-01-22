using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnFoods : MonoBehaviour
{
    public ItemRepository itemRepository;
    [Range(1, 8)] public int spawnCount = 8;
    public Vector3 spawnArea = new Vector3(5, 1, 5);
    private const float spawnDistance = 1f;
    public List<Transform> spawnedObjects = new List<Transform>();
    public AutoMatchManager autoMatchManager; // AutoMatchManager referansý

    private void Start()
    {
        SpawnObjects();
    }

    private void Update()
    {
        // Eðer sahnede "Moveable" tagýna sahip nesne kalmadýysa yeniden spawn yap
        if (!GameObject.FindGameObjectsWithTag("Moveable").Any())
        {
            SpawnObjects();
        }
    }

    [ContextMenu("Spawn Objects")]
    private void SpawnObjects()
    {
        // Clear previously spawned objects
        spawnedObjects.ForEach(x =>
        {
            if (x != null)
                Destroy(x.gameObject);
        });
        spawnedObjects.Clear();

        // AutoMatchManager'ýn items listesini temizle
        if (autoMatchManager != null)
        {
            autoMatchManager.items.Clear();
        }

        int maxTries = 100; // Maximum number of tries for finding a valid position
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

            // Find a valid position for the first object
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

            // Find a valid position for the second object
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

            // Spawn the objects
            var itemPrefab = itemDatas[i].itemPrefab;

            var firstInstance = Instantiate(itemPrefab, firstSpawnPosition, Quaternion.identity);
            var secondInstance = Instantiate(itemPrefab, secondSpawnPosition, Quaternion.identity);

            firstInstance.GetComponent<Item>().matchID = i;
            secondInstance.GetComponent<Item>().matchID = i;

            spawnedObjects.Add(firstInstance.transform);
            spawnedObjects.Add(secondInstance.transform);

            // AutoMatchManager'ýn items listesine ekle
            if (autoMatchManager != null)
            {
                autoMatchManager.items.Add(firstInstance);
                autoMatchManager.items.Add(secondInstance);
            }
        }
    }

    private Vector3 GetRandomPos()
    {
        // Generate a random position within the spawn area
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
