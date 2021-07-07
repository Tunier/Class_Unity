using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    public MonsterCtrl hitmob;
    public List<GameObject> mobList;

    public float damage;
    public float criticalDamage;
    public float attackSpeed;
    public float critcalChance;

    float hitAfterTime;

    void Start()
    {
        damage = player.str;
        attackSpeed = 1f;

        mobList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER"))
        {
            hitmob = other.GetComponent<MonsterCtrl>();

            if (mobList.Contains(other.gameObject)) { }
            else
            {
                mobList.Add(other.gameObject);
            }

            int critcalRandom = Random.Range(0, 100);

            if (critcalRandom >= 100 - critcalChance)
            {
                foreach (GameObject obj in mobList)
                {
                    obj.GetComponent<MonsterCtrl>().Hit(criticalDamage);
                    //StartCoroutine(obj.GetComponent<MonsterCtrl>().MultyHit(damage, 2,0.2f)); // 딜레이를 가진 연속공격호출
                }
                mobList.Clear();
                //hitmob.Hit(criticalDamage);
            }
            else
            {
                foreach (GameObject obj in mobList)
                {
                    obj.GetComponent<MonsterCtrl>().Hit(damage);
                }
                mobList.Clear();
                //hitmob.Hit(damage);
            }

            if ((hitmob.hp <= 0) && (hitmob.state != MonsterCtrl.State.DIE))
            {
                hitmob.Die();
                player.exp += hitmob.exp;
            }
        }
    }

    void Update()
    {
        damage = Mathf.Round(player.str);
        criticalDamage = Mathf.Round(damage * 1.5f);
        critcalChance = 100;//Mathf.Round(player.dex + 20f);

        attackSpeed = 1f;
    }
}
