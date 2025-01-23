using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoMatchManager : MonoBehaviour
{
    public Transform leftPlace; 
    public Transform rightPlace; 
    public List<GameObject> items = new List<GameObject>();
    public Button autoMatchButton;
    public PlacementBox placementBox;

    private bool isMatchingInProgress = false; 
    private bool isButtonLocked = false; // Týklama kilidi


    public void OnAutoMatchButtonClicked()
    {
        // Eðer buton zaten kilitliyse veya PlacementBox doluysa iþlem yapma
        if (isButtonLocked || (placementBox != null && placementBox.currentObject != null))
        {
            Debug.LogWarning("Button is locked or PlacementBox is not empty.");
            return;
        }

        isButtonLocked = true;

        // Eðer eþleþme iþlemi zaten devam ediyorsa hiçbir þey yapma
        if (isMatchingInProgress)
        {
            Debug.LogWarning("Auto-match is already in progress.");
            isButtonLocked = false;
            return;
        }

        // Rastgele bir item seç ve eþini bul
        GameObject[] matchedItems = GetMatchedItems();
        if (matchedItems == null || matchedItems.Length < 2)
        {
            Debug.LogWarning("No matching items found.");
            isButtonLocked = false;
            return;
        }

        isMatchingInProgress = true;
        autoMatchButton.interactable = false;

        PlaceItems(matchedItems);

        StartCoroutine(ResetAutoMatchButton());
        StartCoroutine(UnlockButtonAfterDelay());
    }


    private GameObject[] GetMatchedItems()
    {
        if (items.Count < 2)
            return null;

        // Rastgele bir item seç
        int randomIndex = Random.Range(0, items.Count);
        GameObject selectedItem = items[randomIndex];
        Item selectedItemData = selectedItem.GetComponent<Item>();

        if (selectedItemData == null)
            return null;

  
        GameObject matchedItem = items.Find(item =>
            item != selectedItem &&
            item.GetComponent<Item>()?.matchID == selectedItemData.matchID);

        if (matchedItem == null)
            return null;

        return new GameObject[] { selectedItem, matchedItem };
    }

    private void PlaceItems(GameObject[] matchedItems)
    {
        // Eþleþen nesneleri uygun pozisyonlara taþý
        matchedItems[0].transform.position = leftPlace.position;
        matchedItems[0].transform.rotation = leftPlace.rotation;

        matchedItems[1].transform.position = rightPlace.position;
        matchedItems[1].transform.rotation = rightPlace.rotation;
    }

    private IEnumerator ResetAutoMatchButton()
    {
        yield return new WaitForSeconds(5f); 
        isMatchingInProgress = false;
        autoMatchButton.interactable = true; 
    }

    // Buton kilidini belirli bir süre sonra aç
    private IEnumerator UnlockButtonAfterDelay()
    {
        yield return new WaitForSeconds(1f); 
        isButtonLocked = false;
    }
}
