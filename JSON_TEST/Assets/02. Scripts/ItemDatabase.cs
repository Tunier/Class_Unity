using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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

    public Item(int _Index, ItemType _Type, string _Name, ItemRarity _Rarity, int _Value, int _BuyCost, int _SellCost, string _ItemImagePath)
    {
        Index = _Index;
        Type = _Type;
        Name = _Name;
        Rarity = _Rarity;
        Value = _Value;
        BuyCost = _BuyCost;
        SellCost = _SellCost;
        ItemImagePath = _ItemImagePath;
    }

    public int Index;
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemType Type;
    public string Name;
    [JsonConverter(typeof(StringEnumConverter))]
    public ItemRarity Rarity;
    public int Value;
    public int SellCost;
    public int BuyCost;
    public int Count;
    public int SlotIndex;

    public string ItemImagePath;
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance = null;

    public List<Item> AllItemList = new List<Item>();
    public List<Item> LoadItemList = new List<Item>();

    public Dictionary<int, Item> AllItemDic = new Dictionary<int, Item>();

    [SerializeField]
    Inventory inven;

    const string itemDataPath = "/Resources/Data/All_Item_Data.text";
    const string invenSavePath = "/Resources/Data/MyInvenItems.text";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        #region txt to json
        //ItemData = Resources.Load<TextAsset>(itemDataPath);

        //string[] line = ItemData.text.Substring(0, ItemData.text.Length - 1).Split('\n');
        //for (int i = 0; i < line.Length; i++)
        //{
        //    Item.ItemType type = new Item.ItemType();
        //    Item.ItemRarity rarity = new Item.ItemRarity();

        //    string[] row = line[i].Split('\t');

        //    if (Enum.IsDefined(typeof(Item.ItemType), row[1]))
        //        type = (Item.ItemType)Enum.Parse(typeof(Item.ItemType), row[1], true);
        //    else
        //        Debug.Log("������ Ÿ�� ����");

        //    if (Enum.IsDefined(typeof(Item.ItemRarity), row[3]))
        //        rarity = (Item.ItemRarity)Enum.Parse(typeof(Item.ItemRarity), row[3], true);
        //    else
        //        Debug.Log("������ ���Ƽ ����");

        //    row[6] = row[6].Substring(0, row[6].Length - 1); // ������ �ѱ��� ���߶��ָ� ��ΰ� �̻��ϰ� ����.

        //    AllItemList.Add(new Item(int.Parse(row[0]), type, row[2], rarity, int.Parse(row[4]), int.Parse(row[5]), row[6]));
        //}
        #endregion

        #region Json ������ ������ �޾ƿͼ� ��ųʸ� ����Ʈ�� �����ϱ�.
        if (File.Exists(Application.dataPath + itemDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + itemDataPath);
            AllItemList = JsonConvert.DeserializeObject<List<Item>>(Jdata);
            Debug.Log("�����۵����� �ε强��.");
        }
        else
            Debug.LogWarning("������ �����ϴ�.");

        for (int i = 0; i < AllItemList.Count; i++)
        {
            AllItemDic.Add(AllItemList[i].Index, AllItemList[i]);
        }
        #endregion
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

            Debug.Log("�κ��丮 �ε强��");
        }
        else
            Debug.Log("���̺����Ͼ���");
    }

    public Item newItem(int i)
    {
        int Value;

        var randomItemQuality = UnityEngine.Random.Range(1, 1000);

        if (AllItemDic[i].Type == Item.ItemType.Weapon)
        {
            if (randomItemQuality > 750)
                Value = Mathf.RoundToInt(AllItemDic[i].Value * 1.1f);
            else if (randomItemQuality > 250)
                Value = AllItemDic[i].Value;
            else
                Value = Mathf.RoundToInt(AllItemDic[i].Value * 0.9f);
        }
        else
        {
            Value = AllItemDic[i].Value;
        }

        var item = new Item(AllItemDic[i].Index,
                            AllItemDic[i].Type,
                            AllItemDic[i].Name,
                            AllItemDic[i].Rarity,
                            Value,
                            AllItemDic[i].BuyCost,
                            AllItemDic[i].SellCost,
                            AllItemDic[i].ItemImagePath);

        return item;
    }
}
