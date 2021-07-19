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

        if (Input.GetKeyDown(KeyCode.X))
        {
            UseSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            UseSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            UseSkill(3);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            UseSkill(4);
        }
    }

    void UseSkill(int i)
    {
        if (qSkillSlot.slots[i].skill != null)
        {
            if (qSkillSlot.slots[i].currentSkillCoolTime <= 0f && player.mp >= qSkillSlot.slots[i].skill.mpCost)
            {
                player.mp -= qSkillSlot.slots[i].skill.mpCost;
                qSkillSlot.slots[i].currentSkillCoolTime += qSkillSlot.slots[i].skill.coolTime;
                datebase.UseSkill(qSkillSlot.slots[i].skill);
            }
            else if (player.mp < qSkillSlot.slots[i].skill.mpCost)
            {
                StartCoroutine(UIManager.instance.PrintActionText("마나가 부족합니다."));
                Debug.Log("마나가 부족합니다.");
            }
            else if (qSkillSlot.slots[i].currentSkillCoolTime > 0)
            {
                StartCoroutine(UIManager.instance.PrintActionText("스킬이 쿨타임입니다."));
                Debug.Log("스킬이 쿨타임입니다.");
            }
        }
        else
        {
            StartCoroutine(UIManager.instance.PrintActionText("스킬이 없습니다."));
            Debug.Log("스킬이 없습니다.");
        }
    }
}
