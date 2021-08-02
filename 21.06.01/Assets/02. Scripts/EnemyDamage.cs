using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    const string bulletTag = "BULLET";

    float hp = 100f; // ü��
    GameObject bloodEffect; // ���� ȿ�� ����

    float initHp = 100f;
    public GameObject hpBarPrefab;
    public Vector3 hpBaroffset = new Vector3(0, 2.2f, 0);
    Canvas uiCanvas;
    Image hpBarImage;

    void Start()
    {
        // Load �Լ��� ���������� Resources ���� �����͸� �ҷ����� �Լ���
        // Load<����������>("������ ���");
        // �ֻ��� ��δ� Resources ������ ex) C ����̺�
        // ������ ��δ� ���� ������ + ���ϸ���� ��Ȯ�ϰ� Ǯ��θ� ���.
        bloodEffect = Resources.Load<GameObject>("Blood");
        SetHpBar();
    }

    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        var hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = gameObject.transform;
        _hpBar.offset = hpBaroffset;
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

            hpBarImage.fillAmount = hp / initHp;
            // ü���� 0 ���ϰ� �Ǹ� ���� �׾��ٰ� �Ǵ�.
            if (hp <= 0)
            {
                // ���� ��ȯ ����.
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                hpBarImage.GetComponentInParent<Image>().color = Color.clear;

                GameManager.instance.IncKillCount();
                GetComponent<CapsuleCollider>().enabled = false;
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
