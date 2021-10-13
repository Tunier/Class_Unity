using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    ParticleSystem muzzleFalsh;

    PhotonView pv;
    // ĳ���̶���ؼ� �̸� �����صα�
    // ������ �������
    bool isMouseClick => Input.GetMouseButtonDown(0);

    void Start()
    {
        pv = GetComponent<PhotonView>();
        muzzleFalsh = firePos.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (pv.IsMine && isMouseClick)
        {
            FireBullet();
            pv.RPC("FireBullet", RpcTarget.Others, null);
        }
    }

    [PunRPC] // RPC �Լ���°��� ��Ȯ�ϰ� ������־�� ������.
    void FireBullet()
    {
        // �ѱ��� ȭ��ȿ���� �������� ���� ȭ��ȿ�� ���� �ȵǵ���
        // �ߺ� ���� �ȵǵ��� �ϴ� ����.
        if (!muzzleFalsh.isPlaying)
            muzzleFalsh.Play(true);

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}
