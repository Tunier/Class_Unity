using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletCtrl : MonoBehaviour
{
    public float speed;

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
            Destroy(gameObject);
            obj.GetComponent<PlayerCtrl>().Hit();
        }
        else if (obj.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
