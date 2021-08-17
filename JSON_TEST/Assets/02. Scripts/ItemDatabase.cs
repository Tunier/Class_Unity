using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string UIDCODE;
    public string Name;
    public int Type;
    public int Rarity;
    public int BuyCost;
    public int SellCost;
    public string ItemImagePath;

    public int SlotIndex;
    public int Count;

    public ItemEffect itemEffect = new ItemEffect();
}

[System.Serializable]
public class ItemEffect
{
    public string UIDCODE;
    public string Value;
    public string ValueType;
    public List<float> f_Value = new List<float>();
    public List<int> i_ValueType = new List<int>();
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance = null;

    public List<Item> AllItemList = new List<Item>();
    public List<Item> LoadItemList = new List<Item>();

    public List<ItemEffect> AllItemEffectList = new List<ItemEffect>();

    public Dictionary<string, Item> AllItemDic = new Dictionary<string, Item>();
    public Dictionary<string, ItemEffect> AllItemEffectDic = new Dictionary<string, ItemEffect>();

    [SerializeField]
    Inventory inven;

    const string itemDataPath = "/Resources/Data/All_Item_Data.text";
    const string itemEffectDataPath = "/Resources/Data/All_Item_Effect_Data.text";
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
        //        Debug.Log("아이템 타입 오류");

        //    if (Enum.IsDefined(typeof(Item.ItemRarity), row[3]))
        //        rarity = (Item.ItemRarity)Enum.Parse(typeof(Item.ItemRarity), row[3], true);
        //    else
        //        Debug.Log("아이템 레어리티 오류");

        //    row[6] = row[6].Substring(0, row[6].Length - 1); // 마지막 한글자 안잘라주면 경로가 이상하게 잡힘.

        //    AllItemList.Add(new Item(int.Parse(row[0]), type, row[2], rarity, int.Parse(row[4]), int.Parse(row[5]), row[6]));
        //}
        #endregion

        #region Json 아이템 데이터, 아이템효과 데이터 받아와서 딕셔너리 리스트에 저장하기.
        if (File.Exists(Application.dataPath + itemDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + itemDataPath);
            AllItemList = JsonConvert.DeserializeObject<List<Item>>(Jdata);
            Debug.Log("아이템데이터 로드성공.");
        }
        else
            Debug.LogWarning("아이템데이터파일이 없습니다.");

        if (File.Exists(Application.dataPath + itemEffectDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + itemEffectDataPath);
            AllItemEffectList = JsonConvert.DeserializeObject<List<ItemEffect>>(Jdata);
            Debug.Log("아이템효과데이터 로드성공.");
        }
        else
            Debug.LogWarning("아이템효과데이터파일이 없습니다.");

        for (int i = 0; i < AllItemList.Count; i++)
        {
            AllItemDic.Add(AllItemList[i].UIDCODE, AllItemList[i]);
        }

        for (int i = 0; i < AllItemEffectList.Count; i++)
        {
            AllItemEffectDic.Add(AllItemEffectList[i].UIDCODE, AllItemEffectList[i]);

            string[] row = AllItemEffectDic[AllItemEffectList[i].UIDCODE].Value.Split('/');
            string[] row2 = AllItemEffectDic[AllItemEffectList[i].UIDCODE].ValueType.Split('/');

            for (int j = 0; j < row.Length; j++)
            {
                AllItemEffectDic[AllItemEffectList[i].UIDCODE].f_Value.Add(float.Parse(row[j]));
                AllItemEffectDic[AllItemEffectList[i].UIDCODE].i_ValueType.Add(int.Parse(row2[j]));
            }
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

            Debug.Log("인벤토리 로드성공");
        }
        else
            Debug.Log("세이브파일없음");
    }

    public Item newItem(string _s)
    {
        var item = new Item();

        item.UIDCODE = AllItemDic[_s].UIDCODE;
        item.Type = AllItemDic[_s].Type;
        item.Name = AllItemDic[_s].Name;
        item.Rarity = AllItemDic[_s].Rarity;
        item.BuyCost = AllItemDic[_s].BuyCost;
        item.SellCost = AllItemDic[_s].SellCost;
        item.ItemImagePath = AllItemDic[_s].ItemImagePath;

        var randomItemQuality = UnityEngine.Random.Range(1, 1000);

        if (AllItemDic[_s].Type != 9 && AllItemDic[_s].Type != 10)
        {
            //if (randomItemQuality > 750)
            //{
            //    for (int i = 0; i < AllItemEffectDic[_s].f_Value.Count; i++)
            //        item.itemEffect.f_Value[i] = AllItemEffectDic[item.UIDCODE].f_Value[i] * 1.1f;
            //}
            //else if (randomItemQuality > 250)
            //{
            //    for (int i = 0; i < AllItemEffectDic[_s].f_Value.Count; i++)
            //        item.itemEffect.f_Value[i] = AllItemEffectDic[item.UIDCODE].f_Value[i];
            //}
            //else
            //{
            //    for (int i = 0; i < AllItemEffectDic[_s].f_Value.Count; i++)
            //        item.itemEffect.f_Value[i] = AllItemEffectDic[item.UIDCODE].f_Value[i] * 0.9f;
            //}
            item.itemEffect.f_Value = AllItemEffectDic[item.UIDCODE].f_Value;
        }
        else
        {
            item.itemEffect.f_Value = AllItemEffectDic[item.UIDCODE].f_Value;
        }

        item.itemEffect.i_ValueType = AllItemEffectDic[item.UIDCODE].i_ValueType;

        return item;
    }
}
