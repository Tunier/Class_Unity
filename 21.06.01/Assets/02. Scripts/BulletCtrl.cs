using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20f; // ÃÑ¾Ë °ø°Ý·Â
    public float speed = 1000f; // ÃÑ¾Ë ¼Óµµ

    Rigidbody rb;
    Transform tr;
    TrailRenderer trail;
    
    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        // GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
    }

    private void OnDisable()
    {
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }

    void Update()
    {
        
    }
}
