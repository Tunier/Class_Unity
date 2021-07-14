using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    const string bulletTag = "BULLET";

    float hp = 100f; // ü��
    GameObject bloodEffect; // ���� ȿ�� ����

    void Start()
    {
        // Load �Լ��� ���������� Resources ���� �����͸� �ҷ����� �Լ���
        // Load<����������>("������ ���");
        // �ֻ��� ��δ� Resources ������ ex) C ����̺�
        // ������ ��δ� ���� ������ + ���ϸ���� ��Ȯ�ϰ� Ǯ��θ� ���.
        bloodEffect = Resources.Load<GameObject>("Blood");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == bulletTag)
        {
            // ���� ȿ�� �Լ� ȣ��
            ShowBloodEffect(collision);
            // �Ѿ� ����
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);

            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            // ü���� 0 ���ϰ� �Ǹ� ���� �׾��ٰ� �Ǵ�.
            if (hp <= 0)
            {
                // ���� ��ȯ ����.
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
        }
    }

    void ShowBloodEffect(Collision Coll)
    {
        // �浹 ��ġ �� ��������.
        Vector3 pos = Coll.contacts[0].point;
        // �浹 ��ġ�� ���� ����(�Ѿ��� ���ƿ� ����) ���ϱ�.
        Vector3 _normal = Coll.contacts[0].normal;
        // �Ѿ��� ���ƿ� ���Ⱚ ���.
        Quaternion rot = Quaternion.FromToRotation(Vector3.back, _normal);
        // ���� ȿ�� ���� ����.
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        // 1�� �� ����.
        Destroy(blood, 1f);
    }

    void Update()
    {

    }
}
