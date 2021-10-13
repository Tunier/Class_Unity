using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadAttackCtrl : MonoBehaviour
{
    [SerializeField]
    MonsterFootman footMan;

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
                var playerCreature = player.GetComponent<Creature>();
                playerCreature.Hit(footMan.finalNormalAtk);
                isAttacked = true;
            }
        }
    }
    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

}
