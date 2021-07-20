using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillEffect
{
    public string skillName;

    //[Tooltip("HP, MP, HPMAX, MPMAX, STR, ATK �� ����.")]
    //public string[] part;

    //public int[] num;
}

public class SkillEffectDatebase : MonoBehaviour
{
    PlayerCtrl player;

    public SkillEffect[] skillEffects;

    public const string ShockWave = "ShockWave", LightningBolt = "LightningBolt";


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    public void UseSkill(Skill _skill)
    {
        switch (_skill.skillName)
        {
            case ShockWave:
                var obj = Instantiate(_skill.skillPrefab, player.transform.position, Quaternion.identity);
                obj.transform.forward = player.transform.forward;
                //Debug.Log(skillEffects[0].skillName + " ���");
                break;
            case LightningBolt:
                //Debug.Log(skillEffects[1].skillName + " ���");
                break;
            default:
                Debug.Log("��ų �����Ͱ� �����ϴ�.");
                break;
        }
    }
}
