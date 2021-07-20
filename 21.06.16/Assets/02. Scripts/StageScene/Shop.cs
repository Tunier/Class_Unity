using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    GameObject shopBase;

    [SerializeField]
    GameObject slotsGroup;

    [SerializeField]
    List<ShopSlot> slots = new List<ShopSlot>();

    public bool isBuying = false;
    public bool isSelling = false;

    void Start()
    {
        slots.AddRange(slotsGroup.GetComponentsInChildren<ShopSlot>());

        slots[0].AddItem(Resources.Load<Item>("ItemInfo/Sword2"));
    }

    void Update()
    {
        DeleteNullSlot();

        if (shopBase.activeSelf) // ����â�� ����������
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ�� ������
            {
                shopBase.SetActive(false); // ����â ����
            }
            else if (isBuying || isSelling) // ���or�ȱ� ����� �ϳ��� ����������
            {
                if (Input.GetMouseButtonDown(1)) // ������ Ŭ���ϸ� �Ѵ� ����(�⺻���°���)
                {
                    isBuying = false;
                    isSelling = false;
                }
            }
        }
        else // ����â�� ������ �� ��ɴ� ����
        {
            isBuying = false;
            isSelling = false;
        }
    }

    public void OnClickBuyButton()
    {
        isBuying = true;
        isSelling = false;
    }

    public void OnClickSellButton()
    {
        isSelling = true;
        isBuying = false;
    }

    public void OnClickExitButton()
    {
        shopBase.SetActive(false);
    }

    public void GetItem(Item _item)
    {
        // ��� ������ �������
        foreach (ShopSlot slot in slots) // ��� ������
        {
            if (slot.item == null) // ��� �ִ� ���Կ�
            {
                slot.AddItem(_item); // �������߰�
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
