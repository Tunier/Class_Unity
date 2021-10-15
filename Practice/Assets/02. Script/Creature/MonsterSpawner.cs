using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    GameObject player;

    public ObjPoolingManager.Monster monster;
    public GameObject[] movepoints;

    public float checkDistance;
    [Range(1, 10)]
    public int maxSpawnCount;
    public int spawnCount;

    public float spawnDelay;
    float spawnOvertime = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        for (int i = 0; i < maxSpawnCount; i++)
        {
            SpawnMonster();
        }
    }

    void Update()
    {
        if (spawnCount < maxSpawnCount)
            spawnOvertime += Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) < checkDistance && spawnOvertime >= spawnDelay && spawnCount < maxSpawnCount)
        {
            spawnOvertime = 0f;
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        float x = Random.Range(-8f, 8f);
        float y = 2f;
        float z = Random.Range(-8f, 8f);

        var obj = ObjPoolingManager.Instance.GetMonsterAtPool(monster);
        var monsterbase = obj.GetComponent<MonsterBase>();
        obj.transform.position = transform.position + new Vector3(x, y, z);
        monsterbase.spawner = this;

        foreach (var movepoint in movepoints)
        {
            if (!monsterbase.movePoints.Contains(movepoint.transform))
            {
                monsterbase.movePoints.Add(movepoint.transform);
            }
        }

        obj.SetActive(true);

        spawnCount++;
    }
}