using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet; // �Ѿ� ������ ����ϱ� ���� ����.
    public Transform firePos; // �Ѿ� �߻� ��ġ.
    public ParticleSystem cartidge; // ź�� ������.
    private ParticleSystem muzzleFlash; // �ѱ� ȭ�� ��ƼŬ.

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>(muzzleFlash);
    }

    // Update is called once per frame
    void Update()
    {
        // 0�̸� ��Ŭ�� 1�̸� ��Ŭ��
        // GetMouseButtonDown �Լ��� �������� �ѹ��� ����.
        if (Input.GetMouseButtonDown(0))
        {
            // �����Լ� ȣ��
            Fire();
        }
    }

    void Fire()
    {
        // Instantiate(���������� ������Ʈ, ��ġ, ����);
        // ������ �ʴ� ��ü(Object)�� Ȱ��ȭ ���ִ� �Լ�.
        Instantiate(bullet, firePos.position, firePos.rotation);
        cartidge.Play(); // ź�� ��ƼŬ ���
        muzzleFlash.Play(); // �ѱ� ȭ�� ��ƼŬ ���
    }
}
