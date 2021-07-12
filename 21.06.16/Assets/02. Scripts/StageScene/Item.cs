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

    public string itemName;
    [TextArea]
    public string itemDescription;
    public ItemType itemType;
    public ItemRarelity itemRarelity;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string EquipmentType;
}
