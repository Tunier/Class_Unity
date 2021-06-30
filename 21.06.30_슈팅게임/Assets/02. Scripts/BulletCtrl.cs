using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public GameObject bulletEff;
    PlayerCtrl player;

    float speed;

    string targetTag = "Enemy"; // 총알이 충돌할 대상의 태그값

    Vector3 dir = Vector3.up;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        speed = 8f;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        if (transform.position.y >= 6)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            // 충돌지점 좌표 반환.

            GameObject eff = Instantiate(bulletEff, contactPoint, Quaternion.identity);
            Destroy(eff, 0.2f);

            if (collision.tag == "Player")
            {
                collision.GetComponent<PlayerCtrl>().Damaged();
            }
            else
            {
                Destroy(collision.gameObject);
                player.score += 100;
            }

            Destroy(gameObject);
        }
    }

    public void setDir(Vector3 v)
    {
        dir = v;
    }

    public void setTargetTeg(string str)
    {
        targetTag = str;
    }
}
