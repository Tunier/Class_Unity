using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 8f;

    float x = 0;
    float z = 0;

    Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // addforce = ������ ���� ����, �ӵ��� ���Ƿ� ��Ȯ�ϰ� �����Ҽ�����.
        // velocity = �ӵ��� ������(������ ���������ʰ� �ٷ� �ӵ��� �ٲ�)

        velocity = new Vector3(x, 0f, z) * speed;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }
}
