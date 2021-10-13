using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMessage : MonoBehaviour
{
    [SerializeField]
    PlayerInfo player;
    [SerializeField]
    Inventory inven;
    [SerializeField]
    Slot slot;
    [SerializeField]
    Shop_Test shop;
    [SerializeField]
    Text messageTxt;
    [SerializeField]
    GameObject messageBackground;
    [SerializeField]
    GameObject quantityMessage;
    [SerializeField]
    Text quantityTxt;

    public InputField inputField;

    [SerializeField]
    Item item;
    Slot curSlot;
    int selectNum;
    int count = 0;
    int lastCount;

    private void Update()
    {
        if (curSlot != null)
            CheckCount(item);

        if (inputField.text != "")
            lastCount = int.Parse(inputField.text);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (messageBackground.activeSelf || quantityMessage.activeSelf)
                OnClickNoButton();
        }
    }

    public void OnClilckYesButton()
    {
        switch (selectNum)
        {
            case 0:
                SellItem(item);
                messageBackground.SetActive(false);
                break;
            case 1:
                DragSlot.instance.dragSlot.SellItem(DragSlot.instance.dragSlot.item);
                curSlot = null;
                messageBackground.SetActive(false);
                break;
            case 2:
                messageBackground.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnClickQuantityYes()
    {
        switch (selectNum)
        {
            case 0:
                SellItem(item, lastCount);
                quantityMessage.SetActive(false);
                break;
            case 1:
                if (DragSlot.instance.dragSlot != null)
                {
                    player.stats.Gold += DragSlot.instance.dragSlot.item.SellCost * lastCount;

                    DragSlot.instance.dragSlot.SetSlotCount(-lastCount);
                    DragSlot.instance.SetColorAlpha(0);
                    DragSlot.instance.dragSlot = null;
                }
                else
                {
                    player.stats.Gold += item.SellCost * lastCount;
                    curSlot.SetSlotCount(-lastCount);
                }
                curSlot = null;
                quantityMessage.SetActive(false);
                break;
            case 2:
                quantityMessage.SetActive(false);
                break;
        }
    }
    public void OnClickNoButton()
    {
        messageBackground.SetActive(false);
        quantityMessage.SetActive(false);
    }

    public void ShowMessageTxt(Item _item, int _num)
    {
        item = _item;
        selectNum = _num;
        switch (_num)
        {
            case 0:
                messageTxt.text = "구매 하시겠습니까?";
                break;
            case 1:
                messageTxt.text = "판매 하시겠습니까?";
                break;
            case 2:
                messageTxt.text = "골드가 부족합니다.";
                break;
            default:
                break;
        }

        messageBackground.SetActive(true);
    }

    public void ShowQuantityTxt(Item _item, int _num, Slot _slot = null)
    {
        quantityMessage.SetActive(true);
        item = _item;
        selectNum = _num;
        if (_slot != null)
            curSlot = _slot;

        switch (_num)
        {
            case 0:
                quantityTxt.text = "구매 수량";
                break;
            case 1:
                quantityTxt.text = "판매 수량";
                inputField.text = 0.ToString();
                break;
            case 2:
                break;
        }
    }

    public void SellItem(Item _item, int _count = 1)
    {
        if (player.stats.Gold >= _item.BuyCost * _count)
        {
            player.stats.Gold -= _item.BuyCost * _count;
            inven.GetItem(_item, _count);
        }
        else
        {
            ShowMessageTxt(item, 2);
        }
    }

    public void CheckCount(Item _item)
    {
        switch (selectNum)
        {
            case 0:
                if (_item.BuyCost != 0)
                    count = player.stats.Gold / _item.BuyCost;

                if (inputField.text != "")
                {
                    if (count < int.Parse(inputField.text))
                    {
                        inputField.text = count.ToString();
                        lastCount = count;
                    }
                    else
                    {
                        lastCount = int.Parse(inputField.text);
                    }
                }
                break;
            case 1:
                count = curSlot.itemCount;

                if (inputField.text != "")
                {
                    if (count < int.Parse(inputField.text))
                    {
                        inputField.text = count.ToString();
                    }
                    else
                    {
                        lastCount = int.Parse(inputField.text);
                    }
                }
                else if (inputField.text == "")
                {
                    inputField.text = "0";
                }
                break;
        }
    }
}
