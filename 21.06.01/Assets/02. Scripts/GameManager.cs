using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Transform[] points;
    public GameObject enemy;

    public float creatTime = 2f;
    public int maxEnemy = 10;
    public bool isGameOver = false;

    [Header("오브젝트풀 정보")]
    public GameObject bulletPrefeb;
    int maxPool = 10;
    public List<GameObject> bulletpool = new List<GameObject>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        CreatePolling();
    }

    void Start()
    {
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(CreateEnemy());
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletpool)
        {
            if (bullet.activeSelf == false)
            {
                return bullet;
            }
        }
        return null;
    }

    public void CreatePolling()
    {
        GameObject ObjectPools = new GameObject("ObjectPools");

        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate<GameObject>(bulletPrefeb, ObjectPools.transform);

            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            bulletpool.Add(obj);
        }
    }

    void Update()
    {

    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(creatTime);

                int idx = Random.Range(1, points.Length);
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
        }
    }
}
