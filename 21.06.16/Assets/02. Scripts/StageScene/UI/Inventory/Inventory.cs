using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    ItemEffectDatebase itemdatebase;

    [SerializeField]
    GameObject slotsParent;

    public List<Slot> slots;

    public bool isFull = false;

    private void Awake()
    {
        slots = new List<Slot>();

        slots.AddRange(slotsParent.GetComponentsInChildren<Slot>());
    }

    private void Update()
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
        if (_item.itemType != Item.ItemType.Equipment) // ��� �ƴ������϶� (�Һ�, ��� ��)
        {
            foreach (Slot slot in slots) // ��� ������
            {
                if (slot.item != null) // �������� �ִ� ���Կ�
                {
                    if (slot.item.itemName == _item.itemName) // ���� �̸��� ���� item�� ������
                    {
                        slot.SetSlotCount(count); // ������ ������Ŵ
                        return;
                    }
                }
            }
        }

        // ��� ������ �������
        foreach (Slot slot in slots) // ��� ������
        {
            if (slot.item == null) // ��� �ִ� ���Կ�
            {
                slot.AddItem(_item, count); // �������߰�
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
