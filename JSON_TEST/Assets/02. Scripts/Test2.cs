using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour
{
    PlayerTest player;
    SkillDatabase skillDB;

    [SerializeField]
    Text skillNameText;
    [SerializeField]
    Text skillLvText;
    [SerializeField]
    Text skillDiscText;

    private void Awake()
    {
        player = FindObjectOfType<PlayerTest>();
        skillDB = FindObjectOfType<SkillDatabase>();
    }

    private void Start()
    {
        UpdateToolTip();
    }

    public void UpdateToolTip() // 임시로 Hp회복 스킬 툴팁 업데이트함.
    {
        var skill = player.skillDic[1];
        skillNameText.text = skill.Name;
        skillLvText.text = "Lv : " + skill.SkillLv;
        if (skill.SkillLv != 0)
            skillDiscText.text = string.Format(skill.SkillDescription, skill.Value + (skill.SkillLv - 1) * skill.ValueFactor);
        else
            skillDiscText.text = string.Format(skill.SkillDescription + "\n미습득 스킬입니다.", skill.Value);
    }

    public void OnClickSkillLvUpButton()
    {
        player.SkillLvUp(skillDB.AllSkillDic[1]);
        UpdateToolTip();
    }
}
