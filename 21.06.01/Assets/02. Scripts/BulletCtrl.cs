using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20f; // �Ѿ� ���ݷ�
    public float speed = 1000f; // �Ѿ� �ӵ�

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
        // GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    void Update()
    {
        
    }
}
