using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    [SerializeField] GameObject go_1;
    [SerializeField] GameObject go_2;

    [SerializeField] Vector3 pos;

    [SerializeField] float angle;

    [SerializeField] float angle2;

    List<GameObject> golist;

    void Awake()
    {
        golist?.Add(gameObject);
        golist?.Add(gameObject);
        golist?.Add(gameObject);

        //golist ??= new List<GameObject>();
    }

    void Update()
    {
        //Vector3 v = go_2.transform.position - go_1.transform.position;
        //angle = Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;

        //angle2 = Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg;
    }
}
