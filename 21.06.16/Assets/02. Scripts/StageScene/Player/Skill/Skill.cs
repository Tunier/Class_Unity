using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill/skill")]
public class Skill : ScriptableObject
{
    public enum SkillType
    {
        Active,
        Passive,
    }

    public string skillName;
    public SkillType skilltype;
    [TextArea]
    public string skillDescription;

    public Sprite skillImage;
    public GameObject skillPrefab;

    public float coolTime;

    public float mpCost;
}
