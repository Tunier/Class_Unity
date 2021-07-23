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
        if (_item.itemType != Item.ItemType.Equipment) // ��� �ƴ������϶� (�Һ�, ��� ��)
        {
            for (int i = 0; i < slots.Count; i++) // ��� ������
            {
                if (slots[i].item != null) // �������� �ִ� ���Կ�
                {
                    if (slots[i].item.itemName == _item.itemName) // ���� �̸��� ���� item�� ������
                    {
                        slots[i].SetSlotCount(count); // ������ ������Ŵ
                        return;
                    }
                }
            }
        }

        // ��� ������ �������
        for (int i = 0; i < slots.Count; i++) // ��� ������
        {
            if (slots[i].item == null) // ��� �ִ� ���Կ�
            {
                slots[i].AddItem(_item, count); // �������߰�
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
