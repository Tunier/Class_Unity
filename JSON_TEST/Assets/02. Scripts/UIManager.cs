using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerTest player;
    SkillDatabase skillDB;

    [SerializeField]
    Text NameText;
    [SerializeField]
    Text skillLvText;
    [SerializeField]
    Text DiscText;

    private void Awake()
    {
        player = FindObjectOfType<PlayerTest>();
        skillDB = FindObjectOfType<SkillDatabase>();
    }

    private void Start()
    {

    }

    public void UpdateToolTip(Item _item)
    {
        //var skill = player.skillDic[1];
        //skillNameText.text = skill.Name;
        //skillLvText.text = "Lv : " + skill.SkillLv;
        //if (skill.SkillLv != 0)
        //    skillDiscText.text = string.Format(skill.SkillDescription, skill.Value + (skill.SkillLv - 1) * skill.ValueFactor);
        //else
        //    skillDiscText.text = string.Format(skill.SkillDescription + "\n미습득 스킬입니다.", skill.Value);

        //var item = ItemDatabase.instance.AllItemDic["0000003"];

        var item = _item;
        var itemEffect = ItemDatabase.instance.AllItemEffectDic[item.UIDCODE];

        NameText.text = item.Name;
        if(item.Rarity == 0)
            NameText.color = Color.black;
        else if (item.Rarity == 3)
            NameText.color = Color.yellow;

        string[] str = new string[16];

        for (int i = 0; i < itemEffect.i_ValueType.Count; i++)
        {
            switch (itemEffect.i_ValueType[i])
            {
                case 1:
                    str[i] = string.Format("Hp <color>{0}</color> 회복", itemEffect.f_Value[i]);
                    break;
                case 2:
                    str[i] = string.Format("Mp <color>{0}</color> 회복", itemEffect.f_Value[i]);
                    break;
                case 3:
                    str[i] = string.Format("최대Hp <color>{0}</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 4:
                    str[i] = string.Format("최대Mp <color>{0}</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 5:
                    str[i] = string.Format("최대Hp <color>{0}%</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 6:
                    str[i] = string.Format("최대Mp <color>{0}%</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 7:
                    str[i] = string.Format("공격력 <color>{0}</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 8:
                    str[i] = string.Format("공격력 <color>{0}%</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 9:
                    str[i] = string.Format("방어력 <color>{0}</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 10:
                    str[i] = string.Format("방어력 <color>{0}%</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 11:
                    str[i] = string.Format("힘 <color>{0}</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 13:
                    str[i] = string.Format("민첩 <color>{0}</color> 증가", itemEffect.f_Value[i]);
                    break;
                case 18:
                    str[i] = string.Format("공격시 <color>{0}%</color> hp회복", itemEffect.f_Value[i]);
                    break;
            }

            if (i != 0)
                DiscText.text += "\n" + str[i];
            else
                DiscText.text = str[i];
        }
    }

    public void OnClickSkillLvUpButton()
    {
        player.SkillLvUp(skillDB.AllSkillDic[1]);
    }
}
