using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefeb;

    float eDelay;
    float dTimer;
    float enemySpawnRand;
    void Start()
    {
        eDelay = 2f;
        dTimer = 0f;
    }

    void Update()
    {
        dTimer += Time.deltaTime;
        if (dTimer >= eDelay)
        {
            dTimer = 0;
            enemySpawnRand = Random.Range(-8f, 8f);
            Instantiate(enemyPrefeb, new Vector3(enemySpawnRand, 6, 0), Quaternion.identity);
        }
    }
}
