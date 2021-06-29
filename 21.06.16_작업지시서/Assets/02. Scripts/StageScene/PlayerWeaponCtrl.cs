using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    MonsterCtrl mob;

    public float damage;
    public float attackSpeed;

    void Start()
    {
        damage = 10f;
        attackSpeed = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER"))
        {
            mob = other.GetComponent<MonsterCtrl>();

            mob.Hit(damage);

            if (mob.hp <= 0)
            {
                player.exp += mob.exp;
                mob.Die();
            }
        }
    }

    void Update()
    {

    }

    
}
