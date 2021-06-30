using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    public MonsterCtrl hitmob;
    
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
            hitmob = other.GetComponent<MonsterCtrl>();

            hitmob.Hit(damage);

            if (hitmob.hp <= 0)
            {
                player.exp += hitmob.exp;
                hitmob.Die();
            }
        }
    }

    void Update()
    {

    }

    
}
