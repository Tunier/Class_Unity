using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Test : MonoBehaviour
{
    [SerializeField]
    GameObject shopBase;
    [SerializeField]
    GameObject slotsGroup;
    [SerializeField]
    GameObject invenBase;

    public List<ShopSlot_Test> slots = new List<ShopSlot_Test>();

    public bool isBuying = false;
    public bool isSelling = false;

    Texture2D buyIcon;
    Texture2D sellIcon;
    void Start()
    {
        slots.AddRange(slotsGroup.GetComponentsInChildren<ShopSlot_Test>());
        buyIcon = Resources.Load<Texture2D>("UI/Curser/Cursor_Flask");
        sellIcon = Resources.Load<Texture2D>("UI/Curser/G_Cursor_Flask");
        
        #region 테스트 코드
        slots[0].AddItem(ItemDatabase.instance.newItem("0000008"));
        slots[1].AddItem(ItemDatabase.instance.newItem("0000009"));
        slots[2].AddItem(ItemDatabase.instance.newItem("0000010"));
        slots[3].AddItem(ItemDatabase.instance.newItem("0000011"));
        slots[4].AddItem(ItemDatabase.instance.newItem("0000012"));
        slots[5].AddItem(ItemDatabase.instance.newItem("0000013"));
        slots[6].AddItem(ItemDatabase.instance.newItem("0000000"));
        slots[7].AddItem(ItemDatabase.instance.newItem("0000004"));
        slots[8].AddItem(ItemDatabase.instance.newItem("0000001"));
        slots[9].AddItem(ItemDatabase.instance.newItem("0000005"));
        slots[10].AddItem(ItemDatabase.instance.newItem("0000002"));
        slots[11].AddItem(ItemDatabase.instance.newItem("0000006"));
        #endregion
    }

    void Update()
    {
        if (shopBase.activeSelf) 
        {
            if (isBuying || isSelling) 
            {
                if (Input.GetMouseButtonDown(1)) 
                {
                    isBuying = false;
                    isSelling = false;
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }
        else // 상점창이 꺼지면 두 기능다 꺼줌
        {
            isBuying = false;
            isSelling = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OnClickBuyButton()
    {
        isBuying = true;
        isSelling = false;
        Cursor.SetCursor(buyIcon, new Vector2(buyIcon.width / 5, 0), CursorMode.Auto);
    }

    public void OnClickSellButton()
    {
        isSelling = true;
        isBuying = false;
        Cursor.SetCursor(sellIcon, new Vector2(sellIcon.width / 5, 0), CursorMode.Auto);
    }
    public void OnClickExitButton()
    {
        shopBase.SetActive(false);
        invenBase.SetActive(false);
    }
} 
