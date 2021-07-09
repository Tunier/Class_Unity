using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField]
    Text countText;
    [SerializeField]
    GameObject countImage;

    Status status;
    ItemEffectDatebase database;
    InputNumberUI inputNumber;
    Inventory inven;

    Rect baseRect; // InventoryBase의 이미지 Rect 정보 가져옴.

    private void Start()
    {
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        status = FindObjectOfType<Status>();
        database = FindObjectOfType<ItemEffectDatebase>();
        inputNumber = FindObjectOfType<InputNumberUI>();
        inven = FindObjectOfType<Inventory>();
    }

    void SetColorAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int count = 1)
    {
        item = _item;
        itemCount = count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            countImage.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.text = "0";
            countImage.SetActive(false);
        }

        SetColorAlpha(1);
    }

    public void SetSlotCount(int count)
    {
        itemCount += count;
        countText.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColorAlpha(0);

        countText.text = "0";
        countImage.SetActive(false);
    }

    // 슬롯 우클릭시 슬롯에 있는 아이템 타입을 보고 소모품이면 아이템 사용
    // 장비면 스테이터스창의 해당 장비타입칸에 착용.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (transform.parent.CompareTag("INVENTORY")) // 인벤토리에 있는 슬롯.
                if (item != null)
                {
                    database.UseItem(item);

                    if (item.itemType == Item.ItemType.Used)
                        SetSlotCount(-1);
                    else if (item.itemType == Item.ItemType.Equipment)
                    {
                        if (item.EquipmentType == "Weapon")
                        {
                            if (status.slots[0].item == null)
                                SetSlotCount(-1);
                        }
                    }
                }

            if (transform.parent.CompareTag("STATUS")) // 스테이터스 창에 있는 슬롯.
                if (item != null)
                {
                    UnEquipItem(item);
                }
        }
    }

    // 드레그 시작시 드레그 시작한 슬롯에 있는 아이템 드레그 슬롯에 복제
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;

        }
    }

    // 드레그 하는 동안 포지션 변경
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // 드레그 끝나면 드레그 슬롯 알파값 0으로 바꾸고 비워줌
    public void OnEndDrag(PointerEventData eventData)
    {
        if (DragSlot.instance.transform.localPosition.x < baseRect.xMin
            || DragSlot.instance.transform.localPosition.x > baseRect.xMax
            || DragSlot.instance.transform.localPosition.y < baseRect.yMin
            || DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            if (DragSlot.instance.dragSlot != null)
            {
                if (DragSlot.instance.dragSlot.itemCount <= 1)
                {
                    DragSlot.instance.SetColorAlpha(0);
                    StartCoroutine(inputNumber.DropItemCoruntine(1));
                }
                else
                    inputNumber.Call();
            }
        }
        else
        {
            DragSlot.instance.SetColorAlpha(0);
            DragSlot.instance.dragSlot = null;
        }
    }

    // 드레그해서 드롭했을경우 슬롯두개 내용물을 바꿔줌, 비어있으면 넣어만줌.
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.parent.CompareTag("INVENTORY"))
            if (DragSlot.instance.dragSlot != null)
                ChangeSlot();

        if (transform.parent.CompareTag("STATUS"))
            if (DragSlot.instance.dragSlot != null)
                if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
                    ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item _item = item;
        int _itemCount = itemCount;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (_item != null)
            DragSlot.instance.dragSlot.AddItem(_item, _itemCount);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }

    public void UnEquipItem(Item _item)
    {
        inven.GetItem(_item);

        database.UnEquipItem(_item);

        SetSlotCount(-1);
    }
}
