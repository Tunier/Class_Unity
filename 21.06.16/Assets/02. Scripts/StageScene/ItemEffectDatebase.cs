using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;

    [Tooltip("HP, MP, HPMAX, MPMAX, STR, DEX, ATK �� ����.")]
    public string[] part;

    public int[] num;
}

public class ItemEffectDatebase : MonoBehaviour
{
    public ItemEffect[] itemEffects;

    [SerializeField]
    PlayerCtrl player;
    [SerializeField]
    PlayerWeaponCtrl pWp;
    [SerializeField]
    SlotToolTip toolTip;

    [SerializeField]
    Status status;
    [SerializeField]
    Inventory inven;

    private const string HP = "HP", MP = "MP", HPMAX = "HPMAX", MPMAX = "MPMAX", STR = "STR", DEX = "DEX", ATK = "ATK";

    public void UseItem(Item _item)
    {
        switch (_item.itemType)
        {
            case Item.ItemType.Used:
                for (int i = 0; i < itemEffects.Length; i++)
                {
                    if (itemEffects[i].itemName == _item.itemName)
                    {
                        for (int j = 0; j < itemEffects[i].part.Length; j++)
                        {
                            switch (itemEffects[i].part[j])
                            {
                                case HP:
                                    player.hp += itemEffects[i].num[j];
                                    break;
                                case MP:
                                    player.mp += itemEffects[i].num[j];
                                    break;
                                default:
                                    Debug.Log("����");
                                    break;
                            }
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// ��� �ش��ϴ� ���ĭ�� �߰���Ű��, ������ ������Ŵ.
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(Item _item)
    {
        if (_item.EquipmentType == "Weapon")
        {
            if (status.weaponSlot.item == null)
            {
                status.weaponSlot.AddItem(_item);
            }
            else
            {
                Item item = status.weaponSlot.item;

                UnEquipItem(item);
                status.weaponSlot.SetSlotCount(-1);
                status.weaponSlot.AddItem(_item);
                inven.GetItem(item);
            }
        }

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].itemName == _item.itemName)
            {
                for (int j = 0; j < itemEffects[i].part.Length; j++)
                {
                    switch (itemEffects[i].part[j])
                    {
                        case HPMAX:
                            player.hpMax += itemEffects[i].num[j];
                            break;
                        case MPMAX:
                            player.mpMax += itemEffects[i].num[j];
                            break;
                        case STR:
                            player.str += itemEffects[i].num[j];
                            break;
                        case DEX:
                            player.dex += itemEffects[i].num[j];
                            break;
                        case ATK:
                            pWp.weaponDamage += itemEffects[i].num[j];
                            break;
                        default:
                            Debug.Log("����");
                            break;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// ����� ������ ������ ȿ���� ������. ������ ��������� ����.
    /// </summary>
    /// <param name="_item"></param>
    public void UnEquipItem(Item _item)
    {
        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].itemName == _item.itemName)
            {
                for (int j = 0; j < itemEffects[i].part.Length; j++)
                {
                    switch (itemEffects[i].part[j])
                    {
                        case HPMAX:
                            player.hpMax -= itemEffects[i].num[j];
                            break;
                        case MPMAX:
                            player.mpMax -= itemEffects[i].num[j];
                            break;
                        case STR:
                            player.str -= itemEffects[i].num[j];
                            break;
                        case DEX:
                            player.dex -= itemEffects[i].num[j];
                            break;
                        case ATK:
                            pWp.weaponDamage -= itemEffects[i].num[j];
                            break;
                        default:
                            Debug.Log("����");
                            break;
                    }
                }
            }
        }
    }

    public void ShowToolTip(Item _item)
    {
        toolTip.ShowToolTip(_item);
    }

    public void HideToolTip()
    {
        toolTip.HideToolTip();
    }

    public void SetItemCostText(int cost)
    {
        toolTip.SetItemCostText(cost);
    }
}
