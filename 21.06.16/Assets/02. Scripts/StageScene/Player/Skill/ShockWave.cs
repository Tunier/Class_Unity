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
        if (other.CompareTag("MONSTER")) //���Ͱ� ������
        {
            hitmob = other.gameObject;

            player.hitmob = hitmob.GetComponent<MonsterCtrl>(); // ���� ���� ������ �÷��̾�� ����

            if (!mobList.Contains(other.gameObject)) // ���� ���Ͱ� ����Ʈ�� ������
            {
                mobList.Add(other.gameObject); // ���� ���͸� ����Ʈ�� �����ϰ�

                if (player.CritCal()) // ũ��Ƽ���� ������ ����ؼ� StartMultyHit(�ٴ���Ʈ)�� ȣ��
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
