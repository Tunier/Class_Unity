using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoSingletone<WeaponManager>
{
    [Header("오브젝트 풀 정보")]
    public GameObject arrowPrefab;
    int maxPool = 20;
    public List<GameObject> arrowPool = new List<GameObject>();

    GameObject arrowPools;

    private void Awake()
    {
        arrowPools = new GameObject("ArrowPools");

        CreatePooling();
    }

    public void CreatePooling()
    {
        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate(arrowPrefab, arrowPools.transform);
            obj.name = "Arrow_" + i.ToString("00");
            obj.SetActive(false);
            arrowPool.Add(obj);
        }
    }

    public GameObject GetArrow()
    {
        for (int i = 0; i < arrowPool.Count; i++)
        {
            if (arrowPool[i].activeSelf == false)
            {
                return arrowPool[i];
            }
        }
        return null;
    }
}