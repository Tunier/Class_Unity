using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    PlayerCtrl player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
    }
    void Start()
    {

    }

    void Update()
    {
        transform.position += Vector3.down * 2 * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Player"))
        {
            if (gameObject.name == "Power(Clone)")
            {
                Destroy(gameObject);
                if (obj.GetComponent<PlayerCtrl>().power < 2)
                    obj.GetComponent<PlayerCtrl>().power++;
                else
                    player.score += 1000;
            }
            else if (gameObject.name == "Boom(Clone)")
            {
                Destroy(gameObject);
                GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject g in enemy)
                {
                    Destroy(g);
                    player.score += 100;
                }
            }
            else if (gameObject.name == "Life(Clone)")
            {
                Destroy(gameObject);
                player.life++;
            }
        }
        else if (collision.name == "DownSideWall")
        {
            Destroy(gameObject);
        }
    }
}
