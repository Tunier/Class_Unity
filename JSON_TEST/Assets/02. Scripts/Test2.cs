using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour
{
    PlayerTest player;
    SkillDatabase skillDB;
    [SerializeField]
    Text skillDiscText;

    private void Awake()
    {
        player = FindObjectOfType<PlayerTest>();
        skillDB = FindObjectOfType<SkillDatabase>();
    }

    private void Start()
    {
        skillDiscText.text = string.Format(skillDB.AllSkillDic[0].SkillDescription, skillDB.AllSkillDic[0].Value + (player.playerStats.Skill_Lv - 1) * skillDB.AllSkillDic[0].ValueFactor);
    }

    public void SkillLevelUp()
    {
        player.playerStats.Skill_Lv++;
        UpdateToolTip();
    }

    public void UpdateToolTip()
    {
        skillDiscText.text = string.Format(skillDB.AllSkillDic[0].SkillDescription, skillDB.AllSkillDic[0].Value + (player.playerStats.Skill_Lv - 1) * skillDB.AllSkillDic[0].ValueFactor);
    }
}
