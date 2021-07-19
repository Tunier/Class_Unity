using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// struct = 구조체, 클래스 열화판으로 생각하면 편함. stack 영역에 보관됨.
// 클래스는 구조체 이후 등장한 개념.
// 그러나 지금에 와서는 메모리 적재 형태의 차이만 있을뿐 기능상 큰 차이는 없음.
[System.Serializable] // 직렬화 인스펙터에 표시.
public struct PlayerSfx
{
    // 배열을 쓰는 이유 = 무기가 변경되면 사운드도 변경하기 위해서 사용.
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType // 열거형.
    {
        RIFLE = 0,
        SHOTGUN
    }
    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bullet; // 총알 프리팹 사용하기 위한 변수.
    public Transform firePos; // 총알 발사 위치.
    public ParticleSystem cartidge; // 탄피 프리팹.
    private ParticleSystem muzzleFlash; // 총구 화염 파티클.

    AudioSource _audio;
    public PlayerSfx playerSfx; // 오디오 클립 저장 변수.

    Shake shake;

    public Image magazineImg;
    public Text magazineText;

    public int maxBullet = 10;
    public int remainingBullet = 10;

    public float reloadTime = 2f;
    bool isReloading = false;

    public Sprite[] weaponIcons;
    public Image weaponImage;

    public void OnChangeWeapon()
    {
        currWeapon++;
        currWeapon = (WeaponType)((int)currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }

    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>(muzzleFlash);
        _audio = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        // 0이면 좌클린 1이면 우클릭
        // GetMouseButtonDown 함수는 눌렀을때 한번만 동작.
        if (!isReloading && Input.GetMouseButtonDown(0))
        {
            remainingBullet--;

            // 공격함수 호출
            Fire();

            if (remainingBullet <= 0)
            {
                //재장전 코루틴 함수
            }
        }
    }

    void Fire()
    {
        // shake 스크립트 내부의 shakeCamera 코루틴 함수 호출
        // 매개변수 값을 생략했으므로 ShakeCamera의 기본값을 적용.
        StartCoroutine(shake.ShakeCamera());

        // Instantiate(동적생성할 오브젝트, 위치, 방향);
        // 사용되지 않는 객체(Object)를 활성화 해주는 함수.
        //Instantiate(bullet, firePos.position, firePos.rotation);

        var _bullet = GameManager.instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        cartidge.Play(); // 탄피 파티클 재생
        muzzleFlash.Play(); // 총구 화염 파티클 재생

        FireSfx(); // 공격시 사운드 발생.

        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        UpdateBulletText();

        if (remainingBullet == 0)
        {
            StartCoroutine(Reloading());
        }
    }

    void FireSfx()
    {
        // 현재 선택된 무기의 넘버에 맞는 사운드를 선택해서 가지고옴.
        var _sfx = playerSfx.fire[(int)currWeapon];
        // Play = 소리 조절 불가, PlayOneShot 소리 조절 가능.
        _audio.PlayOneShot(_sfx, 1f);
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        _audio.PlayOneShot(playerSfx.reload[(int)currWeapon], 1f);

        float audioLegth = playerSfx.reload[(int)currWeapon].length + 0.3f;
        yield return new WaitForSeconds(audioLegth);

        isReloading = false;
        magazineImg.fillAmount = 1f;
        remainingBullet = maxBullet;

        UpdateBulletText();
    }

    void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#00ff00>{0}</color>/{1}", remainingBullet, maxBullet);
    }
}
