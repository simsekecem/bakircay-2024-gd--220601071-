using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlacementBox : MonoBehaviour
{
    public GameObject currentObject;
    public Animator lidAnimator;

    [SerializeField] private Transform _leftObjectPlacement;
    [SerializeField] private Transform _rightObjectPlacement;
    [SerializeField] private ScoreManager scoreManager;
    public Button autoMatchButton;
    public Button shuffleButton;

    private readonly string objectTag = "Moveable";
    private Coroutine matchCoroutine;

    private bool isLidOpen = false;
    private bool isLidAnimating = false;

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

            UpdateShuffleButtonInteractable();
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

        // Eþleþme baþlar baþlamaz Shuffle butonunu devre dýþý býrak
        if (shuffleButton != null)
        {
            shuffleButton.interactable = false;
        }

        matchCoroutine = StartCoroutine(MatchCoroutine(otherItem));
        return true;
    }

    private IEnumerator MatchCoroutine(Item otherItem)
    {
        var currentItem = currentObject.GetComponent<Item>();

        currentItem.SetCollidersActive(false);
        otherItem.SetCollidersActive(false);

        otherItem.transform.DOMove(_rightObjectPlacement.position, 0.5f);
        otherItem.transform.DORotateQuaternion(_rightObjectPlacement.rotation, 0.5f);

        currentItem.transform.DOMove(_leftObjectPlacement.position, 0.5f);
        currentItem.transform.DORotateQuaternion(_leftObjectPlacement.rotation, 0.5f);

        yield return new WaitForSeconds(0.5f);

        if (!isLidOpen && !isLidAnimating)
        {
            isLidAnimating = true;

            AudioManager.Instance.PlaySound(AudioManager.Instance.lidOpenSound);

            lidAnimator.SetTrigger("LidOpen");
            yield return new WaitForSeconds(1f);
            isLidOpen = true;
            isLidAnimating = false;

            UpdateShuffleButtonInteractable(); // Shuffle durumu güncellenir
        }

        Vector3 targetPos = (currentItem.transform.position + otherItem.transform.position) / 2f;
        currentItem.transform.DOMove(targetPos, 1f);
        otherItem.transform.DOMove(targetPos, 1f);
        yield return new WaitForSeconds(1f);

        PlayBurnEffect(currentItem.gameObject);
        PlayBurnEffect(otherItem.gameObject);
        yield return new WaitForSeconds(3f);

        targetPos += Vector3.down * 5f;
        currentItem.transform.DOMove(targetPos, 1f);
        otherItem.transform.DOMove(targetPos, 1f);
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySound(AudioManager.Instance.matchSound);

        if (currentItem != null && otherItem != null)
        {
            Debug.Log("CurrentItem Score: " + currentItem.itemData.itemScore);
            scoreManager.UpdateScore(currentItem.itemData.itemScore);

            currentItem.gameObject.SetActive(false);
            otherItem.gameObject.SetActive(false);

            if (isLidOpen && !isLidAnimating)
            {
                isLidAnimating = true;

                AudioManager.Instance.PlaySound(AudioManager.Instance.lidCloseSound);

                lidAnimator.SetTrigger("LidClose");
                yield return new WaitForSeconds(1f); // Kapak kapanma süresini bekle
                isLidOpen = false;
                isLidAnimating = false;

                UpdateShuffleButtonInteractable(); // Kapak kapandýktan sonra buton durumu güncelle
            }
            currentObject = null;
            matchCoroutine = null;

            // Eþleþme tamamlandýðýnda Shuffle butonunu tekrar aktif hale getir
            if (shuffleButton != null)
            {
                shuffleButton.interactable = true;
            }
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

    private void SetCurrentObject(Collider other)
    {
        other.attachedRigidbody.isKinematic = true;
        currentObject = other.attachedRigidbody.gameObject;

        currentObject.transform.DOMove(_leftObjectPlacement.position, 0.5f);
        currentObject.transform.DORotateQuaternion(_leftObjectPlacement.rotation, 0.5f);

        UpdateShuffleButtonInteractable(); // Shuffle durumu güncellenir
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

            UpdateShuffleButtonInteractable(); // Shuffle durumu güncellenir
        }
    }

    private void UpdateShuffleButtonInteractable()
    {
        if (shuffleButton != null)
        {
            shuffleButton.interactable = !isLidOpen && !isLidAnimating && matchCoroutine == null; // Eþleþme iþlemi yapýlmýyorsa aktif
        }
    }

    public void OnLidClosed() // Animator Event için metod
    {
        isLidOpen = false;
        isLidAnimating = false;
        UpdateShuffleButtonInteractable();
    }
}
