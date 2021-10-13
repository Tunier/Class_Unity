using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : Creature
{
    public int dropGold { get; protected set; }
    public float exp { get; protected set; }

    public bool isAnger { get; set; } = false;        // 선공 : Anger true 후공 : Anger false

    public List<Transform> movePoints;  // 무브포인트
    public MonsterSpawner spawner;      // 본인이 스폰된 스포너

    public abstract void DropItem();
}