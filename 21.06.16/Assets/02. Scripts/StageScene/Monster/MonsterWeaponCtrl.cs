using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeaponCtrl : MonoBehaviour
{
    PlayerCtrl player;

    float damage;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
        
        damage = 10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PLAYER"))
        {
            if ((player.hitable == true) && (player.state != PlayerCtrl.State.DIE))
            {
                player.Hit(damage);
            }
        }
    }
}
