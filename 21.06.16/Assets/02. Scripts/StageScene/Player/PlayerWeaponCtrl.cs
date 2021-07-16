using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    public MonsterCtrl hitmob;
    public List<GameObject> mobList;

    public float weaponDamage = 0;
    

    public bool isCrit = false;

    void Start()
    {
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


            foreach (GameObject obj in mobList)
            {
                int critcalRandom = Random.Range(0, 100);

                if (critcalRandom >= 100 - player.critcalChance)
                {
                    isCrit = true;
                    obj.GetComponent<MonsterCtrl>().Hit(player.resultDamage * 1.5f);
                    //StartCoroutine(obj.GetComponent<MonsterCtrl>().MultyHit(resultdamage, 2,0.2f)); // 딜레이를 가진 연속공격호출
                }
                else
                {
                    isCrit = false;
                    obj.GetComponent<MonsterCtrl>().Hit(player.resultDamage);
                }
            }

            if ((hitmob.hp <= 0) && (hitmob.state != MonsterCtrl.State.DIE))
            {
                hitmob.Die();
                player.exp += hitmob.exp;
            }
        }
    }

    public void ClearMobList()
    {
        mobList.Clear();
    }
}
