using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    RectTransform rect;

    [SerializeField]
    GameObject slotsParent;

    public Slot[] slots;

    public Slot weaponSlot;

    [SerializeField]
    GameObject[] baseImages;

    [SerializeField]
    Item baseWeapon;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        rect.localPosition = new Vector3(-198, -12);

        slots = slotsParent.GetComponentsInChildren<Slot>();

        foreach (Slot slot in slots)
        {
            if (slot.name == "WeaponSlot")
                weaponSlot = slot;
        }

        slots[0].AddItem(Resources.Load<Item>("ItemInfo/Sword1"));
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



