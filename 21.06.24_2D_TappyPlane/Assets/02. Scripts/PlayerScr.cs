using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour
{
    float g_velocity; // 중력가속도

    float upPower;
    bool hitable; // 맞을수 있는가 없는가.
    public int hp;
    float rotSpeed;

    Rigidbody2D rb;

    public GameManager gm;

    public GameObject[] hpIcon;

    void Start()
    {
        g_velocity = 0;
        upPower = 3f;
        hitable = true;
        hp = 3;
        rotSpeed = 20f;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*
        0 : 마우스 왼쪽 버튼
        1 : 마우스 오른쪽 버튼
        2 : 마우스 휠 버튼
        3~6 : 마우스에 달린 추가 버튼
        */
        if (Input.GetMouseButtonDown(0))
        {
            rb.AddForce(Vector2.up * upPower, ForceMode2D.Impulse);
            // Impulse 순간적으로 강한 힘을 주는 방식. (점프 등에 사용)
            // Force 힘을 전체에 고루 퍼뜨리는 방식. (캐릭터 이동에 사용)
            if (transform.eulerAngles.z <= 35f)
                transform.Rotate(0, 0, rotSpeed);
            else
                transform.rotation = Quaternion.Euler(0, 0, 35f);
        }

        if (transform.eulerAngles.z >= -35f)
            transform.Rotate(0, 0, rotSpeed * -3.5f * Time.deltaTime);
        else
            transform.rotation = Quaternion.Euler(0, 0, -35f);
    }

    public IEnumerator isHit()
    {
        if (hitable == true)
        {
            hp--;
            hitable = false;

            damage();

            if (hp <= 0)
            {
                gm.gameOverFunc();
            }

            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                }

                yield return new WaitForSeconds(0.2f);
            }
            hitable = true;
        }
        yield return null;
    }

    void MakeGravity()
    {
        // 리지드바디 안쓰고 중력구현.
        g_velocity += Time.deltaTime;
        transform.position += Vector3.down * g_velocity * Time.deltaTime;
    }

    public void Call_Hit()
    {
        StartCoroutine(isHit());
    }

    void damage()
    {
        hpIcon[hp].SetActive(false);
    }

    public void Healing()
    {
        if (hp < 3)
        {
            hp++;
            hpIcon[hp].SetActive(true);
        }
    }
}
