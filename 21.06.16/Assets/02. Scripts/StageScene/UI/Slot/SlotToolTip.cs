using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    public GameObject baseImage;

    [SerializeField]
    Text itemNameText;
    [SerializeField]
    Text itemDescText;
    [SerializeField]
    Text itemCostText;
    [SerializeField]
    Text itemHouToUseText;

    [SerializeField]
    RectTransform invenBase;
    [SerializeField]
    RectTransform statusBase;
    [SerializeField]
    RectTransform quickSlotBase;

    public Vector3 RD_Offset;
    public Vector3 RU_Offset;

    private void Awake()
    {
        RD_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, -baseImage.GetComponent<RectTransform>().rect.height * 0.5f); // ������ �Ʒ��� ���� ������
        RU_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, baseImage.GetComponent<RectTransform>().rect.height * 0.5f); // ������ ���� ���� ������
    }

    private void Update()
    {
        if (baseImage.activeSelf)
        {
            if (Input.mousePosition.y >= baseImage.GetComponent<RectTransform>().rect.height)
                baseImage.transform.position = Input.mousePosition + RD_Offset;
            else
                baseImage.transform.position = Input.mousePosition + RU_Offset;
        }
    }

    public void ShowToolTip(Item _item)
    {
        baseImage.SetActive(true);

        itemNameText.text = _item.itemName;
        itemDescText.text = _item.itemDescription;

        SetItemNameColor(_item.itemRarelity);

        if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, Input.mousePosition))
        {
            if (_item.itemType == Item.ItemType.Equipment)
            {
                itemHouToUseText.text = "��Ŭ�� - ����";
            }
            else if (_item.itemType == Item.ItemType.Used)
            {
                itemHouToUseText.text = "��Ŭ�� - ���";
            }
            else
            {
                itemHouToUseText.text = "";
            }
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(statusBase, Input.mousePosition))
        {
            itemHouToUseText.text = "��Ŭ�� - ��������";
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, Input.mousePosition))
        {
            if (_item.itemType == Item.ItemType.Equipment)
            {
                itemHouToUseText.text = "��Ŭ�� - ����";
            }
            else if (_item.itemType == Item.ItemType.Used)
            {
                itemHouToUseText.text = "��Ŭ�� - ���";
            }
            else
            {
                itemHouToUseText.text = "";
            }
        }
        else
        {
            return;
        }
    }

    public void HideToolTip()
    {
        baseImage.SetActive(false);
    }

    ///<summary>
    ///SetItemNameColor(������ ���Ƽ)<br/>
    ///�������� ���Ƽ�� ���� ������ �̸� �ؽ�Ʈ�� �÷��� �ٲ���.
    ///</summary>
    public void SetItemNameColor(Item.ItemRarelity itemRarelity)
    {
        Color color;

        switch (itemRarelity)
        {
            case Item.ItemRarelity.Common:
                itemNameText.color = Color.white;
                break;
            case Item.ItemRarelity.Rare:
                ColorUtility.TryParseHtmlString("#0078FF", out color);
                itemNameText.color = color;
                break;
            case Item.ItemRarelity.Epic:
                itemNameText.color = Color.yellow;
                break;
        }
    }

    public void SetItemDescColor(Color _color)
    {
        itemDescText.color = _color;
    }

    public void SetItemHouToUseColor(Color _color)
    {
        itemHouToUseText.color = _color;
    }

    public void SetItemCostText(int cost)
    {
        itemCostText.text = string.Format("{0:N0}", cost) + " Gold";
    }
}
