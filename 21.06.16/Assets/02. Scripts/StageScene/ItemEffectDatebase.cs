using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;

    [Tooltip("HP, MP, HPMAX, MPMAX, STR, ATK 만 가능.")]
    public string[] part;

    public int[] num;
}

public class ItemEffectDatebase : MonoBehaviour
{
    [SerializeField]
    ItemEffect[] itemEffects;

    [SerializeField]
    PlayerCtrl player;
    [SerializeField]
    PlayerWeaponCtrl pWp;
    [SerializeField]
    SlotToolTip toolTip;

    [SerializeField]
    Status status;

    private const string HP = "HP", MP = "MP", HPMAX = "HPMAX", MPMAX = "MPMAX", STR = "STR", ATK = "ATK";

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
                                    Debug.Log("오류");
                                    break;
                            }
                        }
                    }
                }
                break;
        }
    }

    public void EquipItem(Item _item)
    {
        if (_item.EquipmentType == "Weapon")
        {
            if (status.weaponSlot.item == null)
            {
                status.weaponSlot.AddItem(_item);
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
                        case ATK:
                            pWp.weaponDamage += itemEffects[i].num[j];
                            break;
                        default:
                            Debug.Log("오류");
                            break;
                    }
                }
            }
        }
    }

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
                        case ATK:
                            pWp.weaponDamage -= itemEffects[i].num[j];
                            break;
                        default:
                            Debug.Log("오류");
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
}
