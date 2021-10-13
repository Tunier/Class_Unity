using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolingManager : MonoSingletone<ObjPoolingManager>
{
    public enum Obj
    {
        GoblinHunterArrow,
        GoblinKingSlah,
    }
    public enum Effect
    {
        RunEffect,
    }

    public enum Monster
    {
        Pig,
        GoblinHunter,
        GoblinKing,
        SkeltonWarrior,
    }

    [Header("풀링할 오브젝트들")]
    public GameObject go_goblinHunterArrow;
    public GameObject go_monsterPig;
    public GameObject go_monsterGoblinHunter;
    public GameObject go_monsterGoblinKing;
    public GameObject go_monsterSkeletonWarrior;

    [Header("풀링할 이펙트들")]
    public GameObject Run_Effect;
    public GameObject goblinKingSlash_Effect;

    int maxcount = 50;

    [Header("풀링된 오브젝트들 리스트")]
    public List<GameObject> arrowPool = new List<GameObject>();
    public List<GameObject> pigPool = new List<GameObject>();
    public List<GameObject> goblinHunterPool = new List<GameObject>();
    public List<GameObject> goblinKingPool = new List<GameObject>();
    public List<GameObject> skeletonWarriorPool = new List<GameObject>();

    [Header("풀링된 이펙트들 리스트")]
    public List<GameObject> RunEffectPool = new List<GameObject>();
    public List<GameObject> slashPool = new List<GameObject>();

    private void Awake()
    {
        CreatePool(go_goblinHunterArrow, maxcount);
        CreatePool(goblinKingSlash_Effect, 10);
        CreatePool(go_monsterPig, maxcount);
        CreatePool(go_monsterGoblinHunter, maxcount);
        CreatePool(go_monsterGoblinKing, 1);
        CreatePool(go_monsterSkeletonWarrior, maxcount);
        CreatePool(Run_Effect, 10);
    }

    void CreatePool(GameObject _obj, int _count)
    {
        var pool = new GameObject(_obj.name + "Pool");

        for (int i = 0; i < _count; i++)
        {
            var obj = Instantiate(_obj);
            obj.SetActive(false);
            obj.name = _obj.name + " " + (i + 1).ToString("00");
            obj.transform.SetParent(pool.transform);

            if (obj.name.Contains("Goblin_Arrow"))
            {
                arrowPool.Add(obj);
            }
            else if (obj.name.Contains("Goblin_Slash"))
            {
                slashPool.Add(obj);
            }
            else if (obj.name.Contains("Pig"))
            {
                pigPool.Add(obj);
            }
            else if (obj.name.Contains("Goblin_Hunter"))
            {
                goblinHunterPool.Add(obj);
            }
            else if (obj.name.Contains("Skeleton_Warrior"))
            {
                skeletonWarriorPool.Add(obj);
            }
            else if (obj.name.Contains("RunEffect"))
            {
                RunEffectPool.Add(obj);
            }
            else if (obj.name.Contains("Goblin_King"))
            {
                goblinKingPool.Add(obj);
            }
        }
    }

    public GameObject GetMonsterAtPool(Monster _monster)
    {
        switch (_monster)
        {
            case Monster.Pig:
                foreach (var obj in pigPool)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            case Monster.GoblinHunter:
                foreach (var obj in goblinHunterPool)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            case Monster.SkeltonWarrior:
                foreach (var obj in skeletonWarriorPool)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            case Monster.GoblinKing:
                foreach (var obj in goblinKingPool)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            default:
                Debug.LogError("없는몬스터를 받아가려고함.");
                return null;
        }
    }

    public GameObject GetObjAtPool(Obj _obj)
    {
        switch (_obj)
        {
            case Obj.GoblinHunterArrow:
                foreach (var obj in arrowPool)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            case Obj.GoblinKingSlah:
                foreach (var obj in slashPool)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            default:
                return null;
        }
    }

    public GameObject GetEffectAtPool(Effect _effect)
    {
        switch (_effect)
        {
            case Effect.RunEffect:
                foreach (var obj in RunEffectPool)
                {
                    if (!obj.activeSelf)
                        return obj;
                }
                return null;
            default:
                return null;
        }
    }
}