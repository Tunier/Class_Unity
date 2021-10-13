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
    // 캐싱이라고해서 미리 선점해두기
    // 이유는 성능향상
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

    [PunRPC] // RPC 함수라는것을 명확하게 명시해주어야 동작함.
    void FireBullet()
    {
        // 총구의 화염효과가 실행중일 때는 화염효과 실행 안되도록
        // 중복 실행 안되도록 하는 구문.
        if (!muzzleFalsh.isPlaying)
            muzzleFalsh.Play(true);

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}
