using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;

    public int itemScore;

    private const string itemPurchaseKey = "ItemPurchased::";

    public bool IsPurchased
    {
        get { return PlayerPrefs.GetInt(itemPurchaseKey + itemName, 0) == 1; }
        set { PlayerPrefs.SetInt(itemPurchaseKey + itemName, value ? 1 : 0); }
    }
}