using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    public List<Slot> slots;

    public bool isFull = false;

    private void Awake()
    {
        slots = new List<Slot>();
        slots.AddRange(slotsParent.GetComponentsInChildren<Slot>());
    }

    void Update()
    {
        DeleteNullSlot();

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
                isFull = false;

            if (i == (slots.Count - 1))
                if (slots[i].item != null)
                    isFull = true;
        }
    }

    public void GetItem(Item _item, int count = 1)
    {
        if (_item.itemType != Item.ItemType.Equipment) // 장비가 아닌종류일때 (소비, 재료 등)
        {
            for (int i = 0; i < slots.Count; i++) // 모든 슬롯중
            {
                if (slots[i].item != null) // 아이템이 있는 슬롯에
                {
                    if (slots[i].item.itemName == _item.itemName) // 같은 이름을 가진 item이 있으면
                    {
                        slots[i].SetSlotCount(count); // 갯수를 증가시킴
                        return;
                    }
                }
            }
        }

        // 장비 종류에 상관없이
        for (int i = 0; i < slots.Count; i++) // 모든 슬롯중
        {
            if (slots[i].item == null) // 비어 있는 슬롯에
            {
                slots[i].AddItem(_item, count); // 아이템추가
                return;
            }
        }
    }

    void DeleteNullSlot()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i] == null)
                slots.RemoveAt(i);
        }
    }
}
