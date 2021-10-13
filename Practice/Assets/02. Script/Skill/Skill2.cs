using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour
{
    PlayerInfo player;
    PlayerActionCtrl playerAC;

    GameObject curHitMob;

    public List<GameObject> mobList = new List<GameObject>();

    Skill _skill;

    public GameObject[] skillEffect;

    void Start()
    {
        player = FindObjectOfType<PlayerInfo>();
        playerAC = FindObjectOfType<PlayerActionCtrl>();

        _skill = SkillDatabase.instance.AllSkillDic["0300002"];
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster")) //���Ͱ� ������
        {
            curHitMob = other.gameObject;

            if (!mobList.Contains(curHitMob)) // ���� ���Ͱ� ����Ʈ�� ������
            {
                player.targetMonster = curHitMob;
                mobList.Add(curHitMob); // ���� ���͸� ����Ʈ�� �����ϰ�


                if (CritcalCalculate()) // ũ��Ƽ���� ������ ����ؼ� Hit�� ȣ��
                {
                    var obj = Instantiate(skillEffect[1], new Vector3(curHitMob.transform.position.x, curHitMob.transform.position.y + 3, curHitMob.transform.position.z),
                                                                      Quaternion.Euler(-90, 0, 0));
                    Destroy(obj, 1f);

                    curHitMob.GetComponent<MonsterBase>().Hit((_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1)
                                                                * _skill.ValueFactor) * 1.5f);

                    UIManager.Instance.ShowDamageText((_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor) * 1.5f, true);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 1.5f * 0.01f;
                }
                else
                {
                    var obj = Instantiate(skillEffect[0], new Vector3(curHitMob.transform.position.x, curHitMob.transform.position.y + 3, curHitMob.transform.position.z), 
                                                                      Quaternion.Euler(-90, 0, 0));
                    Destroy(obj, 1f);

                    curHitMob.GetComponent<MonsterBase>().Hit(_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1)
                                                               * _skill.ValueFactor);

                    UIManager.Instance.ShowDamageText(_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 0.01f;
                }
            }
            else { return; }
        }
    }

    public bool CritcalCalculate()
    {
        bool isCrit = false;
        int crit;

        crit = Random.Range(0, 10000);

        if (player.finalCriticalChance >= crit)
            isCrit = true;

        return isCrit;
    }
}
