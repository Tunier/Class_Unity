using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillEffect
{
    public string skillName;

    //[Tooltip("HP, MP, HPMAX, MPMAX, STR, ATK 만 가능.")]
    //public string[] part;

    //public int[] num;
}

public class SkillEffectDatebase : MonoBehaviour
{
    PlayerCtrl player;
    
    public SkillEffect[] skillEffects;

    public const string ShockWave = "ShockWave", LightningBolt = "LightningBolt";

    [SerializeField]
    GameObject shockwavePrefeb;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }

    public void UseSkill(Skill _skill)
    {
        if (_skill != null)
            switch (_skill.skillName)
            {
                case ShockWave:
                    Debug.Log(skillEffects[0].skillName + " 사용");
                    player.mp -= 10;
                    var obj = Instantiate(shockwavePrefeb, player.transform.position, Quaternion.Euler(player.transform.forward));
                    break;
                case LightningBolt:
                    Debug.Log(skillEffects[1].skillName + " 사용");
                    break;
                default:
                    Debug.Log("스킬 데이터가 없습니다.");
                    break;
            }

        else
            Debug.Log("해당 슬롯에 스킬이 없습니다.");
    }
}
