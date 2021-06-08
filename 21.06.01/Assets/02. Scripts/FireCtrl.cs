using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet; // 총알 프리팹 사용하기 위한 변수.
    public Transform firePos; // 총알 발사 위치.
    public ParticleSystem cartidge; // 탄피 프리팹.
    private ParticleSystem muzzleFlash; // 총구 화염 파티클.

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>(muzzleFlash);
    }

    // Update is called once per frame
    void Update()
    {
        // 0이면 좌클린 1이면 우클릭
        // GetMouseButtonDown 함수는 눌렀을때 한번만 동작.
        if (Input.GetMouseButtonDown(0))
        {
            // 공격함수 호출
            Fire();
        }
    }

    void Fire()
    {
        // Instantiate(동적생성할 오브젝트, 위치, 방향);
        // 사용되지 않는 객체(Object)를 활성화 해주는 함수.
        Instantiate(bullet, firePos.position, firePos.rotation);
        cartidge.Play(); // 탄피 파티클 재생
        muzzleFlash.Play(); // 총구 화염 파티클 재생
    }
}
