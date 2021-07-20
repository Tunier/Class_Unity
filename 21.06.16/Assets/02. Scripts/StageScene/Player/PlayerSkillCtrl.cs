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
    /// ���� ��ȣ�� �޾Ƽ� �ش� ���Կ� ��ų�� �ְ�, ������ ����ϰ�, �ش� ��ų�� ��Ÿ���� �ƴϸ� ��ų�� �����.<br/>
    /// ��ų�� ������ "��ų�� �����ϴ�." �ؽ�Ʈ ���, ������ �����ϸ� "������ �����մϴ�." �ؽ�Ʈ ���, ��Ÿ���̸� "��ų�� ��Ÿ���Դϴ�." �ؽ�Ʈ ���.
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
                StartCoroutine(UIManager.instance.PrintActionText("������ �����մϴ�."));
                //Debug.Log("������ �����մϴ�.");
            }
            else if (qSkillSlot.slots[slotNum].currentSkillCoolTime > 0)
            {
                StartCoroutine(UIManager.instance.PrintActionText("��ų�� ��Ÿ���Դϴ�."));
                //Debug.Log("��ų�� ��Ÿ���Դϴ�.");
            }
        }
        else
        {
            StartCoroutine(UIManager.instance.PrintActionText("��ų�� �����ϴ�."));
            //Debug.Log("��ų�� �����ϴ�.");
        }
    }
}
