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

        slots.AddRange(slotsParent.GetComponentsInChildren<Slot>()); // 칠드런으로 받아오는 이유는 비활성화 되면 못받아오기때문에 비활성화된 상태도 찾아올수있는 칠드런으로 받아온다.
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
    /// 인벤토리에 아이템을 추가시킴. 겹칠수 있는 아이템은 겹쳐줌.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void GetItem(Item _item, int count = 1)
    {
        if (_item.itemType != Item.ItemType.Equipment) // 장비가 아닌종류일때 (소비, 재료 등)
        {
            foreach (Slot slot in slots) // 모든 슬롯중
            {
                if (slot.item != null) // 아이템이 있는 슬롯에
                {
                    if (slot.item.itemName == _item.itemName) // 같은 이름을 가진 item이 있으면
                    {
                        slot.SetSlotCount(count); // 갯수를 증가시킴
                        return;
                    }
                }
            }
        }

        // 장비 종류에 상관없이
        foreach (Slot slot in slots) // 모든 슬롯중
        {
            if (slot.item == null) // 비어 있는 슬롯에
            {
                slot.AddItem(_item, count); // 아이템추가
                return;
            }
        }
    }

    /// <summary>
    /// 게임도중에 슬롯갯수가 바뀌면 없는 슬롯을 리스트에서 제거해줌
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
