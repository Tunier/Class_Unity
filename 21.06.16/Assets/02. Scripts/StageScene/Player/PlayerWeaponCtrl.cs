using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    public MonsterCtrl hitmob;
    [SerializeField]
    public List<GameObject> mobList;

    public float weaponDamage = 0;
    public float resultdamage;
    public float criticalDamage;
    public float attackSpeed;
    public float critcalChance;

    float hitAfterTime;

    void Start()
    {
        resultdamage = player.str + weaponDamage;
        attackSpeed = 1f;

        mobList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER"))
        {
            hitmob = other.GetComponent<MonsterCtrl>();

            if (!mobList.Contains(other.gameObject))
            {
                mobList.Add(other.gameObject);
            }
            else { return; }

            int critcalRandom = Random.Range(0, 100);

            if (critcalRandom >= 100 - critcalChance)
            {
                foreach (GameObject obj in mobList)
                {
                    obj.GetComponent<MonsterCtrl>().Hit(criticalDamage);
                    //StartCoroutine(obj.GetComponent<MonsterCtrl>().MultyHit(damage, 2,0.2f)); // 딜레이를 가진 연속공격호출
                }
            }
            else
            {
                foreach (GameObject obj in mobList)
                {
                    obj.GetComponent<MonsterCtrl>().Hit(resultdamage);
                }
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
        resultdamage = Mathf.Round(player.str) + weaponDamage;
        criticalDamage = Mathf.Round(resultdamage * 1.5f);
        critcalChance = Mathf.Round(player.dex + 20f);

        attackSpeed = 1f;
    }

    public void ClearMobList()
    {
        mobList.Clear();
    }
}
