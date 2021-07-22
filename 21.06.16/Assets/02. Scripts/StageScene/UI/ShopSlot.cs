using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    PlayerCtrl player;
    ItemEffectDatebase database;
    Shop shop;

    public Item item;
    public Image itemImage;

    Inventory inven;

    [SerializeField]
    RectTransform invenBase;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
        inven = FindObjectOfType<Inventory>();
        database = FindObjectOfType<ItemEffectDatebase>();
        shop = FindObjectOfType<Shop>();
    }

    void SetColorAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = item.itemImage;
        SetColorAlpha(1);
    }

    /// <summary>
    /// �������忡�� �������� �Ǹ��ϸ�(�÷��̾ �������� ���), �÷��̾��� ��尡 �����̻� �ִ��� Ȯ���ϰ� ������<br/>
    /// �÷��̾��� ��尡 �������� ���Ű���ŭ ����. ������ ���� ����ְ�, �������� �κ��� �־���.<br/>
    /// ������ "��尡 ���ڶ��ϴ�." ���.
    /// </summary>
    /// <param name="_item"></param>
    public void SellItem(Item _item)
    {
        if (player.gold >= item.buyCost)
        {
            player.gold -= item.buyCost;
            ClearSlot();
            inven.GetItem(_item);
        }
        else
        {
            StartCoroutine(UIManager.instance.PrintActionText("��尡 ���ڶ��ϴ�."));
        }
    }

    void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        SetColorAlpha(0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                SellItem(item);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (shop.isBuying)
                    SellItem(item);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                DragSlot.instance.shopSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (invenBase.gameObject.activeSelf)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, Input.mousePosition))
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.shopSlot = null;

                SellItem(item);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            database.ShowToolTip(item);
            database.SetItemCostText(item.buyCost);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        database.HideToolTip();
    }
}
