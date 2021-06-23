using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LMonsterWeaponCtrl : MonoBehaviour
{
    Collider col;

    PlayerCtrl player;
    MonsterCtrl mob;

    float attackDamage;

    void Start()
    {
        col = GetComponent<Collider>();
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
        mob = GameObject.Find("Golem").GetComponent<MonsterCtrl>();
        attackDamage = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PLAYER"))
        {
            col.enabled = false;
            //print("�÷��̾� �ǰ����� ���� �ݶ��̴���Ȱ��ȭ");

            if (player.state != PlayerCtrl.State.HIT)
            {
                player.hp -= attackDamage;
                player.SetState(PlayerCtrl.State.HIT);
            }

            print(player.hp);

            if (player.hp <= 0)
                player.SetState(PlayerCtrl.State.DIE);
        }
    }

    void Update()
    {
        if (mob.state == MonsterCtrl.State.ATTACK)
        {
            if (mob.attackType == 1)
            {
                col.enabled = true;
                //print("���ݻ��·� ���� �ݶ��̴�Ȱ��ȭ");
            }
            else
            {
                col.enabled = false;
            }
        }
        else
        {
            col.enabled = false;
            //print("����ݻ��·� ���� �ݶ��̴���Ȱ��ȭ");
        }
    }
}
