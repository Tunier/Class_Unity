using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSkillSlot : MonoBehaviour
{
    [SerializeField]
    GameObject slotsParent;

    public List<SkillSlot> slots;

    private void Awake()
    {
        slots = new List<SkillSlot>();
        slots.AddRange(slotsParent.GetComponentsInChildren<SkillSlot>());
    }

    private void Start()
    {
        slots[0].AddSkillIcon(Resources.Load<Skill>("Skill_Info/LightningBolt"));
        slots[1].AddSkillIcon(Resources.Load<Skill>("Skill_Info/ShockWave")); // 임시로 스킬 추가
    }

    void Update()
    {
        DeleteNullSlot();
    }

    void DeleteNullSlot()
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i] == null)
                slots.RemoveAt(i);
        }
    }
}
