using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Used,
        Resources,
    }

    public enum ItemRarelity
    {
        Common,
        Rare,
        Epic,
    }

    public ItemType itemType;
    public string itemName;
    public ItemRarelity itemRarelity;
    [TextArea]
    public string itemDescription;
    public int buyCost;
    public int sellCost;

    public Sprite itemImage;
    public GameObject itemPrefab;

    public string EquipmentType;
}
