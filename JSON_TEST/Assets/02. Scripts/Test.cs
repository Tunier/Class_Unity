using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Test : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Inventory inven;

    PlayerTest player;
    SkillDatabase skillDB;

    public int i;

    void Start()
    {
        player = FindObjectOfType<PlayerTest>();
        skillDB = FindObjectOfType<SkillDatabase>();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GetItem(i);
        }
    }

    public void GetItem(int i)
    {
        //var item = new Item(Database.instance.AllItemList[i].Index,
        //                    Database.instance.AllItemList[i].Type,
        //                    Database.instance.AllItemList[i].Name,
        //                    Database.instance.AllItemList[i].Rarity,
        //                    Database.instance.AllItemList[i].BuyCost,
        //                    Database.instance.AllItemList[i].SellCost,
        //                    Database.instance.AllItemList[i].ItemImagePath);
        //inven.GetItem(item);

        var item = ItemDatabase.instance.newItem(i);

        inven.GetItem(item);
    }
}