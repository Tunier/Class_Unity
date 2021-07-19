using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    GameObject hitmob;

    PlayerCtrl player;

    public List<GameObject> mobList;

    public float moveSpeed;
    public float damageFactor;
    public int attackTimes;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        mobList = new List<GameObject>();

        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();

        moveSpeed = 10f;
        damageFactor = 0.6f;
        attackTimes = 2;
    }
    void Start()
    {
        Destroy(gameObject, 1.2f);
    }

    void Update()
    {
        rb.velocity = transform.forward * moveSpeed;
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

                if (player.CritCal()) // 크리티컬이 떴는지 계산해서 StartMultyHit(다단히트)를 호출
                {
                    hitmob.GetComponent<MonsterCtrl>().StartMultyHit(player.resultDamage * damageFactor * 1.5f, attackTimes, 0.2f, true);
                }
                else
                {
                    hitmob.GetComponent<MonsterCtrl>().StartMultyHit(player.resultDamage * damageFactor, attackTimes, 0.2f, false);
                }
            }
            else { return; }
        }
    }
}
