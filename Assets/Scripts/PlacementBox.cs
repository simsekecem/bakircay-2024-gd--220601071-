using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBox : MonoBehaviour
{
    public GameObject currentObject;

    [SerializeField] private Transform _leftObjectPlacement;
    [SerializeField] private Transform _rightObjectPlacement;

    private readonly string objectTag = "Moveable";
    private Coroutine matchCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null || other.attachedRigidbody.CompareTag(objectTag) == false)
            return;

        if (other.gameObject == currentObject)
            return;

        if (currentObject == null)
        {
            SetCurrentObject(other);
        }
        else
        {
            if (CheckMatch(other))
                return;

            other.attachedRigidbody.AddForce(Vector3.up * 30f + Vector3.forward * 50f, ForceMode.Impulse);
        }
    }

    private bool CheckMatch(Collider other)
    {
        if (matchCoroutine != null)
        {
            return false;
        }

        var currentItem = currentObject.GetComponent<Item>();
        var otherItem = other.attachedRigidbody.gameObject.GetComponent<Item>();
        if (!currentItem.IsMatching(otherItem))
            return false;

        other.attachedRigidbody.isKinematic = true;
        matchCoroutine = StartCoroutine(MatchCoroutine(otherItem));
        return true;
    }

    private IEnumerator MatchCoroutine(Item otherItem)
    {
        var currentItem = currentObject.GetComponent<Item>();

        currentItem.SetCollidersActive(false);
        otherItem.SetCollidersActive(false);


        otherItem.transform.position = _rightObjectPlacement.position;
        otherItem.transform.rotation = _rightObjectPlacement.rotation;

        yield return new WaitForSeconds(1f);

        Vector3 targetPos = (currentItem.transform.position + otherItem.transform.position) / 2f;

        currentItem.transform.position = targetPos;
        otherItem.transform.position = targetPos;

        yield return new WaitForSeconds(1f);

        targetPos += Vector3.down * 2f;
        currentItem.transform.position = targetPos;
        otherItem.transform.position = targetPos;

        yield return new WaitForSeconds(1f);

        if (currentItem != null && otherItem != null)
        {
            currentItem.gameObject.SetActive(false);
            otherItem.gameObject.SetActive(false);
        }

        currentObject = null;
        matchCoroutine = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null || other.attachedRigidbody.CompareTag(objectTag) == false)
            return;
        if (other.attachedRigidbody.gameObject == currentObject)
        {
            currentObject = null;
        }
    }

    private void SetCurrentObject(Collider other)
    {
        other.attachedRigidbody.isKinematic = true;
        currentObject = other.attachedRigidbody.gameObject;

        currentObject.transform.position = _leftObjectPlacement.position;
        currentObject.transform.rotation = _leftObjectPlacement.rotation;
    }
}
