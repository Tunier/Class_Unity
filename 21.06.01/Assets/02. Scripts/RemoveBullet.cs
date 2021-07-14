using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; // ����ũ ������.

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� �߻��� �͵� �߿��� BULLET �ױ׸� ����.
        if (collision.collider.tag == "BULLET")
        {
            // ����ũ ����Ʈ �Լ� ȣ��.
            ShowEffect(collision);
            // �浹�� �߻��� ������Ʈ ����
            // Destroy(collision.gameObject); // �ٷ� ����.
            // Destroy(collision.gameObject, 5f); // �������� ����.
            collision.gameObject.SetActive(false); // �浹�ϸ� ������Ʈ�� ��Ȱ��ȭ��.
        }

    }
    void ShowEffect(Collision coll)
    {
        // �浹 ������ ������ �������.
        // �浹 �� �߻��� ������ ��ġ ����.
        ContactPoint contact = coll.contacts[0];
        // FromToRotation(ȸ����Ű���� �ϴ� ����, Ÿ�� ����)
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        // �浹�� �� �� ����Ʈ�� ȿ�� ������ ��������(�Ѿ��� ���ƿ� ����)���� ����.
        // ��������(�Ѿ��� ���ƿ� ���� -Z)���� ���� ������ ����.
        // �Ѿ��� �߻�� ��ġ�� �̵�(�巳�뿡�� ���� ����� �Ѿ��ڱ� ����)
        Vector3 point = contact.point + (-contact.normal * 0.01f);
        GameObject spark = Instantiate(sparkEffect, point, rot);
        // ���������� ����Ʈ�� �θ�� �巳���� ����.
        spark.transform.SetParent(this.transform);
    }
}
