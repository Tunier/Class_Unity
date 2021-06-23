using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public enum SpeedType
    {
        NORMAL,
        DASH
    }

    public SpeedType speedType;

    public Rigidbody rb;
    public float speed;
    public float dashSpeed;
    public int hp = 3;

    float x = 0;
    float z = 0;

    Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        speedType = SpeedType.NORMAL;

        speed = 8f;
        dashSpeed = speed * 1.5f;
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        PlayerMove();

        // addforce = 서서히 힘을 가함, 속도를 임의로 정확하게 지정할수없다.
        // velocity = 속도를 변경함(서서히 가속하지않고 바로 속도가 바뀜)
    }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    public void PlayerMove()
    {
        if (Input.GetKey(KeyCode.X))
            speedType = SpeedType.DASH;
        else
            speedType = SpeedType.NORMAL;

        if (speedType == SpeedType.NORMAL)
            velocity = new Vector3(x, 0f, z) * speed;
        else if (speedType == SpeedType.DASH)
            velocity = new Vector3(x, 0f, z) * dashSpeed;
    }
    public void Die()
    {
        gameObject.SetActive(false);

        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.GameOver();
    }
}
