using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntAttackCtrl : MonoBehaviour
{
    [SerializeField]
    MonsterGoblinKing goblinKing;

    GameObject player;
    bool isAttacked;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        isAttacked = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isAttacked)
            {
                var playerCreature = player.GetComponent<Creature>();
                playerCreature.Hit(goblinKing.finalNormalAtk);
                isAttacked = true;
            }
        }
    }
}
