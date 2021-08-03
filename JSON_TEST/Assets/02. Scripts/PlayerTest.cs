using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public int Level;
    public float MaxHp;
    public float MaxMp;
    public float Str;
    public float Int;
}

public class PlayerTest : MonoBehaviour
{
    public Stats stats;

    public List<Skill> skillList = new List<Skill>(); // ���߿� ���̺�� ������. ���̺��Ҷ� ��ųʸ����� ��ų�� �޾Ƽ� �����Ұ�.
    public Dictionary<int, Skill> skillDic = new Dictionary<int, Skill>();

    public float finalMaxHp;
    public float curHp;
    public float ItemEffectMaxHp;
    public float SkillEffectMaxHp;

    public float finalMaxMp;
    public float curMp;
    public float ItemEffectMaxMp;
    public float SkillEffectMaxMp;

    public float finalStr;
    public float ItemEffectStr;
    public float SkillEffectStr;

    public float finalInt;
    public float ItemEffectInt;
    public float SkillEffectInt;

    public float finalAtk;
    public float ItemEffectAtk;
    public float SkillEffectAtk;

    SkillDatabase skillDB;

    private void Awake()
    {
        skillDB = FindObjectOfType<SkillDatabase>();

        stats.Level = 5;
        stats.MaxHp = 100f + (stats.Level - 1) * 20;
        //stats.MaxMp = 50f;
        stats.Str = 5f + (stats.Level - 1);
        stats.Int = 5f + (stats.Level - 1);
    }

    void Start()
    {
        skillDic.Add(skillDB.AllSkillDic[0].Index, skillDB.AllSkillDic[0]); // �ӽ÷� �÷��̾��� ��ų����Ʈ�� ���̾ ��ų�� �־���.
        skillDic.Add(skillDB.AllSkillDic[1].Index, skillDB.AllSkillDic[1]); // �ӽ÷� �÷��̾��� ��ų����Ʈ�� Hp���� ��ų �־���.
        skillDic[1].SkillLv = 0;

        foreach (KeyValuePair<int, Skill> skill in skillDic)
        {
            if (skill.Value.skillType == Skill.SkillType.Passive)
            {
                skillDB.UsePassiveSkillOnLoad(skill.Value, gameObject);
            }
        }

        RefeshFinalStats();
        curHp = finalMaxHp;
    }

    void Update()
    {

    }

    public void RefeshFinalStats()
    {
        finalMaxHp = stats.MaxHp + ItemEffectMaxHp + SkillEffectMaxHp;
        finalAtk = stats.Str + ItemEffectAtk + SkillEffectAtk;
        finalStr = stats.Str + ItemEffectStr + SkillEffectStr;
        finalInt = stats.Int + ItemEffectInt + SkillEffectInt;
    }

    public void SkillLvUp(Skill _skill)
    {
        if (skillDic.ContainsKey(_skill.Index)) // �ش� ��ų�� �÷��̾ ������ ������
        {
            skillDic[_skill.Index].SkillLv++; // �ش� ��ų�� ������ �ø���.
            Debug.Log(skillDic[_skill.Index].Name + " ��ų�� ��ų������ " + skillDic[_skill.Index].SkillLv + "�� �Ǿ����ϴ�.");
            if (skillDic[_skill.Index].skillType == Skill.SkillType.Passive) // ��ųŸ���� �нú��� ��ų�� ������ ��Ű��
            {
                skillDB.UseSkill(_skill, gameObject); // �нú� ��ų ȿ�� �ߵ�.
            }
        }
        else
            Debug.LogError("�÷��̾ �ش� ��ų�� ������ ���� �ʽ��ϴ�.");
    }
}
