using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    GameManager gm;

    float speed;

    public int increase_Score = 50;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        speed = 5f;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= -8)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Plane")
        {
            gm.score += increase_Score;
            Destroy(gameObject);
        }
    }
}
