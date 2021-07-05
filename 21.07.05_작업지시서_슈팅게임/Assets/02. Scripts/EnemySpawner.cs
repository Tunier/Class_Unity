using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemy;

    Vector3 spawnPoint;

    float spawnDelay;
    float spawnAfterTime;

    float h; // ¼öÆò

    void Start()
    {
        spawnDelay = 2f;
        spawnAfterTime = 0;
    }

    void Update()
    {
        h = Random.Range(-2.5f, 2.5f);

        spawnPoint = new Vector3(h, transform.position.y, transform.position.z);

        spawnAfterTime += Time.deltaTime;
        if (spawnAfterTime >= spawnDelay)
        {
            spawnAfterTime = 0;
            GameObject obj = Instantiate(enemy[0], spawnPoint, Quaternion.identity);
        }
    }
}
