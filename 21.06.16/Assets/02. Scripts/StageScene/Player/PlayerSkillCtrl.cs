using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillCtrl : MonoBehaviour
{
    [SerializeField]
    PlayerCtrl player;

    [SerializeField]
    SkillEffectDatebase datebase;

    [SerializeField]
    QuickSkillSlot qSkillSlot;

    [SerializeField]
    List<Skill> playerSkills;

    void Start()
    {
        playerSkills = new List<Skill>();

        playerSkills.Add(Resources.Load<Skill>("Skill_Info/ShockWave"));
        playerSkills.Add(Resources.Load<Skill>("Skill_Info/LightningBolt"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            UseSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            UseSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            UseSkill(3);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            UseSkill(4);
        }
    }

    /// <summary>
    /// 슬롯 번호를 받아서 해당 슬롯에 스킬이 있고, 마나가 충분하고, 해당 스킬이 쿨타임이 아니면 스킬을 사용함.<br/>
    /// 스킬이 없으면 "스킬이 없습니다." 텍스트 출력, 마나가 부족하면 "마나가 부족합니다." 텍스트 출력, 쿨타임이면 "스킬이 쿨타임입니다." 텍스트 출력.
    /// </summary>
    /// <param name="slotNum"></param>
    void UseSkill(int slotNum)
    {
        if (qSkillSlot.slots[slotNum].skill != null)
        {
            if (qSkillSlot.slots[slotNum].currentSkillCoolTime <= 0f && player.mp >= qSkillSlot.slots[slotNum].skill.mpCost)
            {
                player.mp -= qSkillSlot.slots[slotNum].skill.mpCost;
                qSkillSlot.slots[slotNum].currentSkillCoolTime += qSkillSlot.slots[slotNum].skill.coolTime;
                datebase.UseSkill(qSkillSlot.slots[slotNum].skill);
            }
            else if (player.mp < qSkillSlot.slots[slotNum].skill.mpCost)
            {
                StartCoroutine(UIManager.instance.PrintActionText("마나가 부족합니다."));
                //Debug.Log("마나가 부족합니다.");
            }
            else if (qSkillSlot.slots[slotNum].currentSkillCoolTime > 0)
            {
                StartCoroutine(UIManager.instance.PrintActionText("스킬이 쿨타임입니다."));
                //Debug.Log("스킬이 쿨타임입니다.");
            }
        }
        else
        {
            StartCoroutine(UIManager.instance.PrintActionText("스킬이 없습니다."));
            //Debug.Log("스킬이 없습니다.");
        }
    }
}
