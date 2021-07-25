using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Inventory inven;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var item = new Item(Database.instance.AllItemList[0].Index,
                                Database.instance.AllItemList[0].Type,
                                Database.instance.AllItemList[0].Name,
                                Database.instance.AllItemList[0].Rarity,
                                Database.instance.AllItemList[0].SellCost,
                                Database.instance.AllItemList[0].BuyCost,
                                Database.instance.AllItemList[0].ItemImagePath);
            inven.GetItem(item);
        }
    }
}
