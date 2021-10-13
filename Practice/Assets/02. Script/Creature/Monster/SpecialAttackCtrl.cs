using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackCtrl : MonoBehaviour
{
    public MonsterGoblinKing goblinKing;
    GameObject player;

    float delayTime = 0;
    Rigidbody rb;

    Vector3 shotRot;

    bool isAttacked;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        delayTime = 0;
        shotRot = (new Vector3(transform.position.x, transform.position.y, transform.position.z) - new Vector3(player.transform.position.x, 0, player.transform.position.z)).normalized;
        isAttacked = false;
    }

    void Update()
    {
        delayTime += Time.deltaTime;
        if (delayTime >= 0.5f)
        {
            delayTime -= delayTime;
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * 1f, ForceMode.Impulse);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isAttacked == false)
            {
                isAttacked = true;
                var playerCreature = player.GetComponent<Creature>();
                playerCreature.Hit(goblinKing.finalNormalAtk);
            }
        }
    }
}
