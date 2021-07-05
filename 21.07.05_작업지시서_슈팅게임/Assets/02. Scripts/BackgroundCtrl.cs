using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    float movespeed;

    void Start()
    {
        movespeed = 2f;
    }

    void Update()
    {
        transform.Translate(Vector3.down * movespeed * Time.deltaTime);

        if (transform.position.y <= -12f)
        {
            transform.position = Vector3.up * 11.9f;
        }
    }
}
