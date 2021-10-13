using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    PlayerInfo player;

    PlayerActionCtrl playerAC;

    Vector3 pos;

    public GameObject curHitMob;
    public List<GameObject> mobList = new List<GameObject>();

    Skill _skill;

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerInfo>();
        playerAC = FindObjectOfType<PlayerActionCtrl>();

        StartCoroutine(StopMotion());
        StartCoroutine(ClearMobList());

        Destroy(gameObject, 5.2f);

        _skill = SkillDatabase.instance.AllSkillDic["0300005"];
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
                    curHitMob.GetComponent<MonsterBase>().Hit((_skill.Value + (player.player_Skill_Dic["0300005"] - 1)
                                                                * _skill.ValueFactor) * 1.5f);

                    UIManager.Instance.ShowDamageText((_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor) * 1.5f, true);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 1.5f * 0.01f;
                }
                else
                {
                    curHitMob.GetComponent<MonsterBase>().Hit(_skill.Value + (player.player_Skill_Dic["0300005"] - 1)
                                                               * _skill.ValueFactor);

                    UIManager.Instance.ShowDamageText(_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 0.01f;
                }
            }
            else { return; }
        }
    }

    void Update()
    {
        pos = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);

        transform.position = pos;
    }

    IEnumerator StopMotion()
    {
        yield return new WaitForSeconds(5f);

        playerAC.isWhirlwind = false;
        playerAC.isUsingSkill = false;
    }

    IEnumerator ClearMobList()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.33f);

            mobList.Clear();
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
