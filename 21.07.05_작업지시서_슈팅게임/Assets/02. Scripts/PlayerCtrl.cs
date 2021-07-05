using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    Transform tr;
    Animator ani;

    public GameObject[] bullet;
    int bulletType;

    public float speed; // 이동속도
    public int power;
    public int life;
    public float score;


    float shotDelay;
    float shotAfterTime;

    readonly int hashMove = Animator.StringToHash("Horizontal");

    float h; // horizontal 수평
    float v; // vertical 수직

    void Start()
    {
        bulletType = 0;

        speed = 3f;
        life = 3;
        power = 1;
        score = 0;
        
        shotAfterTime = 0;

        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerMovement();
        ShotBullet();

        ani.SetInteger(hashMove, (int)h); // 에니메이션 제어
    }

    void PlayerMovement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, v, 0).normalized;

        tr.position += movement * speed * Time.deltaTime;
    }

    void ShotBullet()
    {
        shotDelay = Random.Range(0.2f, 0.3f);

        if (power == 1)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                shotAfterTime = shotDelay;
            }

            if (Input.GetKey(KeyCode.X))
            {
                shotAfterTime += Time.deltaTime;

                if (shotAfterTime >= shotDelay)
                {
                    shotAfterTime = 0;
                    GameObject obj = Instantiate(bullet[bulletType], new Vector3(tr.position.x, tr.position.y + 0.4f, tr.position.z), Quaternion.identity);
                }
            }
        }
        else if (power == 2)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                shotAfterTime = shotDelay;
            }

            if (Input.GetKey(KeyCode.X))
            {
                shotAfterTime += Time.deltaTime;

                if (shotAfterTime >= shotDelay)
                {
                    shotAfterTime = 0;
                    GameObject obj = Instantiate(bullet[bulletType], new Vector3(tr.position.x - 0.1f, tr.position.y + 0.4f, tr.position.z), Quaternion.identity);
                    GameObject obj2 = Instantiate(bullet[bulletType], new Vector3(tr.position.x + 0.1f, tr.position.y + 0.4f, tr.position.z), Quaternion.identity);
                }
            }
        }
    }

    public void Hit()
    {
        life--;
        if (life <= 0)
        {
            life = 0;
            Destroy(gameObject);
        }
    }
}
