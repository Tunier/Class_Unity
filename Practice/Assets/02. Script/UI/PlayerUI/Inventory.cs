using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject inventory_Slot_Parent;
    [SerializeField]
    GameObject eqipment_Slot_Parent;
    [SerializeField]
    GameObject inventory_Base;

    [SerializeField]
    Text goldText;

    [SerializeField]
    PlayerInfo player;
    [SerializeField]
    PlayerActionCtrl playerAC;
    Tooltip tooltip;

    public List<Slot> inventory_Slots = new List<Slot>();
    public List<Slot> Equipment_Slots = new List<Slot>();

    public bool isFull = false;

    public Slot WeaponSlot { get; private set; }
    public Slot HelmetSlot { get; private set; }
    public Slot ArmorSlot { get; private set; }
    public Slot BeltSlot { get; private set; }
    public Slot BootsSlot { get; private set; }
    public Slot GlovesSlot { get; private set; }
    public Slot NecklaceSlot { get; private set; }
    public Slot RingSlot { get; private set; }


    private void Awake()
    {
        inventory_Slots.AddRange(inventory_Slot_Parent.GetComponentsInChildren<Slot>()); // 칠드런으로 받아오는 이유는 비활성화 되면 못받아오기때문에 비활성화된 상태도 찾아올수있는 칠드런으로 받아온다.
        Equipment_Slots.AddRange(eqipment_Slot_Parent.GetComponentsInChildren<Slot>());
        tooltip = FindObjectOfType<Tooltip>();

        WeaponSlot = Equipment_Slots[0];
        HelmetSlot = Equipment_Slots[1];
        ArmorSlot = Equipment_Slots[2];
        BeltSlot = Equipment_Slots[3];
        BootsSlot = Equipment_Slots[4];
        GlovesSlot = Equipment_Slots[5];
        NecklaceSlot = Equipment_Slots[6];
        RingSlot = Equipment_Slots[7];
    }

    private void Start()
    {
        for (int i = 0; i < inventory_Slots.Count; i++)
        {
            inventory_Slots[i].index = i;
        }

        for (int i = 0; i < Equipment_Slots.Count; i++)
        {
            Equipment_Slots[i].index = i;
        }

        LoadInven();
    }

    private void Update()
    {
        DeleteNullSlot();

        for (int i = 0; i < inventory_Slots.Count; i++)
        {
            inventory_Slots[i].index = i;

            if (inventory_Slots[i].itemCount == 0)
            {
                isFull = false;
            }

            if (i == (inventory_Slots.Count - 1))
                if (inventory_Slots[i].itemCount != 0)
                    isFull = true;
        }

        goldText.text = string.Format("{0:N0} <color=#FFF900>G</color>", player.stats.Gold);
    }

    /// <summary>
    /// 인벤토리에 아이템을 추가시킴. 겹칠수 있는 아이템은 겹쳐줌.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void GetItem(Item _item, int count = 1)
    {
        if (_item.Type == 9 || _item.Type == 10) // 소비탬이나 재료탬일때
        {
            foreach (Slot slot in inventory_Slots) // 모든 슬롯중
            {
                if (slot.itemCount != 0) // 아이템이 있는 슬롯에
                {
                    if (slot.item.Name == _item.Name) // 같은 이름을 가진 item이 있으면
                    {
                        slot.SetSlotCount(count); // 갯수를 증가시킴
                        return;
                    }
                }
            }
        }

        // 장비 종류에 상관없이
        foreach (Slot slot in inventory_Slots) // 모든 슬롯중
        {
            if (slot.itemCount == 0) // 비어 있는 슬롯에
            {
                slot.AddItem(_item, count); // 아이템추가
                return;
            }
        }
    }

    public void SaveInven()
    {
        List<Item> myItems = new List<Item>();
        List<Item> myEquipItems = new List<Item>();
        List<Item> myPotionSlotItems = new List<Item>();

        for (int i = 0; i < inventory_Slots.Count; i++)
        {
            if (inventory_Slots[i].itemCount != 0)
            {
                if (!myItems.Contains(inventory_Slots[i].item))
                {
                    myItems.Add(inventory_Slots[i].item);
                }
            }
        }

        for (int i = 0; i < Equipment_Slots.Count; i++)
        {
            if (Equipment_Slots[i].itemCount != 0)
            {
                if (!myEquipItems.Contains(Equipment_Slots[i].item))
                {
                    myEquipItems.Add(Equipment_Slots[i].item);
                }
            }
        }

        for (int i = 0; i < playerAC.potionSlot.Count; i++)
        {
            if (playerAC.potionSlot[i].itemCount != 0)
            {
                if (!myPotionSlotItems.Contains(playerAC.potionSlot[i].item))
                {
                    myPotionSlotItems.Add(playerAC.potionSlot[i].item);
                }
            }
        }

        string Jdata = JsonConvert.SerializeObject(myItems, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/Data/MyInvenItems.text", Jdata);

        string Jdata2 = JsonConvert.SerializeObject(myEquipItems, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/Data/MyEquipItems.text", Jdata2);

        string Jdata3 = JsonConvert.SerializeObject(myPotionSlotItems, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/Data/MyQuickSlotItems.text", Jdata3);

        Debug.Log("인벤 세이브 완료");
    }

    public void LoadInven()
    {
        if (File.Exists(Application.dataPath + "/Resources/Data/MyInvenItems.text"))
        {
            List<Item> loadItems = new List<Item>();

            string Jdata = File.ReadAllText(Application.dataPath + "/Resources/Data/MyInvenItems.text");
            loadItems = JsonConvert.DeserializeObject<List<Item>>(Jdata);

            foreach (var item in loadItems)
            {
                inventory_Slots[item.SlotIndex].AddItem(item, item.Count);
            }

            Debug.Log("인벤토리 로드 완료.");
        }
        else
        {
            Debug.Log("인벤토리 세이브 데이터 없음.");
        }

        if (File.Exists(Application.dataPath + "/Resources/Data/MyEquipItems.text"))
        {
            List<Item> loadEquipItems = new List<Item>();

            string Jdata2 = File.ReadAllText(Application.dataPath + "/Resources/Data/MyEquipItems.text");
            loadEquipItems = JsonConvert.DeserializeObject<List<Item>>(Jdata2);

            foreach (var item in loadEquipItems)
            {
                Equipment_Slots[item.SlotIndex].OnLoadEquipItem(item);
            }
        }
        else
        {
            Debug.Log("장착한 장비 데이터 없음.");
        }

        if (File.Exists(Application.dataPath + "/Resources/Data/MyQuickSlotItems.text"))
        {
            List<Item> loadPotionSlotItems = new List<Item>();

            string Jdata2 = File.ReadAllText(Application.dataPath + "/Resources/Data/MyQuickSlotItems.text");
            loadPotionSlotItems = JsonConvert.DeserializeObject<List<Item>>(Jdata2);

            foreach (var item in loadPotionSlotItems)
            {
                playerAC.potionSlot[item.SlotIndex].AddItem(item, item.Count);
            }
        }
        else
        {
            Debug.Log("장착한 장비 데이터 없음.");
        }
    }

    /// <summary>
    /// 게임도중에 슬롯갯수가 바뀌면 없는 슬롯을 리스트에서 제거해줌
    /// </summary>
    void DeleteNullSlot()
    {
        for (int i = 0; i < inventory_Slots.Count; ++i)
        {
            if (inventory_Slots[i] == null)
                inventory_Slots.RemoveAt(i);
        }
    }
}
