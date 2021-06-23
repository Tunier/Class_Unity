using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrf;
    public GameObject bulletPrf2;
    public GameObject bulletPrf3;
    public GameObject bulletPrf4;

    float spawnRateMin = 0.5f;
    float spawnRateMax = 3f;
    
    int bulletType;

    private Transform target;
    private float spawnDelay;
    private float timeAfterSpawn;

    private GameObject bullet;



    void Start()
    {
        timeAfterSpawn = 0f;
        spawnDelay = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerCtrl>().transform;
        bulletType = 0;
    }

    void Update()
    {
        bulletType = Random.Range(0, 4);
        
        timeAfterSpawn += Time.deltaTime;
        //Time.deltatime = ���� �����ӿ��� ���� �������� ����Ǳ���� �ɸ� �ð��� �����´�.

        if (timeAfterSpawn >= spawnDelay)
        {
            timeAfterSpawn -= spawnDelay;
            if (bulletType == 0)
            {
                bullet = Instantiate(bulletPrf, transform.position, transform.rotation);
                // 1. �Ű������� ������ ���ӿ�����Ʈ�� �־��ָ� 0,0,0 ��ġ 0,0,0 ������ ����
                // 2. �Ű������� ������ ���ӿ�����Ʈ�� �ٸ����ӿ�����Ʈ�� ������ �ι�°�� �־��� ���ӿ�����Ʈ�� �ڽ����� ����.
                // 3. �Ű������� ������ ���ӿ�����Ʈ, ��ǥ, ������ �־��ָ� ������ ��ǥ�� ������ ������ ���� ������Ʈ ����.
            }
            else if (bulletType == 1)
            {
                bullet = Instantiate(bulletPrf2, transform.position, transform.rotation);
            }
            else if (bulletType == 2)
            {
                bullet = Instantiate(bulletPrf3, transform.position, transform.rotation);
            }
            else if (bulletType == 3)
            { 
                bullet = Instantiate(bulletPrf4, transform.position, transform.rotation);
            }

            //bullet.transform.LookAt(target);
            bullet.transform.forward = target.position - transform.position;

            spawnDelay = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
