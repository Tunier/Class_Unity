using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponCtrl : MonoBehaviour
{
    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PLAYER"))
        {
            col.enabled = false;

            PlayerCtrl player = collision.collider.GetComponent<PlayerCtrl>();

            player.SetState(PlayerCtrl.State.HIT);

            player.hp -= 10;

            print(player.hp);

            if (player.hp <= 0)
                player.SetState(PlayerCtrl.State.DIE);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
