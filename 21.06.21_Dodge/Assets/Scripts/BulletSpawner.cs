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
        //Time.deltatime = 이전 프레임에서 현재 프레임이 실행되기까지 걸린 시간을 가져온다.

        if (timeAfterSpawn >= spawnDelay)
        {
            timeAfterSpawn -= spawnDelay;
            if (bulletType == 0)
            {
                bullet = Instantiate(bulletPrf, transform.position, transform.rotation);
                // 1. 매개변수로 생성할 게임오브젝트만 넣어주면 0,0,0 위치 0,0,0 각도로 생성
                // 2. 매개변수로 생성할 게임오브젝트와 다른게임오브젝트를 넣으면 두번째로 넣어준 게임오브젝트의 자식으로 생성.
                // 3. 매개변수로 생성할 게임오븢게트, 좌표, 각도를 넣어주면 지정한 좌표에 지정한 각도로 게임 오브젝트 생성.
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
