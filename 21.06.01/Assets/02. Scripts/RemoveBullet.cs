using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� �߻��� �͵� �߿��� BULLET �ױ׸� ����.
        if (collision.collider.tag == "BULLET")
        {
            // �浹�� �߻��� ������Ʈ ����
            Destroy(collision.gameObject); // �ٷ� ����.
            // Destroy(collision.gameObject, 5f); // �������� ����.
            // collision.gameObject.SetActive(false); // �浹�ϸ� ������Ʈ�� ��Ȱ��ȭ��.
        }
    }
}
