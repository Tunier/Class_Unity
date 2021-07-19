using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject playerWeapon;
    public GameObject hitmob;
    public List<GameObject> mobList;

    public float weaponDamage = 0;

    void Start()
    {
        mobList = new List<GameObject>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER"))
        {
            hitmob = other.gameObject;

            player.hitmob = hitmob.GetComponent<MonsterCtrl>();

            if (!mobList.Contains(other.gameObject))
            {
                mobList.Add(other.gameObject);
            }
            else { return; }

            foreach (GameObject obj in mobList)
            {
                if (player.CritCal())
                {
                    obj.GetComponent<MonsterCtrl>().Hit(player.resultDamage * 1.5f);
                    UIManager.instance.PrintDamageText(player.resultDamage * 1.5f, true);
                }
                else
                {
                    obj.GetComponent<MonsterCtrl>().Hit(player.resultDamage);
                    UIManager.instance.PrintDamageText(player.resultDamage, false);
                }

                if (obj.GetComponent<MonsterCtrl>().hp <= 0)
                {
                    obj.GetComponent<MonsterCtrl>().Die();
                    player.exp += obj.GetComponent<MonsterCtrl>().exp;
                }
            }
        }
    }
}
