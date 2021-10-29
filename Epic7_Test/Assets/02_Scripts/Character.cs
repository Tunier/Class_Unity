using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityPointType
{
    None,
    Consentration,
    Rage,
}

public enum ElementType
{
    None,
    Advantage,
    Fire,
    Water,
    Grass,
    Light,
    Dark,
}

public enum JobType
{
    Warrier,
    Assassin,
    Archer,
    SpiritualSorcerer,
}

public class Character : MonoBehaviour
{
    public int maxHp { get; protected set; }
    public int curHp
    {
        get => curHp;
        protected set
        {
            curHp = value;
            // UI반영코드 필요.
        }
    }

    public AbilityPointType apType { get; protected set; } = AbilityPointType.None;
    public int? maxAp
    {
        get => apType == AbilityPointType.None ? null : maxAp;
        protected set
        {
            if (apType == AbilityPointType.None)
            {
                Debug.Log("ApType이 None이라 적용이 안됨.");
                maxAp = null;
            }
            else
            {
                maxAp = value;
                // UI반영코드 필요.
            }
        }
    }
    public int? curAp
    {
        get => apType == AbilityPointType.None ? null : curAp;
        protected set
        {
            if (apType == AbilityPointType.None)
            {
                Debug.Log("ApType이 None이라 적용이 안됨.");
                curAp = null;
            }
            else
            {
                curAp = value;
                // UI반영코드 필요.
            }
        }
    }

    public int level { get; protected set; } = 1;
    public ElementType elementType { get; protected set; }
    public JobType jobType { get; protected set; }
    public int dmg { get; protected set; }
    public int def { get; protected set; }
    public int speed { get; protected set; }
    public int critChance { get; protected set; }
    public int critDamageRate { get; protected set; }
    public int effectHitChance { get; protected set; }
    public int resiChance { get; protected set; }
    public int evasionChance { get; protected set; }

    public delegate void hitDel(Character _char);

    public event hitDel hitEvnet;

    void Awake()
    {
        hitEvnet += Hit;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_dmg"></param>
    /// <param name="_elementType"></param>
    protected virtual void Hit(Character _char)
    {
        var attaker = _char;

        switch (attaker.elementType)
        {
            case ElementType.None:
                curHp -= attaker.dmg - def;
                break;

            case ElementType.Advantage:
                curHp -= Mathf.RoundToInt((attaker.dmg - def) * 1.1f);
                break;

            case ElementType.Fire:
                if (elementType == ElementType.Grass)
                    curHp -= Mathf.RoundToInt((attaker.dmg - def) * 1.1f);
                else
                    curHp -= attaker.dmg - def;
                break;

            case ElementType.Water:
                if (elementType == ElementType.Fire)
                    curHp -= Mathf.RoundToInt((attaker.dmg - def) * 1.1f);
                else
                    curHp -= attaker.dmg - def;
                break;

            case ElementType.Grass:
                if (elementType == ElementType.Water)
                    curHp -= Mathf.RoundToInt((attaker.dmg - def) * 1.1f);
                else
                    curHp -= attaker.dmg - def;
                break;

            case ElementType.Light:
                if (elementType == ElementType.Dark)
                    curHp -= Mathf.RoundToInt((attaker.dmg - def) * 1.1f);
                else
                    curHp -= attaker.dmg - def;
                break;

            case ElementType.Dark:
                if (elementType == ElementType.Light)
                    curHp -= Mathf.RoundToInt((attaker.dmg - def) * 1.1f);
                else
                    curHp -= attaker.dmg - def;
                break;
        }
    }
}
