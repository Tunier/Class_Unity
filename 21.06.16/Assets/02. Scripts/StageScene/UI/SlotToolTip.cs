using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    public GameObject baseImage;

    [SerializeField]
    Text itemName;
    [SerializeField]
    Text itemDesc;
    [SerializeField]
    Text itemHouToUse;

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
        RD_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, -baseImage.GetComponent<RectTransform>().rect.height * 0.5f, 0); // 오른쪽 아래로 띄우는 오프셋
        RU_Offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, baseImage.GetComponent<RectTransform>().rect.height * 0.5f, 0); // 오른쪽 위로 띄우는 오프셋
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

        itemName.text = _item.itemName;
        itemDesc.text = _item.itemDescription;

        switch (_item.itemRarelity)
        {
            case Item.ItemRarelity.Common:
                SetitemNameColor(Color.white);
                break;
            case Item.ItemRarelity.Rare:
                SetitemNameColor(Color.blue);
                break;
            case Item.ItemRarelity.Epic:
                SetitemNameColor(Color.yellow);
                break;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, Input.mousePosition))
        {
            if (_item.itemType == Item.ItemType.Equipment)
            {
                itemHouToUse.text = "우클릭 - 장착";
            }
            else if (_item.itemType == Item.ItemType.Used)
            {
                itemHouToUse.text = "우클릭 - 사용";
            }
            else
            {
                itemHouToUse.text = "";
            }
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(statusBase, Input.mousePosition))
        {
            itemHouToUse.text = "우클릭 - 장착해제";
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(quickSlotBase, Input.mousePosition))
        {
            if (_item.itemType == Item.ItemType.Equipment)
            {
                itemHouToUse.text = "우클릭 - 장착";
            }
            else if (_item.itemType == Item.ItemType.Used)
            {
                itemHouToUse.text = "우클릭 - 사용";
            }
            else
            {
                itemHouToUse.text = "";
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

    public void SetitemNameColor(Color _color)
    {
        itemName.color = _color;
    }

    public void SetitemDescColor(Color _color)
    {
        itemDesc.color = _color;
    }
    public void SetitemHouToUseColor(Color _color)
    {
        itemHouToUse.color = _color;
    }
}
