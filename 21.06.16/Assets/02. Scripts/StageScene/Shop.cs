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

        if (shopBase.activeSelf) // 상점창이 켜져있을때
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // ESC키를 누르면
            {
                shopBase.SetActive(false); // 상점창 꺼짐
            }
            else if (isBuying || isSelling) // 사기or팔기 기능중 하나가 켜져있을때
            {
                if (Input.GetMouseButtonDown(1)) // 오른쪽 클릭하면 둘다 꺼줌(기본상태가됨)
                {
                    isBuying = false;
                    isSelling = false;
                }
            }
        }
        else // 상점창이 꺼지면 두 기능다 꺼줌
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
        // 장비 종류에 상관없이
        foreach (ShopSlot slot in slots) // 모든 슬롯중
        {
            if (slot.item == null) // 비어 있는 슬롯에
            {
                slot.AddItem(_item); // 아이템추가
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
