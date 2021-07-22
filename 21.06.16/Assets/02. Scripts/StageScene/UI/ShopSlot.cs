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
    /// 상점입장에서 아이템을 판매하면(플레이어가 아이템을 사면), 플레이어의 골드가 가격이상 있는지 확인하고 있으면<br/>
    /// 플레이어의 골드가 아이템의 구매가만큼 감소. 상점의 슬롯 비워주고, 아이템을 인벤에 넣어줌.<br/>
    /// 없으면 "골드가 모자랍니다." 출력.
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
            StartCoroutine(UIManager.instance.PrintActionText("골드가 모자랍니다."));
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
