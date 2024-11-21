using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private Vector3 mousePosition;
    public float targetHeight = 5f; // The height
    public float liftSpeed = 1f;    // The speed of lift
    private bool isLifting = false; // The object is lifting or not
    private float initialYPosition; // Starting Y position
    private float liftDuration = 0.5f; // How long the lifting takes

    private Vector3 initialPosition;

    public Vector3 minBounds; // Minimum boundary
    public Vector3 maxBounds; // Maximum boundary

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        // Capture the position
        mousePosition = Input.mousePosition - GetMousePos();
        initialPosition = transform.position;
        initialYPosition = transform.position.y;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
        }

        // Start lifting
        if (!isLifting)
        {
            StartCoroutine(LiftObject());
        }
    }

    private void OnMouseDrag()
    {
        // Object's position while dragging
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        worldMousePos.y = Mathf.Max(targetHeight, worldMousePos.y); 

        worldMousePos.x = Mathf.Clamp(worldMousePos.x, minBounds.x, maxBounds.x);
        worldMousePos.y = Mathf.Clamp(worldMousePos.y, minBounds.y, maxBounds.y);
        worldMousePos.z = Mathf.Clamp(worldMousePos.z, minBounds.z, maxBounds.z);

        transform.position = worldMousePos;
    }

    private void OnMouseUp()
    {
        // Mouse release
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true; // Enable gravity
        }
        isLifting = false; // Stop the lifting
    }

    private void Start()
    {
    }

    private IEnumerator LiftObject()
    {
        isLifting = true;
        float elapsedTime = 0f;

        while (elapsedTime < liftDuration)
        {
           
            float newY = Mathf.Lerp(initialYPosition, targetHeight, elapsedTime / liftDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            
            elapsedTime += Time.deltaTime * liftSpeed;
            yield return null;
        }

        // The final position
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        isLifting = false;
    }
}
