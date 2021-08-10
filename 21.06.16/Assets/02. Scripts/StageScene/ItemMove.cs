using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();

        Destroy(gameObject, 20f);
    }

    void Update()
    {
        tr.Rotate(new Vector3(0, 120) * Time.deltaTime);
        tr.Translate(new Vector3(0, Mathf.Sin(Time.time * 5)) * Time.deltaTime);
    }
}
