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
        // �浹 �������� ����Ʈ �߻�(�Ѿ��� �������� �ݴ��������)
        var obj = Instantiate(effect, contact.point, Quaternion.LookRotation(-contact.normal));

        Destroy(obj, 2f);
        Destroy(gameObject);
    }
}
