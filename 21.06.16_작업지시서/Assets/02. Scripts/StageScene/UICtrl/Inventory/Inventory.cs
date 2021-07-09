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
        if (_item.itemType != Item.ItemType.Equipment) // 장비가 아닌종류일때
        {
            for (int i = 0; i < slots.Length; i++) // 모든 슬롯중
            {
                if (slots[i].item != null) // 아이템이 있는 슬롯에
                {
                    if (slots[i].item.itemName == _item.itemName) // 같은 이름을 가진 item이 있으면
                    {
                        slots[i].SetSlotCount(count); // 갯수를 증가시킴
                        return; // 함수종료
                    }
                }
            }
        }

        // 장비인 경우
        for (int i = 0; i < slots.Length; i++) // 모든 슬롯중
        {
            if (slots[i].item == null) // 비어 있는 슬롯에
            {
                slots[i].AddItem(_item, count); // 아이템추가
                return; // 함수종료
            }
        }
    }
}
