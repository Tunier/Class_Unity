using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject effect;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000f);
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);
        // 충돌 지점에서 이펙트 발생(총알의 진행방향과 반대방향으로)
        var obj = Instantiate(effect, contact.point, Quaternion.LookRotation(-contact.normal));

        Destroy(obj, 2f);
        Destroy(gameObject);
    }
}
