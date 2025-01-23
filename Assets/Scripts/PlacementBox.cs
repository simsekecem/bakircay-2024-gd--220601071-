using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class PlacementBox : MonoBehaviour
{
    public GameObject currentObject;
    public Animator lidAnimator; // Animator referans�, Inspector'da atanmal�.

    [SerializeField] private Transform _leftObjectPlacement;
    [SerializeField] private Transform _rightObjectPlacement;
    [SerializeField] private ScoreManager scoreManager;
    public Button autoMatchButton;

    private readonly string objectTag = "Moveable";
    private Coroutine matchCoroutine;

    // Lid'in a��k veya kapal� durumunu kontrol eden de�i�kenler
    private bool isLidOpen = false; // Lid ba�lang��ta kapal�
    private bool isLidAnimating = false; // Animasyon oynarken ba�ka bir tetikleme engellenir

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null || other.attachedRigidbody.CompareTag(objectTag) == false)
            return;

        if (other.gameObject == currentObject)
            return;

        if (currentObject == null)
        {
            SetCurrentObject(other);

            if (autoMatchButton != null)
            {
                autoMatchButton.interactable = false;
            }
        }
        else
        {
            if (CheckMatch(other))
                return;

            other.attachedRigidbody.AddForce(Vector3.up * 30f + Vector3.forward * 50f, ForceMode.Impulse);
        }
    }

    public bool CheckMatch(Collider other)
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

        // Lid Open animasyonu bir kez �al��t�r�l�r
        if (!isLidOpen && !isLidAnimating)
        {
            isLidAnimating = true;
            lidAnimator.SetTrigger("LidOpen");
            yield return new WaitForSeconds(1f); // Animasyon s�resince bekleme
            isLidOpen = true;
            isLidAnimating = false;
        }

        otherItem.transform.position = _rightObjectPlacement.position;
        otherItem.transform.rotation = _rightObjectPlacement.rotation;

        yield return new WaitForSeconds(1f);

        Vector3 targetPos = (currentItem.transform.position + otherItem.transform.position) / 2f;

       

        currentItem.transform.position = targetPos;
        otherItem.transform.position = targetPos;
        PlayBurnEffect(currentItem.gameObject);
        PlayBurnEffect(otherItem.gameObject);

        // Efektin bitmesini bekleyin
        yield return new WaitForSeconds(3f);

        targetPos += Vector3.down * 2f;
        currentItem.transform.position = targetPos;
        otherItem.transform.position = targetPos;

        yield return new WaitForSeconds(1f);

        if (currentItem != null && otherItem != null)
        {

            if (currentItem.itemData != null)
            {
                Debug.Log("CurrentItem Score: " + currentItem.itemData.itemScore);
                scoreManager.UpdateScore(currentItem.itemData.itemScore);
            }
            else
            {
                Debug.Log("Item data is null!");
            }

            


            currentItem.gameObject.SetActive(false);
            otherItem.gameObject.SetActive(false);

  
            // Lid Close animasyonu bir kez �al��t�r�l�r
            if (isLidOpen && !isLidAnimating)
            {
                isLidAnimating = true;
                lidAnimator.SetTrigger("LidClose");
                yield return new WaitForSeconds(1f); // Animasyon s�resince bekleme
                isLidOpen = false;
                isLidAnimating = false;
            }
            currentObject = null;
            matchCoroutine = null;
        }


        yield return new WaitForSeconds(5f);
        if (autoMatchButton != null)
        {
            autoMatchButton.interactable = true;
        }


    }

    private void PlayBurnEffect(GameObject targetObject)
    {
        ParticleSystem burnEffect = targetObject.GetComponentInChildren<ParticleSystem>();
        if (burnEffect != null)
        {
            burnEffect.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == null || other.attachedRigidbody.CompareTag(objectTag) == false)
            return;

        if (other.attachedRigidbody.gameObject == currentObject)
        {
            currentObject = null;

            if (autoMatchButton != null)
            {
                autoMatchButton.interactable = true;
            }
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
