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
        inventory_Slots.AddRange(inventory_Slot_Parent.GetComponentsInChildren<Slot>()); // ĥ�己���� �޾ƿ��� ������ ��Ȱ��ȭ �Ǹ� ���޾ƿ��⶧���� ��Ȱ��ȭ�� ���µ� ã�ƿü��ִ� ĥ�己���� �޾ƿ´�.
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
    /// �κ��丮�� �������� �߰���Ŵ. ��ĥ�� �ִ� �������� ������.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void GetItem(Item _item, int count = 1)
    {
        if (_item.Type == 9 || _item.Type == 10) // �Һ����̳� ������϶�
        {
            foreach (Slot slot in inventory_Slots) // ��� ������
            {
                if (slot.itemCount != 0) // �������� �ִ� ���Կ�
                {
                    if (slot.item.Name == _item.Name) // ���� �̸��� ���� item�� ������
                    {
                        slot.SetSlotCount(count); // ������ ������Ŵ
                        return;
                    }
                }
            }
        }

        // ��� ������ �������
        foreach (Slot slot in inventory_Slots) // ��� ������
        {
            if (slot.itemCount == 0) // ��� �ִ� ���Կ�
            {
                slot.AddItem(_item, count); // �������߰�
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

        Debug.Log("�κ� ���̺� �Ϸ�");
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

            Debug.Log("�κ��丮 �ε� �Ϸ�.");
        }
        else
        {
            Debug.Log("�κ��丮 ���̺� ������ ����.");
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
            Debug.Log("������ ��� ������ ����.");
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
            Debug.Log("������ ��� ������ ����.");
        }
    }

    /// <summary>
    /// ���ӵ��߿� ���԰����� �ٲ�� ���� ������ ����Ʈ���� ��������
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
