using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int index;
    public Item item;
    public Image itemImage;
    public bool haveItem = false;

    [SerializeField]
    GameObject countImage;
    [SerializeField]
    Text countText;

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

        if (item.Type != Item.ItemType.Weapon)
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
    /// �ش罽���� �������� ī��Ʈ�� �μ��� ��ŭ ������. ���Կ� �ִ� �������� ī��Ʈ�� 0�̵Ǹ� ClearSlot �ߵ�.
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
    /// �ش�ĭ�� �����. (item = null, itemCount = 0, ��������Ʈ = null, ���İ� 0����)
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
}
