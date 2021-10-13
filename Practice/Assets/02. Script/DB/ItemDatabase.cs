using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string UIDCODE;
    public string Name;
    public int Type;
    // 0: 한손무기, 1: 두손무기, 2: 헬멧, 3: 갑옷 , 4: 벨트, 5:장갑
    // 6: 신발, 7: 목걸이, 8: 반지, 9: 소비, 10: 재료

    public int Rarity;
    // 0: 일반, 1: 레어, 2: 유니크, 3: 에픽, 4: 세트

    public int BuyCost;
    public int SellCost;
    public string ItemImagePath;

    public int SlotIndex; // 세이브용 데이터
    public int Count; // 세이브용 데이터

    public ItemEffect itemEffect = new ItemEffect();
}

[System.Serializable]
public class ItemEffect
{
    public string UIDCODE;
    public string Value;
    public string ValueType;
    // 0: 없음, 1: 현재 Hp, 2: 현재 Mp, 3: 최대 Hp 고정값, 4: 최대 Hp %값, 5: 최대 Mp 고정값, 6: 최대 Mp %값,
    // 7: 물리 공격력 고정값, 8: 물리 공격력 %값, 9: 물리 방어력 고정값, 10: 물리 방어력 %값, 11: 힘 고정값, 12: 힘 %값,
    // 13: 지능 고정값, 14: 지능 %값, 15: 공격시 생명령 회복 고정값, 16: 공격시 데미지의 %만큼 생명력 회복,
    // 17: 마법 공격력 고정값, 18: 마법 공격력 %값, 19: 마법 방어력 고정값, 20: 마법 방어력 %값

    public Dictionary<int, float> ValueDic = new Dictionary<int, float>();
    public string RequireValue;
    public string RequireValueType;
    // ValueType과 동일함.

    public Dictionary<int, float> RequireValueDic = new Dictionary<int, float>();
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

    public Text debugText;

    const string itemDataPath = "/Resources/Data/All_Item_Data.text";
    const string itemEffectDataPath = "/Resources/Data/All_Item_Effect_Data.text";
    const string invenSavePath = "/Resources/Data/MyInvenItems.text";

    public const int numberOfItemeffects = 18;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
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

            for (int j = 0; j < row2.Length; j++)
            {
                AllItemEffectDic[AllItemEffectList[i].UIDCODE].ValueDic.Add(int.Parse(row2[j]), float.Parse(row[j]));
            }

            string[] row3 = AllItemEffectDic[AllItemEffectList[i].UIDCODE].RequireValue.Split('/');
            string[] row4 = AllItemEffectDic[AllItemEffectList[i].UIDCODE].RequireValueType.Split('/');

            for (int j = 0; j < row4.Length; j++)
            {
                AllItemEffectDic[AllItemEffectList[i].UIDCODE].RequireValueDic.Add(int.Parse(row4[j]), float.Parse(row3[j]));
            }
        }
        #endregion
    }

    /// <summary>
    /// UID코드를 매개변수로 받아서 새 아이템을 생성해줌.
    /// </summary>
    /// <param name="_UIDCODE"></param>
    /// <returns></returns>
    public Item newItem(string _UIDCODE)
    {
        var item = new Item();

        item.UIDCODE = AllItemDic[_UIDCODE].UIDCODE;
        item.Type = AllItemDic[_UIDCODE].Type;
        item.Name = AllItemDic[_UIDCODE].Name;
        item.Rarity = AllItemDic[_UIDCODE].Rarity;
        item.BuyCost = AllItemDic[_UIDCODE].BuyCost;
        item.SellCost = AllItemDic[_UIDCODE].SellCost;
        item.ItemImagePath = AllItemDic[_UIDCODE].ItemImagePath;

        //var randomItemQuality = UnityEngine.Random.Range(1, 10000);

        item.itemEffect.ValueDic = AllItemEffectDic[item.UIDCODE].ValueDic;
        item.itemEffect.RequireValueDic = AllItemEffectDic[item.UIDCODE].RequireValueDic;

        // 나중에 아이템 효과 같은경우 아이템 종류에따라 랜덤 옵션 풀을 가지고, 퀄리티에 따라 옵션 갯수와
        // 옵션의 수치가 결정되도록 코드를 고쳐야함.

        return item;
    }
}