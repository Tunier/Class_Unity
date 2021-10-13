using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttackCtrl : MonoBehaviour
{
    [SerializeField]
    MonsterGoblinKing goblinKing;
    [SerializeField]
    GameObject attackEffect;

    GameObject player;
    bool isAttacked;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
        isAttacked = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isAttacked)
            {
                attackEffect.SetActive(true);
                var playerCreature = player.GetComponent<Creature>();
                playerCreature.Hit(goblinKing.finalNormalAtk);
                isAttacked = true;
            }
        }
    }
    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.2f);
        attackEffect.SetActive(false);
        gameObject.SetActive(false);
    }

}
