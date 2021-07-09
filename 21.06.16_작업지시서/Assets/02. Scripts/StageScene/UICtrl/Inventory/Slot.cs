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

    Rect baseRect; // InventoryBase�� �̹��� Rect ���� ������.

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

    // ���� ��Ŭ���� ���Կ� �ִ� ������ Ÿ���� ���� �Ҹ�ǰ�̸� ������ ���
    // ���� �������ͽ�â�� �ش� ���Ÿ��ĭ�� ����.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (transform.parent.CompareTag("INVENTORY")) // �κ��丮�� �ִ� ����.
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

            if (transform.parent.CompareTag("STATUS")) // �������ͽ� â�� �ִ� ����.
                if (item != null)
                {
                    UnEquipItem(item);
                }
        }
    }

    // �巹�� ���۽� �巹�� ������ ���Կ� �ִ� ������ �巹�� ���Կ� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;

        }
    }

    // �巹�� �ϴ� ���� ������ ����
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    // �巹�� ������ �巹�� ���� ���İ� 0���� �ٲٰ� �����
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

    // �巹���ؼ� ���������� ���Եΰ� ���빰�� �ٲ���, ��������� �־��.
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
