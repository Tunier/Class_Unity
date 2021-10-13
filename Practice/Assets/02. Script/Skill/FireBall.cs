using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    GameObject fireBallEffect;
    [SerializeField]
    GameObject explosion_Effet;

    public PlayerInfo player;

    Rigidbody rb;

    float moveSpeed;

    private void Awake()
    {
        explosion_Effet.SetActive(false);
        rb = GetComponent<Rigidbody>();

        moveSpeed = 30f;
    }

    void Start()
    {
        rb.velocity = transform.forward * moveSpeed;
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("RaycastTarget") && !other.CompareTag("Player") && !other.CompareTag("Effect"))
        {
            rb.velocity = Vector3.zero;
            fireBallEffect.SetActive(false);
            explosion_Effet.SetActive(true);

            // ���� ��Ʈ����
            if (other.CompareTag("Monster"))
            {
                var mob = other.GetComponent<MonsterBase>();
                var _skill = SkillDatabase.instance.AllSkillDic["0300000"];
                player.targetMonster = mob.gameObject;

                if (CritcalCalculate()) // ũ��Ƽ���� ������ ����ؼ� Hit�� ȣ��
                {
                    mob.Hit(_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor);

                    UIManager.Instance.ShowDamageText((_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor) * 1.5f, true);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 1.5f * 0.01f;
                }
                else
                {
                    mob.Hit(_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor);

                    UIManager.Instance.ShowDamageText(_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor);

                    player.curHp += player.finalLifeStealPercent * player.finalNormalAtk * 1.5f * 0.01f;
                }
            }

            Destroy(gameObject, 1f);
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
