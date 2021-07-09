using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    [SerializeField]
    Slot[] slots;

    public void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {

    }

    public void GetItem(Item _item, int count = 1)
    {
        if (_item.itemType != Item.ItemType.Equipment) // ��� �ƴ������϶�
        {
            for (int i = 0; i < slots.Length; i++) // ��� ������
            {
                if (slots[i].item != null) // �������� �ִ� ���Կ�
                {
                    if (slots[i].item.itemName == _item.itemName) // ���� �̸��� ���� item�� ������
                    {
                        slots[i].SetSlotCount(count); // ������ ������Ŵ
                        return; // �Լ�����
                    }
                }
            }
        }

        // ����� ���
        for (int i = 0; i < slots.Length; i++) // ��� ������
        {
            if (slots[i].item == null) // ��� �ִ� ���Կ�
            {
                slots[i].AddItem(_item, count); // �������߰�
                return; // �Լ�����
            }
        }
    }
}
