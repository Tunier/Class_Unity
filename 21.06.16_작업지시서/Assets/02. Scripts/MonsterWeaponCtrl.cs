using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponCtrl : MonoBehaviour
{
    Collider col;

    MonsterCtrl Mob;

    void Start()
    {
        col = GetComponent<Collider>();
        Mob = GetComponent<MonsterCtrl>();
        col.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PLAYER"))
        {
            col.enabled = false;

            PlayerCtrl player = collision.collider.GetComponent<PlayerCtrl>();

            player.SetState(PlayerCtrl.State.HIT);

            player.hp -= 20;

            print(player.hp);

            if (player.hp <= 0)
                player.SetState(PlayerCtrl.State.DIE);
        }
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            //if (Mob.state == MonsterCtrl.State.ATTACK)
            //    col.enabled = true;
            //else
            //    col.enabled = false;

            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        StartCoroutine(CheckState());
    }
}
