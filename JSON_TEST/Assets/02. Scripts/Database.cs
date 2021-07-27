using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Weapon,
        Used,
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Epic,
    }

    public Item(int _Index, ItemType _Type, string _Name, ItemRarity _Rarity, int _SellCost, int _BuyCost, string _ItemImagePath)
    {
        Index = _Index;
        Type = _Type;
        Name = String.Format(_Name, Type);
        Rarity = _Rarity;
        SellCost = _SellCost;
        BuyCost = _BuyCost;
        ItemImagePath = _ItemImagePath;
    }

    public int Index;
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemType Type;
    public string Name;
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemRarity Rarity;
    public int SellCost;
    public int BuyCost;
    public int Count;
    public int SlotIndex;

    public string ItemImagePath;
}

public class Database : MonoBehaviour
{
    public static Database instance = null;

    public TextAsset ItemData;
    public List<Item> AllItemList;

    public List<Item> LoadItemList;

    [SerializeField]
    Inventory inven;

    const string itemDataPath = "Data/ItemData";
    const string invenSavePath = "/Resources/Data/MyInvenItems.text";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        ItemData = Resources.Load<TextAsset>(itemDataPath);

        string[] line = ItemData.text.Substring(0, ItemData.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            Item.ItemType type = new Item.ItemType();
            Item.ItemRarity rarity = new Item.ItemRarity();

            string[] row = line[i].Split('\t');

            if (Enum.IsDefined(typeof(Item.ItemType), row[1]))
                type = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), row[1], true);
            else
                Debug.Log("아이템 타입 오류");

            if (Enum.IsDefined(typeof(Item.ItemRarity), row[3]))
                rarity = (Item.ItemRarity)Enum.Parse(typeof(Item.ItemRarity), row[3], true);
            else
                Debug.Log("아이템 레어리티 오류");

            row[6] = row[6].Substring(0, row[6].Length - 1); // 마지막 한글자 안잘라주면 경로가 이상하게 잡힘.

            AllItemList.Add(new Item(int.Parse(row[0]), type, row[2], rarity, int.Parse(row[4]), int.Parse(row[5]), row[6]));
        }
    }

    public void Save()
    {
        inven.SaveInven();
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + invenSavePath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + invenSavePath);
            LoadItemList = JsonConvert.DeserializeObject<List<Item>>(Jdata);
            inven.LoadInven(LoadItemList);

            Debug.Log("로드성공");
        }
        else
            Debug.Log("세이브파일없음");
    }
}
