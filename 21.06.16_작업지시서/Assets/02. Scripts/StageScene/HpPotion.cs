using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : MonoBehaviour
{
    Transform tr;

    float RecoveryAmount;

    void Start()
    {
        tr = GetComponent<Transform>();

        RecoveryAmount = 20f;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 120) * Time.deltaTime);
        transform.Translate(new Vector3(0, Mathf.Sin(Time.time * 5) * 0.6f, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PLAYER"))
        {
            PlayerCtrl player = other.GetComponent<PlayerCtrl>();
            if (player.hp <= (player.hpMax - RecoveryAmount))
            {
                player.hp += RecoveryAmount;
            }
            else
            {
                player.hp = player.hpMax;
            }

            Destroy(gameObject);
        }
    }
}
