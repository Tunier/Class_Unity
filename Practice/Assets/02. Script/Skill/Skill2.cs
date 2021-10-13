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
        if (other.CompareTag("Monster")) //몬스터가 맞으면
        {
            curHitMob = other.gameObject;

            if (!mobList.Contains(curHitMob)) // 맞은 몬스터가 리스트에 없으면
            {
                player.targetMonster = curHitMob;
                mobList.Add(curHitMob); // 맞은 몬스터를 리스트에 저장하고


                if (CritcalCalculate()) // 크리티컬이 떴는지 계산해서 Hit를 호출
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
