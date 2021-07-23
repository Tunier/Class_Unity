using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage;

    [SerializeField]
    GameObject countImage;
    [SerializeField]
    Text countText;

    Status status;
    ItemEffectDatebase database;
    InputNumberUI inputNumber;
    Inventory inven;
    Shop shop;

    PlayerCtrl player;

    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    RectTransform statusBase;
    [SerializeField]
    RectTransform quickSlotBase;
    [SerializeField]
    RectTransform shopBase;

    private void Start()
    {
        status = FindObjectOfType<Status>();
        database = FindObjectOfType<ItemEffectDatebase>();
        inputNumber = FindObjectOfType<InputNumberUI>();
        inven = FindObjectOfType<Inventory>();
        shop = FindObjectOfType<Shop>();
        player = FindObjectOfType<PlayerCtrl>();
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

    /// <summary>
    /// 해당슬롯의 아이템 카운트를 인수값 만큼 더해줌. 아이템 카운트가 0이되면 ClearSlot 발동.
    /// </summary>
    /// <param name="count"></param>
    public void SetSlotCount(int count)
    {
        itemCount += count;
        countText.text = itemCount.ToString();

        if (itemCount <= 0)
        {
            ClearSlot();
            database.HideToolTip();
        }
    }

    /// <summary>
    /// 해당칸을 비워줌. (item = null, itemCount = 0, 스프라이트 = null, 알파값 0으로)
    /// </summary>
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
            if (shopBase.gameObject.activeSelf)
            {
                if (transform.CompareTag("INVENTORY")) // 인벤토리에 있는 슬롯.
                    if (item != null)
                    {
                        SellItem(item);
                    }
            }
            else
            {
                if (transform.CompareTag("INVENTORY")) // 인벤토리에 있는 슬롯.
                    if (item != null)
                    {
                        if (item.itemType == Item.ItemType.Equipment)
                        {
                            Item _item = item;
                            SetSlotCount(-1);
                            database.EquipItem(_item);
                        }
                        else if (item.itemType == Item.ItemType.Used)
                        {
                            database.UseItem(item);
                            SetSlotCount(-1);
                        }
                    }

                if (transform.CompareTag("STATUS")) // 스테이터스 창에 있는 슬롯.
                    if (item != null)
                    {
                        UnEquipItem(item);
                    }

                if (transform.CompareTag("QUICKSLOT"))
                {
                    if (item != null)
                    {
                        if (item.itemType == Item.ItemType.Equipment)
                        {
                            if (item.EquipmentType == "Weapon")
                            {
                                if (status.weaponSlot.item == null)
                                {
                                    EquipItem(item);
                                    return;
                                }
                                else
                                    return;
                            }
                        }

                        database.UseItem(item);

                        if (item.itemType == Item.ItemType.Used)
                            SetSlotCount(-1);
                    }
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (shopBase.gameObject.activeSelf)
            {
                if (shop.isSelling)
                {
                    if (transform.CompareTag("INVENTORY")) // 인벤토리에 있는 슬롯.
                    {
                        if (item != null)
                        {
                            SellItem(item);
                        }
                    }
                }
            }
        }

        if (item != null)
        {
            database.ShowToolTip(item);
            database.SetItemCostText(item.sellCost);
        }
    }

    /// <summary>
    /// 아이템이 제거되고, 스텟창에 아이템이 종류에 맞게 장착됨.
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(Item _item)
    {
        database.EquipItem(_item);

        SetSlotCount(-1);
    }

    /// <summary>
    /// 인벤창에 아이템이 생성되고, 스텟창에 있는 장비슬롯의 아이템 겟수를 -1 (0이되서 ClearSlot함수실행됨)
    /// </summary>
    /// <param name="_item"></param>
    public void UnEquipItem(Item _item)
    {
        inven.GetItem(_item);

        database.UnEquipItem(_item);

        SetSlotCount(-1);
    }

    /// <summary>
    /// 슬롯을 비우고, 상점칸에 아이템이 추가되고, 플레이어의 골드가 아이템의 판매가만큼 증가함.
    /// </summary>
    /// <param name="_item"></param>
    void SellItem(Item _item)
    {
        ClearSlot();
        shop.GetItem(_item);
        player.gold += _item.sellCost;
    }

    // 드레그 시작시 드레그 시작한 슬롯에 있는 아이템 드레그 슬롯에 복제
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    // 드레그 하는 동안 포지션 변경
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // 드레그 끝나면 드레그 슬롯 알파값 0으로 바꾸고 비워줌
    // 드레그 끝난 위치가 스텟창이나 인벤창 밖이면 아이템 드롭창 불러줌.
    public void OnEndDrag(PointerEventData eventData)
    {
        // 활성화된 창만 계산되게 if문 넣어줌.
        if (invenBase.gameObject.activeSelf && statusBase.gameObject.activeSelf && status.gameObject.activeSelf)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
               || RectTransformUtility.RectangleContainsScreenPoint(statusBase, DragSlot.instance.transform.position)
               || RectTransformUtility.RectangleContainsScreenPoint(shopBase, DragSlot.instance.transform.position)
               || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                if (DragSlot.instance.dragSlot != null)
                    if (DragSlot.instance.dragSlot.itemCount <= 1)
                    {
                        DragSlot.instance.SetColorAlpha(0);
                        StartCoroutine(inputNumber.DropItemCoruntine(1));
                    }
                    else
                        inputNumber.Call();
            }
        }
        else if (invenBase.gameObject.activeSelf && statusBase.gameObject.activeSelf)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
                || RectTransformUtility.RectangleContainsScreenPoint(statusBase, DragSlot.instance.transform.position)
                || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                if (DragSlot.instance.dragSlot != null)
                    if (DragSlot.instance.dragSlot.itemCount <= 1)
                    {
                        DragSlot.instance.SetColorAlpha(0);
                        StartCoroutine(inputNumber.DropItemCoruntine(1));
                    }
                    else
                        inputNumber.Call();
            }
        }
        else if (invenBase.gameObject.activeSelf && shopBase.gameObject.activeSelf)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
                || RectTransformUtility.RectangleContainsScreenPoint(shopBase, DragSlot.instance.transform.position)
                || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                if (DragSlot.instance.dragSlot != null)
                    if (DragSlot.instance.dragSlot.itemCount <= 1)
                    {
                        DragSlot.instance.SetColorAlpha(0);
                        StartCoroutine(inputNumber.DropItemCoruntine(1));
                    }
                    else
                        inputNumber.Call();
            }
        }
        else if (invenBase.gameObject.activeSelf)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, DragSlot.instance.transform.position)
                || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                if (DragSlot.instance.dragSlot != null)
                    if (DragSlot.instance.dragSlot.itemCount <= 1)
                    {
                        DragSlot.instance.SetColorAlpha(0);
                        StartCoroutine(inputNumber.DropItemCoruntine(1));
                    }
                    else
                        inputNumber.Call();
            }
        }
        else if (statusBase.gameObject.activeSelf)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(statusBase, DragSlot.instance.transform.position)
                || RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                if (DragSlot.instance.dragSlot != null)
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
            if (RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, DragSlot.instance.transform.position))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
            else
            {
                if (DragSlot.instance.dragSlot != null)
                    if (DragSlot.instance.dragSlot.itemCount <= 1)
                    {
                        DragSlot.instance.SetColorAlpha(0);
                        StartCoroutine(inputNumber.DropItemCoruntine(1));
                    }
                    else
                        inputNumber.Call();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.parent.CompareTag("INVENTORY"))
            if (DragSlot.instance.dragSlot != null)
                if (DragSlot.instance.dragSlot.transform.parent.CompareTag("STATUS"))
                {   // 스텟창에서 인벤창으로 넘어올경우 같은 종류의 장비면 교체해주고, 아니면 아이템 획득.
                    Item _item = DragSlot.instance.dragSlot.item;

                    database.UnEquipItem(DragSlot.instance.dragSlot.item);
                    DragSlot.instance.dragSlot.SetSlotCount(-1);

                    if (item != null)
                    {
                        if (item.EquipmentType == _item.EquipmentType)
                        {
                            EquipItem(item);
                        }
                    }

                    inven.GetItem(_item);
                }
                else // 인벤 내에서는 교환해줌.
                {
                    ChangeSlot();
                }

        if (transform.parent.CompareTag("STATUS"))
            if (DragSlot.instance.dragSlot != null)
                if (DragSlot.instance.dragSlot.transform.parent.CompareTag("INVENTORY"))
                {   // 인벤창에서 스텟창으로 넘어갈경우 장비칸이 비어있으면 맞는 장비타입칸에 장착, 장비가 있으면 같은 장비타입일경우 교체.
                    if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
                    {
                        Item _item = item;

                        if (item != null)
                        {
                            if (item.EquipmentType == DragSlot.instance.dragSlot.item.EquipmentType)
                            {
                                database.UnEquipItem(_item);
                                SetSlotCount(-1);
                                database.EquipItem(DragSlot.instance.dragSlot.item);
                                DragSlot.instance.dragSlot.SetSlotCount(-1);
                                inven.GetItem(_item);
                            }
                        }
                        else if (item == null)
                        {
                            database.EquipItem(DragSlot.instance.dragSlot.item);
                            DragSlot.instance.dragSlot.SetSlotCount(-1);
                        }
                    }
                }

        if (transform.parent.CompareTag("QUICKSLOT"))
            if (DragSlot.instance.dragSlot != null)
                ChangeSlot();

        if (item != null)
        {
            database.ShowToolTip(item);
            database.SetItemCostText(item.sellCost);
        }
    }

    private void ChangeSlot()
    {
        Item _item = item;
        int _itemCount = itemCount;

        if (_item != null)
        {
            if (_item.itemName == DragSlot.instance.dragSlot.item.itemName)
            {
                if (_item.itemType == Item.ItemType.Used)
                {
                    SetSlotCount(DragSlot.instance.dragSlot.itemCount);

                    DragSlot.instance.dragSlot.ClearSlot();
                }
            }
            else
            {
                AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

                DragSlot.instance.dragSlot.AddItem(_item, _itemCount);
            }
        }
        else
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            database.ShowToolTip(item);
            database.SetItemCostText(item.sellCost);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        database.HideToolTip();
    }
}
