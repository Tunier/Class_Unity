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
                    Debug.Log(skillEffects[0].skillName + " ���");
                    player.mp -= 10;
                    var obj = Instantiate(shockwavePrefeb, player.transform.position, Quaternion.Euler(player.transform.forward));
                    break;
                case LightningBolt:
                    Debug.Log(skillEffects[1].skillName + " ���");
                    break;
                default:
                    Debug.Log("��ų �����Ͱ� �����ϴ�.");
                    break;
            }

        else
            Debug.Log("�ش� ���Կ� ��ų�� �����ϴ�.");
    }
}
