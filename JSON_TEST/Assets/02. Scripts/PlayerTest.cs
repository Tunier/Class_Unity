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

    public List<Skill> skillList = new List<Skill>(); // 나중에 세이브용 데이터. 세이브할때 딕셔너리에서 스킬만 받아서 저장할것.
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
        skillDic.Add(skillDB.AllSkillDic[0].Index, skillDB.AllSkillDic[0]); // 임시로 플레이어의 스킬리스트에 파이어볼 스킬을 넣어줌.
        skillDic.Add(skillDB.AllSkillDic[1].Index, skillDB.AllSkillDic[1]); // 임시로 플레이어의 스킬리스트에 Hp증가 스킬 넣어줌.
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
        if (skillDic.ContainsKey(_skill.Index)) // 해당 스킬을 플레이어가 가지고 있을때
        {
            skillDic[_skill.Index].SkillLv++; // 해당 스킬의 레벨을 올린다.
            Debug.Log(skillDic[_skill.Index].Name + " 스킬의 스킬레벨이 " + skillDic[_skill.Index].SkillLv + "가 되었습니다.");
            if (skillDic[_skill.Index].skillType == Skill.SkillType.Passive) // 스킬타입이 패시브인 스킬을 레벨업 시키면
            {
                skillDB.UseSkill(_skill, gameObject); // 패시브 스킬 효과 발동.
            }
        }
        else
            Debug.LogError("플레이어가 해당 스킬을 가지고 있지 않습니다.");
    }
}
