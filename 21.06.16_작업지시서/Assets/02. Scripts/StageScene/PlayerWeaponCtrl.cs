using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    public MonsterCtrl hitmob;

    public float damage;
    public float criticalDamage;
    public float attackSpeed;
    float critcalChance;

    void Start()
    {
        damage = player.str;
        attackSpeed = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER"))
        {
            hitmob = other.GetComponent<MonsterCtrl>();

            critcalChance = Random.Range(0, 100);

            if (critcalChance >= 80)
                hitmob.Hit(criticalDamage);
            else
                hitmob.Hit(damage);

            if ((hitmob.hp <= 0) && (hitmob.state != MonsterCtrl.State.DIE))
            {
                hitmob.Die();
                player.exp += hitmob.exp;
            }
        }
    }

    void Update()
    {
        damage = player.str;
        criticalDamage = player.str * 1.5f;

        attackSpeed = 1f;
    }


}
