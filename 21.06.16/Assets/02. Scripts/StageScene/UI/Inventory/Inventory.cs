using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    [SerializeField]
    Text goldText;

    [SerializeField]
    PlayerCtrl player;

    RectTransform rect;

    public List<Slot> slots;

    public bool isFull = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        
        slots = new List<Slot>();

        slots.AddRange(slotsParent.GetComponentsInChildren<Slot>()); // ĥ�己���� �޾ƿ��� ������ ��Ȱ��ȭ �Ǹ� ���޾ƿ��⶧���� ��Ȱ��ȭ�� ���µ� ã�ƿü��ִ� ĥ�己���� �޾ƿ´�.
    }

    private void Start()
    {
        rect.localPosition = new Vector3(190, -12);
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

        goldText.text = string.Format("{0:N0}", player.gold);
    }

    /// <summary>
    /// �κ��丮�� �������� �߰���Ŵ. ��ĥ�� �ִ� �������� ������.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
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

    /// <summary>
    /// ���ӵ��߿� ���԰����� �ٲ�� ���� ������ ����Ʈ���� ��������
    /// </summary>
    void DeleteNullSlot()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i] == null)
                slots.RemoveAt(i);
        }
    }
}
