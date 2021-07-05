using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    public GameObject[] bullet;
    public GameObject[] item;

    Transform tr;

    float shotAfterTime;
    float shotDelay;

    float moveSpeed;

    float h;
    float v;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }
    void Start()
    {
        moveSpeed = 2f;
        shotAfterTime = 0;
        shotDelay = 1f;
    }

    void Update()
    {
        Movement();
        shot();
    }

    void Movement()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    public virtual void Hit()
    {
        Destroy(gameObject);

        int itemdrop = Random.Range(20, 100);

        if (itemdrop >= 95)
        {
            Instantiate(item[2], tr.position, Quaternion.identity);
        }
        else if (itemdrop >= 85)
        {
            Instantiate(item[1], tr.position, Quaternion.identity);
        }
        else if (itemdrop >= 75)
        {
            Instantiate(item[0], tr.position, Quaternion.identity);
        }
    }

    public virtual void shot()
    {
        shotAfterTime += Time.deltaTime;

        if (shotAfterTime >= shotDelay)
        {
            shotAfterTime = 0;
            GameObject obj = Instantiate(bullet[0], new Vector3(tr.position.x, tr.position.y - 0.4f, tr.position.z), Quaternion.Euler(0, 0, 180));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "DownSideWall")
        {
            Destroy(gameObject);
        }
    }
}
