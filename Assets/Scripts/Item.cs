using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int matchID = -1;
    public Rigidbody selfRigidbody;

    public bool isDragged = false;
    public bool isPlaced = false;
    public ItemData itemData;
    public float spawnWidth = 10f; // X ekseni geniþliði
    public float spawnDepth = 10f; // Z ekseni uzunluðu


    private List<Collider> _colliders = new List<Collider>();

    private void Awake()
    {
        selfRigidbody = GetComponent<Rigidbody>();
        _colliders.AddRange(GetComponentsInChildren<Collider>());
    }

    public bool IsMatching(Item otherItem)
    {
        return this != otherItem && matchID == otherItem.matchID;
    }

    public void SetCollidersActive(bool isActive)
    {
        foreach (var col in _colliders)
        {
            col.enabled = isActive;
        }
    }

    private void FixedUpdate()
    {
        if (isPlaced || isDragged)
            return;

        if (transform.position.y < -5)
        {
            Debug.Log($"Re-positioning object: {gameObject.name}");
            RePositionObject();
        }
    }

    private void RePositionObject()
    {
        selfRigidbody.velocity = Vector3.zero;
        selfRigidbody.angularVelocity = Vector3.zero;

        selfRigidbody.isKinematic = true;

        // Rastgele spawn pozisyonu hesaplama
        transform.position = new Vector3(
            Random.Range(-spawnWidth * 0.5f, spawnWidth * 0.5f), // X ekseni için rastgele deðer
            2f, // Sabit bir Y pozisyonu
            Random.Range(-spawnDepth * 0.5f, spawnDepth * 0.5f)  // Z ekseni için rastgele deðer
        );

        selfRigidbody.isKinematic = false;
    }

}