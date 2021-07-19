using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int itemId;
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

    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    RectTransform statusBase;
    [SerializeField]
    RectTransform quickSlotBase;

    private void Start()
    {
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
        itemId = 0;
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColorAlpha(0);

        countText.text = "0";
        countImage.SetActive(false);

        inven.isFull = false;
    }

    // 슬롯 우클릭시 슬롯에 있는 아이템 타입을 보고 소모품이면 아이템 사용
    // 장비면 스테이터스창의 해당 장비타입칸에 착용.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (transform.CompareTag("INVENTORY")) // 인벤토리에 있는 슬롯.
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

    public void EquipItem(Item _item)
    {
        database.EquipItem(_item);

        SetSlotCount(-1);
    }

    public void UnEquipItem(Item _item)
    {
        inven.GetItem(_item);

        database.UnEquipItem(_item);

        SetSlotCount(-1);
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
    // 드레그 끝난 위치가 스텟창이나 인벤창 밖이면 아이템 드롭창 불러줌.
    public void OnEndDrag(PointerEventData eventData)
    {
        // 활성화된 창만 계산되게 if문 넣어줌.
        if (invenBase.gameObject.activeSelf && statusBase.gameObject.activeSelf)
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

        if (transform.parent.CompareTag("QUICKSLOT"))
            if (DragSlot.instance.dragSlot != null)
                ChangeSlot();

        if (item != null)
            database.ShowToolTip(item);
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
            database.ShowToolTip(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        database.HideToolTip();
    }
}
