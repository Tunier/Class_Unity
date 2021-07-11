using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    public Slot[] slots;

    public Slot weaponSlot;

    ItemEffectDatebase itemDatebase;

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

    }
}



