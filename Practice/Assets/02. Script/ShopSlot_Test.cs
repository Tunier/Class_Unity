using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopSlot_Test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    PlayerInfo player;
    [SerializeField]
    Shop_Test shop;
    [SerializeField]
    Inventory inven;
    [SerializeField]
    ShopMessage shopMessage;
    [SerializeField]
    Text itemName;
    [SerializeField]
    Text itemPrice;

    public Item item;
    public Image itemImage;
    public Image slot_BG;

    int itemCount;

    Tooltip tooltip;

    const string defalt_EquipmentSlotBG_Path = "UI/Tooltip/TooltipBackground";
    const string defalt_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_alphaFront";
    const string common_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_white";
    const string rare_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_cyan";
    const string unique_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_pink";
    const string epic_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_yellow";
    const string set_SlotBG_Path = "UI/Inventory/Slot_Frame/itemFrame_green";

    private void Start()
    {
        tooltip = FindObjectOfType<Tooltip>();
    }

    public void AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = Resources.Load<Sprite>(_item.ItemImagePath);
        itemName.text = item.Name;
        itemPrice.text = item.BuyCost.ToString();
        itemCount = 1;
        SetColorAlpha(1);
        switch (_item.Rarity)
        {
            case 0: // 일반
                slot_BG.sprite = Resources.Load<Sprite>(common_SlotBG_Path);
                break;
            case 1: // 레어
                slot_BG.sprite = Resources.Load<Sprite>(rare_SlotBG_Path);
                break;
            case 2: // 유니크
                slot_BG.sprite = Resources.Load<Sprite>(unique_SlotBG_Path);
                break;
            case 3: // 에픽
                slot_BG.sprite = Resources.Load<Sprite>(epic_SlotBG_Path);
                break;
            case 4: // 세트
                slot_BG.sprite = Resources.Load<Sprite>(set_SlotBG_Path);
                break;
        }
    }
    void SetColorAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemCount != 0)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (player.stats.Gold >= item.BuyCost)
                {
                    shopMessage.ShowMessageTxt(item, 0);
                }
                else
                {
                    shopMessage.ShowMessageTxt(item, 2);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift))
            {
                if (item.Type == 9 || item.Type == 10)
                {
                    if (player.stats.Gold >= item.BuyCost)
                    {
                        shopMessage.ShowQuantityTxt(item, 0);
                    }
                    else
                    {
                        shopMessage.ShowMessageTxt(item, 2);
                    }
                }
                else
                {
                    if (player.stats.Gold >= item.BuyCost)
                    {
                        shopMessage.ShowMessageTxt(item, 0);
                    }
                    else
                    {
                        shopMessage.ShowMessageTxt(item, 2);
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (shop.isBuying)
                    shopMessage.SellItem(item);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemCount != 0)
            {
                DragSlot.instance.shopSlot = this;
                DragSlot.instance.DragSetImage(itemImage);
                DragSlot.instance.SetActiveOutLine(true);
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
                DragSlot.instance.SetActiveOutLine(false);
                //SellItem(item);
                //판매 메세지
                shop.isBuying = true;
                shop.isSelling = false;

                if (DragSlot.instance.shopSlot.item.BuyCost > player.stats.Gold)
                    shopMessage.ShowMessageTxt(item, 2);
                else
                    shopMessage.ShowMessageTxt(item, 0);
            }
            else
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.SetActiveOutLine(false);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemCount != 0)
        {
            tooltip.ShowTooltip(item);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
