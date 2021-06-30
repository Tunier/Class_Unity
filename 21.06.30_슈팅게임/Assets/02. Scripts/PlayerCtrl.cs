using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject bullet;
    public GameObject damageImg;

    float speed;

    float shotDelay;
    float shotAfterTime;

    public int hp;
    public float score;

    public Camera mainCam;

    RaycastHit2D hit;

    Vector3 mousePos;

    void Start()
    {
        speed = 4f;
        shotDelay = 0.45f;
        shotAfterTime = shotDelay;
        hp = 3;
        score = 0f;
    }

    void Update()
    {
        PlayerMovement();

        if (Input.GetKey(KeyCode.X))
        {
            ShootBulletToKey();
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            shotAfterTime = shotDelay;
        }

        if (Input.GetMouseButton(0))
        {
            ShootBulletToMouse();
        }
    }

    public void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, v, 0f).normalized;

        transform.position += movement * speed * Time.deltaTime;
    }

    public virtual void ShootBulletToKey()
    {
        shotAfterTime += Time.deltaTime;
        if (shotAfterTime >= shotDelay)
        {
            shotAfterTime = 0;
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }

    public virtual void ShootBulletToMouse()
    {
        shotAfterTime += Time.deltaTime;
        if (shotAfterTime >= shotDelay)
        {
            shotAfterTime = 0;

            mousePos = Input.mousePosition;

            mousePos = mainCam.ScreenToWorldPoint(mousePos);

            hit = Physics2D.Raycast(mousePos, transform.forward, Mathf.Infinity);

            if (hit)
            {
                GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
                Vector3 v = hit.point;

                Vector3 dir = v - transform.position;
                dir = dir.normalized;
                obj.GetComponent<BulletCtrl>().setDir(dir);

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                obj.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            }
        }
    }

    public void Damaged()
    {
        hp--;
        if (damageImg.activeSelf == false)
            damageImg.SetActive(true);

        if (hp < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (hp < 3)
            damageImg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Damage/playerShip1_damage" + (3 - hp));
    }
}
