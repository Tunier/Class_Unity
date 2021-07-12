using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    public Slot[] slots;

    public Slot weaponSlot;

    [SerializeField]
    GameObject[] baseImages;

    [SerializeField]
    Item baseWeapon;

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();

        foreach (Slot slot in slots)
        {
            if (slot.name == "WeaponSlot")
                weaponSlot = slot;
        }

        //itemDatebase.EquipItem(baseWeapon);
    }

    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                baseImages[i].SetActive(false);
            }
            else
            {
                baseImages[i].SetActive(true);
            }
        }
    }
}



