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

        // addforce = 서서히 힘을 가함, 속도를 임의로 정확하게 지정할수없다.
        // velocity = 속도를 변경함(서서히 가속하지않고 바로 속도가 바뀜)

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
