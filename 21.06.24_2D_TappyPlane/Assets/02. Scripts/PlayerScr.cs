using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScr : MonoBehaviour
{
    float g_velocity; // �߷°��ӵ�

    float upPower;
    bool hitable; // ������ �ִ°� ���°�.
    public int hp;

    Rigidbody2D rb;

    public GameManager gm;

    public GameObject[] hpIcon;

    void Start()
    {
        g_velocity = 0;
        upPower = 2f;
        rb = GetComponent<Rigidbody2D>();
        hitable = true;
        hp = 3;
    }

    void Update()
    {
        /*
        0 : ���콺 ���� ��ư
        1 : ���콺 ������ ��ư
        2 : ���콺 �� ��ư
        3~6 : ���콺�� �޸� �߰� ��ư
        */
        if (Input.GetMouseButtonDown(0))
        {
            rb.AddForce(Vector2.up * upPower, ForceMode2D.Impulse);
            // Impulse ���������� ���� ���� �ִ� ���. (���� � ���)
            // Force ���� ��ü�� ��� �۶߸��� ���. (ĳ���� �̵��� ���)
        }
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
        // ������ٵ� �Ⱦ��� �߷±���.
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
}
