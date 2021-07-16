using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillCtrl : MonoBehaviour
{
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
            datebase.UseSkill(qSkillSlot.slots[0].skill);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            datebase.UseSkill(qSkillSlot.slots[1].skill);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            datebase.UseSkill(qSkillSlot.slots[2].skill);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            datebase.UseSkill(qSkillSlot.slots[3].skill);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            datebase.UseSkill(qSkillSlot.slots[4].skill);
        }
    }
}
