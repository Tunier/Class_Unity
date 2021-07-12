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
    GameObject baseImage;

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
        if (weaponSlot.item != null)
        {
            baseImage.SetActive(false);
        }
    }
}



