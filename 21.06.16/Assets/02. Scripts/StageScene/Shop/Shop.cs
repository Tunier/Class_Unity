using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    GameObject shopSlot;
    [SerializeField]
    GameObject shopBase;
    [SerializeField]
    GameObject slotsGroup;

    public List<ShopSlot> slots = new List<ShopSlot>();

    public bool isBuying = false;
    public bool isSelling = false;
    public bool isFull = false;

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
        
        // 상점창 꽉찼는지 체크
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[slots.Count - 1].item != null)
            {
                isFull = true;
            }
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
        if (!isFull)
        {
            // 장비 종류에 상관없이
            foreach (ShopSlot slot in slots) // 모든 슬롯중
            {
                if (slot.item == null) // 비어 있는 슬롯에
                {
                    slot.AddItem(_item); // 아이템추가
                    return;
                }
            }
        }
        else
        {   // 상점 슬롯이 꽉차면 첫칸을 없에고 빈 새로운 칸을 만들어서 한칸씩 땡겨진것처럼 보이게함.
            // 추후에 게임을 껐다가 키거나, 일정 조건을 만족하면 상점창 초기화(기본적으로 파는 아이템만 있게) 할것.
            Destroy(slots[0].gameObject);
            var obj = Instantiate(shopSlot, slotsGroup.transform);
            slots.Add(obj.GetComponent<ShopSlot>());

            foreach (ShopSlot slot in slots)
            {
                if (slot.item == null)
                {
                    slot.AddItem(_item);
                    return;
                }
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
