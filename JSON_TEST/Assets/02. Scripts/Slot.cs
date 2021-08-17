using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    UIManager UIMgr;

    public int index;
    public Item item;
    public Image itemImage;
    public bool haveItem = false;

    [SerializeField]
    GameObject countImage;
    [SerializeField]
    Text countText;

    private void Awake()
    {
        UIMgr = FindObjectOfType<UIManager>();
    }

    public void SetColorAlpha(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int count = 1)
    {
        item = _item;
        item.Count = count;
        item.SlotIndex = index;
        itemImage.sprite = Resources.Load<Sprite>(_item.ItemImagePath);
        SetColorAlpha(1);
        haveItem = true;

        if (item.Type == 9 || item.Type == 10)
        {
            countImage.SetActive(true);
            countText.text = item.Count.ToString();
        }
        else
        {
            countText.text = "0";
            countImage.SetActive(false);
        }
    }

    /// <summary>
    /// 해당슬롯의 아이템의 카운트를 인수값 만큼 더해줌. 슬롯에 있는 아이템의 카운트가 0이되면 ClearSlot 발동.
    /// </summary>
    /// <param name="count"></param>
    public void SetSlotCount(int count)
    {
        item.Count += count;
        countText.text = item.Count.ToString();

        if (item.Count <= 0)
        {
            ClearSlot();
        }
    }

    /// <summary>
    /// 해당칸을 비워줌. (item = null, itemCount = 0, 스프라이트 = null, 알파값 0으로)
    /// </summary>
    void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        SetColorAlpha(0);
        haveItem = false;

        countText.text = "0";
        countImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            SetSlotCount(-1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item.Count != 0)
            UIMgr.UpdateToolTip(item);
    }
}
