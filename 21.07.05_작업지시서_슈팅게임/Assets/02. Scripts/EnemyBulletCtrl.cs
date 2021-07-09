using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    PlayerCtrl player;

    public float speed;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
    }
    void Start()
    {
        speed = 4f;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Player"))
        {
            if (player.hitable)
            {
                Destroy(gameObject);
                obj.GetComponent<PlayerCtrl>().Hit();
            }
        }

        if (obj.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
