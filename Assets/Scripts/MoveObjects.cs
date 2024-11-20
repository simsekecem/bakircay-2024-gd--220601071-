using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private Vector3 mousePosition;
    public float targetHeight = 5f; // The height the object will slowly reach
    public float liftSpeed = 1f;    // The speed of the smooth lift
    private bool isLifting = false; // Whether the object is lifting or not
    private float initialYPosition; // Starting Y position of the object
    private float liftDuration = 0.5f; // How long the lifting takes to reach targetHeight

    private Vector3 initialPosition;

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        // Capture the initial position when the mouse is clicked
        mousePosition = Input.mousePosition - GetMousePos();
        initialPosition = transform.position; // Capture the initial object position
        initialYPosition = transform.position.y; // Capture the starting height

        // Start lifting smoothly
        if (!isLifting)
        {
            StartCoroutine(LiftObject());
        }
    }

    private void OnMouseDrag()
    {
        // Update the object's position while dragging
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
        worldMousePos.y = Mathf.Max(targetHeight, worldMousePos.y); // Ensure we don't go below targetHeight
        transform.position = worldMousePos;
    }

    private void OnMouseUp()
    {
        // On mouse release, start gravity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true; // Enable gravity to allow the object to fall
        }
        isLifting = false; // Stop the lifting process
    }

    private void Start()
    {
        // Initially disable gravity for the lifting process
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Disable gravity while lifting
        }
    }

    private IEnumerator LiftObject()
    {
        isLifting = true;
        float elapsedTime = 0f;

        // While the elapsed time is less than the lifting duration
        while (elapsedTime < liftDuration)
        {
            // Smoothly interpolate the Y position
            float newY = Mathf.Lerp(initialYPosition, targetHeight, elapsedTime / liftDuration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Increment elapsed time
            elapsedTime += Time.deltaTime * liftSpeed; // Ensure the lift happens over time
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is exactly at the targetHeight
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        isLifting = false;
    }
}
