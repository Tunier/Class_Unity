using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    public MonsterCtrl hitmob;
    CameraShake shake;

    public float damage;
    public float criticalDamage;
    public float attackSpeed;
    public float critcalChance;

    void Start()
    {
        shake = Camera.main.GetComponent<CameraShake>();
        damage = player.str;
        attackSpeed = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER"))
        {
            hitmob = other.GetComponent<MonsterCtrl>();

            int critcalRandom = Random.Range(0, 100);

            if (critcalRandom >= 100 - critcalChance)
            {
                hitmob.Hit(criticalDamage);
                StartCoroutine(shake.ShakeCamera());
            }
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
        critcalChance = player.dex + 20f;

        attackSpeed = 1f;
    }


}
