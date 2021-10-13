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
    // 0: �Ѽչ���, 1: �μչ���, 2: ���, 3: ���� , 4: ��Ʈ, 5:�尩
    // 6: �Ź�, 7: �����, 8: ����, 9: �Һ�, 10: ���

    public int Rarity;
    // 0: �Ϲ�, 1: ����, 2: ����ũ, 3: ����, 4: ��Ʈ

    public int BuyCost;
    public int SellCost;
    public string ItemImagePath;

    public int SlotIndex; // ���̺�� ������
    public int Count; // ���̺�� ������

    public ItemEffect itemEffect = new ItemEffect();
}

[System.Serializable]
public class ItemEffect
{
    public string UIDCODE;
    public string Value;
    public string ValueType;
    // 0: ����, 1: ���� Hp, 2: ���� Mp, 3: �ִ� Hp ������, 4: �ִ� Hp %��, 5: �ִ� Mp ������, 6: �ִ� Mp %��,
    // 7: ���� ���ݷ� ������, 8: ���� ���ݷ� %��, 9: ���� ���� ������, 10: ���� ���� %��, 11: �� ������, 12: �� %��,
    // 13: ���� ������, 14: ���� %��, 15: ���ݽ� ����� ȸ�� ������, 16: ���ݽ� �������� %��ŭ ����� ȸ��,
    // 17: ���� ���ݷ� ������, 18: ���� ���ݷ� %��, 19: ���� ���� ������, 20: ���� ���� %��

    public Dictionary<int, float> ValueDic = new Dictionary<int, float>();
    public string RequireValue;
    public string RequireValueType;
    // ValueType�� ������.

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
        //        Debug.Log("������ Ÿ�� ����");

        //    if (Enum.IsDefined(typeof(Item.ItemRarity), row[3]))
        //        rarity = (Item.ItemRarity)Enum.Parse(typeof(Item.ItemRarity), row[3], true);
        //    else
        //        Debug.Log("������ ���Ƽ ����");

        //    row[6] = row[6].Substring(0, row[6].Length - 1); // ������ �ѱ��� ���߶��ָ� ��ΰ� �̻��ϰ� ����.

        //    AllItemList.Add(new Item(int.Parse(row[0]), type, row[2], rarity, int.Parse(row[4]), int.Parse(row[5]), row[6]));
        //}
        #endregion

        #region Json ������ ������, ������ȿ�� ������ �޾ƿͼ� ��ųʸ� ����Ʈ�� �����ϱ�.
        if (File.Exists(Application.dataPath + itemDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + itemDataPath);
            AllItemList = JsonConvert.DeserializeObject<List<Item>>(Jdata);
            Debug.Log("�����۵����� �ε强��.");
        }
        else
            Debug.LogWarning("�����۵����������� �����ϴ�.");

        if (File.Exists(Application.dataPath + itemEffectDataPath))
        {
            string Jdata = File.ReadAllText(Application.dataPath + itemEffectDataPath);
            AllItemEffectList = JsonConvert.DeserializeObject<List<ItemEffect>>(Jdata);
            Debug.Log("������ȿ�������� �ε强��.");
        }
        else
            Debug.LogWarning("������ȿ�������������� �����ϴ�.");

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
    /// UID�ڵ带 �Ű������� �޾Ƽ� �� �������� ��������.
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

        // ���߿� ������ ȿ�� ������� ������ ���������� ���� �ɼ� Ǯ�� ������, ����Ƽ�� ���� �ɼ� ������
        // �ɼ��� ��ġ�� �����ǵ��� �ڵ带 ���ľ���.

        return item;
    }
}