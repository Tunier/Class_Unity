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

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        mobList = new List<GameObject>();

        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();

        moveSpeed = 10f;
    }
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        rb.velocity = transform.forward * moveSpeed;
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
                if (player.isCrit)
                {
                    obj.GetComponent<MonsterCtrl>().Hit(player.resultDamage * 2f);
                    UIManager.instance.PrintDamageText(player.resultDamage * 2f, true);
                }
                else
                {
                    obj.GetComponent<MonsterCtrl>().Hit(player.resultDamage * 1f);
                    UIManager.instance.PrintDamageText(player.resultDamage * 1f, false);
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
