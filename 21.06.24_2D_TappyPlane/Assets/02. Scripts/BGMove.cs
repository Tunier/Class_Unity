using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMove : MonoBehaviour
{
    [SerializeField]
    float movespeed;

    public float limitPos;
    public float movePos;

    void Start()
    {
        movespeed = 2f;
    }

    void Update()
    {
        transform.position += Vector3.left * movespeed * Time.deltaTime;

        if (transform.position.x <= limitPos)
        {
            transform.position += Vector3.right * movePos;
        }
    }
}
