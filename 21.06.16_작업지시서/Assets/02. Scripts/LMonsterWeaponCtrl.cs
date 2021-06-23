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
            //print("플레이어 피격으로 인한 콜라이더비활성화");

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
                //print("공격상태로 인한 콜라이더활성화");
            }
            else
            {
                col.enabled = false;
            }
        }
        else
        {
            col.enabled = false;
            //print("비공격상태로 인한 콜라이더비활성화");
        }
    }
}
