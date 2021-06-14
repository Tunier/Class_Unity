using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    AudioSource audio;
    Animator animator;
    Transform playerTr;
    Transform enemyTr;

    readonly int hashFire = Animator.StringToHash("Fire");
    readonly int hashReload = Animator.StringToHash("Reload");

    // 공격 관련된 변수
    float nextFire = 0f;
    readonly float fireRate = 0.1f; // 발사 간격.
    readonly float damping = 10f; // 회전 속도 계수.

    public bool isFire = false; // 총알 발사 여부 판단.
    public AudioClip fireSfx; // 총알 발사 사운드.

    // 재장전 관련 변수
    readonly float reloadTime = 2f; // 재장전 시간
    readonly int maxBullet = 10; // 탄창 최대 총알 수
    int currBullet = 10; // 현재 총알 수
    bool isReload; // 재장전 여부
    WaitForSeconds wsReload; // 지연 시간 변수
    public AudioClip reloadSfx; // 재장전 소리

    public GameObject Bullet;
    public Transform firePos;

    public MeshRenderer muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);

        // 게임 시작 할 때 머즐 플래시 비활성화.
        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 공격 신호가 들어오면 실행.
        if (!isReload && isFire)
        {
            // Time.time은 겜플레이 이후 실행된 시간.
            if (Time.time >= nextFire)
            {
                // 공격 함수 호출
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0f, 0.3f);
            }
            // 플레이어가 있는 위치의 회전각도 계산.
            // A 벡터 - B 벡터 = B에서 A까지의 방향과 거리
            // B 벡터 - A 벡터 = A에서 B까지의 방향과 거리
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 1f);
        
        // 총구화염 코루틴 
        StartCoroutine(ShowMuzzleFlash());

        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);

        currBullet--; // 총알 1발 날리고
        isReload = (currBullet % maxBullet == 0);
        // 위 코드는 아래 조건문과 동일함
        // if (currBullet == 0)
        //     isReload = true;
        // else
        //     isReload = false;
        if (isReload)
        {
            // 재장전 코루틴 함수 호출.
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;

        animator.SetTrigger(hashReload);
        audio.PlayOneShot(reloadSfx, 1f);
        yield return wsReload;

        currBullet = maxBullet;
        isReload = false;
    }

    IEnumerator ShowMuzzleFlash()
    {
        // 비활성화 했던 머즐 플래시 활성화.
        muzzleFlash.enabled = true;

        // 머즐플래시 오브젝트를 0~360도로 회전하기 위하여 사용
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        // 기준벡터 (1,1,1) 인 벡터에 스케일을 곱해서 크기를 변경하는것.
        // vector3 (1,1,1) * 2 = (2,2,2)가 됨.
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1f, 2f);

        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2) * 0.5f);
        // 셰이더 내에서 사용되는 offset 값에 위에서 만든 offset값을 전달하기 위함.
        // _MainTex 라는 명칭은 셰이더 자체에서 만들어진 것으로 사용자가 변경 불가.
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;
    }
}