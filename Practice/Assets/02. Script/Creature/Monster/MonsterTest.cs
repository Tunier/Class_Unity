using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonsterBase
{
    PlayerInfo player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();
    }

    public override void Die()
    {
        Debug.Log("»ç¸Á");
        gameObject.SetActive(false);
        player.stats.CurExp += 100;
    }

    public override void DropItem()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(float _damage)
    {
        curHp -= _damage;

        if (curHp <= 0)
            Die();
    }

    void Start()
    {
        finalMaxHp = 1000;
        curHp = finalMaxHp;
    }
}
