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

    Vector3 dir; // �̵�����

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
            // ���ʹ� ��+�����̶� ���ͷ� �̵���Ű�� ũ�⿡���� �̵��ӵ� ���̰� ����� ����.
            // �̋� �ʿ��� ���� ����.normalize(���� ����)�̴�
            // ���� ���ʹ� ������ ũ�⸦ 1�� ������ �����̴�.
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
