using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IHit
{
    public void Hit(float _damage);
}

interface IDie
{
    public void Die();
}

[System.Serializable]
public class Stats
{
    public string s_Name;

    public int Level;
    public int Skill_Point;
    public float CurExp;
    public float MaxExp;
    public float MaxHp;
    public float MaxMp;
    public float Str;
    public float Int;

    public float Pos_x;
    public float Pos_y;
    public float Pos_z;
    public float Rot_y;

    public int Gold;
}

[System.Serializable]
public enum STATE
{
    Idle,
    Walk,
    Run,
    Patrol,
    Chase,
    Attacking,
    Backoff,
    Backing,
    Jump,
    JumpAndAttack,
    Rolling,
    Die,
    Hit,
}

public abstract class Creature : MonoBehaviour, IHit, IDie
{
    public STATE state { get; set; } = STATE.Idle;
    public Stats stats { get; set; } = new Stats();

    [SerializeField]
    public float finalMaxHp { get; protected set; }
    [ContextMenuItem("hp ¸®¼Â", "HpReset")]
    public float curHp;

    public float finalHpRegen { get; protected set; }

    public float finalNormalAtk { get; protected set; }
    public float finalMagicAtk { get; protected set; }
    public float finalNormalDef { get; protected set; }
    public float finalMagicDef { get; protected set; }

    public abstract void Hit(float _damage);

    public abstract void Die();

    public void HpReset()
    {
        curHp = finalMaxHp;
    }
}