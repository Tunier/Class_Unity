using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : Creature
{
    public int dropGold { get; protected set; }
    public float exp { get; protected set; }

    public bool isAnger { get; set; } = false;        // ���� : Anger true �İ� : Anger false

    public List<Transform> movePoints;  // ��������Ʈ
    public MonsterSpawner spawner;      // ������ ������ ������

    public abstract void DropItem();
}