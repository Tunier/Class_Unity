using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Tree_UI : MonoBehaviour
{
    PlayerInfo playerinfo;
    Tooltip tooltip;

    [SerializeField]
    Button SkillTree_BG;

    public GameObject skilltree_Slot_Parent;
    public List<SkillSlot> slots = new List<SkillSlot>();

    public List<Skill> player_skill = new List<Skill>();

    public GameObject skillOptionPanel;

    public SkillSlot curSlot = null;

    public Text skillPointText;

    private void Awake()
    {
        playerinfo = FindObjectOfType<PlayerInfo>();
        tooltip = FindObjectOfType<Tooltip>();

        slots.AddRange(skilltree_Slot_Parent.GetComponentsInChildren<SkillSlot>());
    }

    void Start()
    {
        List<string> keys = new List<string>();

        keys.AddRange(playerinfo.player_Skill_Dic.Keys);

        foreach (string key in keys)
        {
            player_skill.Add(SkillDatabase.instance.AllSkillDic[key]);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            slots[i].AddSkill(player_skill[i]);
        }
    }

    void Update()
    {
        if (!SkillTree_BG.gameObject.activeSelf)
        {
            skillOptionPanel.SetActive(false);
        }

        skillPointText.text = playerinfo.stats.Skill_Point + "";
    }

    public void BG_Click()
    {
        skillOptionPanel.SetActive(false);
    }

    public void PlusBtnClick()
    {
        if (playerinfo.stats.Level >= curSlot.skill.NeedLv + playerinfo.player_Skill_Dic[curSlot.skill.UIDCODE])
        {
            if (playerinfo.player_Skill_Dic[curSlot.skill.UIDCODE] < curSlot.skill.MaxSkillLv)
            {
                playerinfo.SetSkillLv(SkillDatabase.instance.AllSkillDic[curSlot.skill.UIDCODE]);
                tooltip.ShowTooltip(curSlot.skill);
            }
            else
            {
                SystemText_ScrollView_Ctrl.Instance.PrintText("스킬이 최대 레벨입니다.");
            }
        }
        else
        {
            SystemText_ScrollView_Ctrl.Instance.PrintText("레벨이 모자랍니다.");
        }
    }

    public void MinusBtnClick()
    {
        if (playerinfo.player_Skill_Dic[curSlot.skill.UIDCODE] > 0)
        {
            playerinfo.SetSkillLv(SkillDatabase.instance.AllSkillDic[curSlot.skill.UIDCODE], -1);
            tooltip.ShowTooltip(curSlot.skill);
        }
        else
        {
            SystemText_ScrollView_Ctrl.Instance.PrintText("스킬이 최소 레벨입니다.");
        }
    }
}