using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponCtrl : MonoBehaviour
{
    public PlayerCtrl player;
    public GameObject hitmob;
    public List<GameObject> mobList;

    public float weaponDamage = 0;

    enum Elimental
    { 
        None,
        Fire,
        Ice,
        Lightning,
    }

    Elimental eli = Elimental.None;

    void Start()
    {
        mobList = new List<GameObject>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER")) //몬스터가 맞으면
        {
            hitmob = other.gameObject;

            player.hitmob = hitmob.GetComponent<MonsterCtrl>(); // 맞은 몬스터 정보를 플레이어에게 전달

            if (!mobList.Contains(other.gameObject)) // 맞은 몬스터가 리스트에 없으면
            {
                mobList.Add(other.gameObject); // 맞은 몬스터를 리스트에 저장하고

                if (player.CritCal()) // 크리티컬이 떴는지 계산해서 Hit를 호출
                {
                    hitmob.GetComponent<MonsterCtrl>().Hit(player.resultDamage * 1.5f, true);
                }
                else
                {
                    hitmob.GetComponent<MonsterCtrl>().Hit(player.resultDamage, false);
                }
            }
            else { return; }
        }
    }
}
