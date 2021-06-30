using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    GameObject target;
    public GameObject Bullet;

    float shotDelay;
    float shotAfterTime;

    float angle;

    public float speed;

    Vector3 dir; // 이동방향

    void Start()
    {
        target = GameObject.Find("Player");
        speed = 2f;
        shotDelay = 2f;
        shotAfterTime = 0f;
    }

    void Update()
    {
        if (target != null)
        {
            dir = target.transform.position - transform.position;

            dir = dir.normalized;

            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            // 벡터는 힘+방향이라 벡터로 이동시키면 크기에따라 이동속도 차이가 생길수 있음.
            // 이떄 필요한 것이 벡터.normalize(단위 벡터)이다
            // 단위 벡터는 벡터의 크기를 1로 통일한 벡터이다.
            Movement();
            ShootBullet();
        }
    }

    void Movement()
    {
        transform.position += dir * speed * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    public virtual void ShootBullet()
    {
        shotAfterTime += Time.deltaTime;
        if (shotAfterTime >= shotDelay)
        {
            shotAfterTime = 0;

            GameObject obj = Instantiate(Bullet, transform.position, Quaternion.identity);
            obj.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            obj.GetComponent<BulletCtrl>().setTargetTeg("Player");
            obj.GetComponent<BulletCtrl>().setDir(dir);
        }
    }
}
