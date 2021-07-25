using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerCtrl : MonoBehaviour
{
    public List<GameObject> mobList = new List<GameObject>();

    public GameObject mob;
    public PlayerCtrl player;

    [SerializeField]
    Map map;

    float spawnDelay;
    float spawnAfterTime;

    int spawnMobLevel;

    Vector3 mobPos; // x -13 ~ -3 / z 6 ~ 17 //추후에 포인트 지정해서 소환으로 변경할것

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        spawnAfterTime = 2f;
        spawnDelay = 6f;
        spawnMobLevel = 1;
    }

    void Update()
    {
        OnOffMonsterSpawn();

        mobPos.x = Random.Range(-13f, -3f);
        mobPos.y = 0f;
        mobPos.z = Random.Range(6f, 17f);
    }

    /// <summary>
    /// 전체 맵에서(나중에 범위를 지정하는것으로 변경할것) 존제하는 몬스터를 리스트로 받아서<br/>
    /// 4마리 이하면 몬스터 스텟을 설정해서 일정 위치 안에 랜덤지점으로 소환함.
    /// </summary>
    public void OnOffMonsterSpawn()
    {
        DeleteNullObj();

        if (map.isHere)
        {
            if (mobList.Count <= 4)
            {
                if (player.level > spawnMobLevel + 1)
                {
                    spawnMobLevel += 1;
                }

                spawnAfterTime += Time.deltaTime;
                if (spawnAfterTime >= spawnDelay)
                {
                    spawnAfterTime = 0;
                    GameObject obj = Instantiate(mob, transform.parent);

                    obj.transform.position = transform.position + mobPos;
                    MonsterCtrl MobCtrl = obj.GetComponent<MonsterCtrl>();
                    MobCtrl.level = spawnMobLevel;
                    MobCtrl.hpMax = (MobCtrl.level - 1) * 25f + 30;
                    MobCtrl.hp = MobCtrl.hpMax;

                    mobList.Add(obj);
                }
            }
        }
        else
        {
            foreach (GameObject mob in mobList)
            {
                Destroy(mob);
            }
        }
    }

    void DeleteNullObj()
    {
        for (int i = 0; i < mobList.Count; ++i)
        {
            if (mobList[i] == null)
                mobList.RemoveAt(i);
        }
    }
}
