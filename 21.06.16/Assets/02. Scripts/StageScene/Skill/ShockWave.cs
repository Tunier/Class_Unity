using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;

    PlayerWeaponCtrl Pwp;
    PlayerCtrl player;

    public List<GameObject> mobList;

    public float moveSpeed;

    public bool isCrit = false;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        mobList = new List<GameObject>();

        Pwp = GameObject.Find("PlayerWeapon").GetComponent<PlayerWeaponCtrl>();
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();

        moveSpeed = 10f;
    }
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        rb.velocity = Vector3.forward * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MONSTER"))
        {
            var hitmob = other.GetComponent<MonsterCtrl>();

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
}
