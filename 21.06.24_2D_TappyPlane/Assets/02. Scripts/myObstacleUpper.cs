using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myObstacleUpper : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += Vector3.left * 4 * Time.deltaTime;
        if (transform.position.x <= -8)
        {
            transform.position += Vector3.right * 16;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Plane")
        {
            print("Hit");
            collision.GetComponent<PlayerScr>().Call_Hit();
        }
    }
}
