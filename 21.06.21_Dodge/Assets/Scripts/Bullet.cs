using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 6f;
    
    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCtrl player = other.GetComponent<PlayerCtrl>();
            player.hp -= 1;

            Destroy(gameObject);

            if (player.hp <= 0)
            {
                player.Die();
            }
        }
        else if (other.CompareTag("WALL"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
