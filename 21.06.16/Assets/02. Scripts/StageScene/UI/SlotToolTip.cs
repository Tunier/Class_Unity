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

    public Vector3 offset;

    private void Awake()
    {
        offset = new Vector3(baseImage.GetComponent<RectTransform>().rect.width * 0.5f, -baseImage.GetComponent<RectTransform>().rect.height * 0.5f, 0);
    }

    private void Update()
    {
        if (baseImage.activeSelf)
            baseImage.transform.position = Input.mousePosition + offset;
    }

    public void ShowToolTip(Item _item)
    {
        baseImage.SetActive(true);

        itemName.text = _item.itemName;
        itemDesc.text = _item.itemDescription;

        if (RectTransformUtility.RectangleContainsScreenPoint(invenBase, Input.mousePosition))
        {
            if (_item.itemType == Item.ItemType.Equipment)
            {
                itemHouToUse.text = "��Ŭ�� - ����";
            }
            else if (_item.itemType == Item.ItemType.Used)
            {
                itemHouToUse.text = "��Ŭ�� - ���";
            }
            else
            {
                itemHouToUse.text = "";
            }
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(statusBase, Input.mousePosition))
        {
            itemHouToUse.text = "��Ŭ�� - ��������";
        }
    }

    public void HideToolTip()
    {
        baseImage.SetActive(false);
    }
}
