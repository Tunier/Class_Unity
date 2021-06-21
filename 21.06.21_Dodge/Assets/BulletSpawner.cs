using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrf;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerCtrl>().transform;        
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;
        //Time.deltatime = ���� �����ӿ��� ���� �������� ����Ǳ���� �ɸ� �ð��� �����´�.

        if (timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn -= spawnRate;

            GameObject bullet = Instantiate(bulletPrf, transform.position, transform.rotation);

            //bullet.transform.LookAt(target);

            bullet.transform.forward = target.position - transform.position;

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
