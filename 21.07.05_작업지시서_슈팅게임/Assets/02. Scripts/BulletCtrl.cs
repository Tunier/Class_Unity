using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    PlayerCtrl player;

    public float speed;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }
    void Start()
    {
        speed = 4f;
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            obj.GetComponent<EnemyCtrl>().Hit();
            player.score += 100;
        }
        else if (obj.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
