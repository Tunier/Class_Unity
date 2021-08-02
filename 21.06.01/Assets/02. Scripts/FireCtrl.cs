using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// struct = ����ü, Ŭ���� ��ȭ������ �����ϸ� ����. stack ������ ������.
// Ŭ������ ����ü ���� ������ ����.
// �׷��� ���ݿ� �ͼ��� �޸� ���� ������ ���̸� ������ ��ɻ� ū ���̴� ����.
[System.Serializable] // ����ȭ �ν����Ϳ� ǥ��.
public struct PlayerSfx
{
    // �迭�� ���� ���� = ���Ⱑ ����Ǹ� ���嵵 �����ϱ� ���ؼ� ���.
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType // ������.
    {
        RIFLE = 0,
        SHOTGUN
    }
    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bullet; // �Ѿ� ������ ����ϱ� ���� ����.
    public Transform firePos; // �Ѿ� �߻� ��ġ.
    public ParticleSystem cartidge; // ź�� ������.
    private ParticleSystem muzzleFlash; // �ѱ� ȭ�� ��ƼŬ.

    AudioSource _audio;
    public PlayerSfx playerSfx; // ����� Ŭ�� ���� ����.

    Shake shake;

    public Image magazineImg;
    public Text magazineText;

    public int maxBullet = 10;
    public int remainingBullet = 10;

    public float reloadTime = 2f;
    bool isReloading = false;

    public Sprite[] weaponIcons;
    public Image weaponImage;

    int enemyLayer;
    int obstacleLayer;
    int layerMask;

    bool isFire = false;
    float nextFire;
    public float fireRate = 0.1f;

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

        // ���̾��� �̸��� ���Ͽ� ���̾ �̸� ����.
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        // 100 | 001 = 101
        // or ������ �Ѵ� 0�� �ƴ� ��쿡 1�� ó����.
        layerMask = 1 << enemyLayer | 1 << obstacleLayer;
    }

    void Update()
    {
        Debug.DrawLine(firePos.position, firePos.forward * 20f, Color.green);

        RaycastHit hit;

        //isFire = Physics.Raycast(firePos.position, firePos.forward, out hit, 20f, 1 << layerMask) ? true : false;

        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20f, layerMask))
            isFire = (hit.collider.CompareTag("ENEMY"));
        else
            isFire = false;

        if (!isReloading && isFire)
        {
            if (Time.time > nextFire)
            {
                remainingBullet--;
                Fire();

                if (remainingBullet == 0)
                {
                    StartCoroutine(Reloading());
                }
                nextFire = Time.time + fireRate;
            }
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        // 0�̸� ��Ŭ�� 1�̸� ��Ŭ��
        // GetMouseButtonDown �Լ��� �������� �ѹ��� ����.
        if (!isReloading && Input.GetMouseButtonDown(0))
        {
            remainingBullet--;
            // �����Լ� ȣ��
            Fire();

            if (remainingBullet <= 0)
            {
                //������ �ڷ�ƾ �Լ�
            }
        }
    }

    void Fire()
    {
        // shake ��ũ��Ʈ ������ shakeCamera �ڷ�ƾ �Լ� ȣ��
        // �Ű����� ���� ���������Ƿ� ShakeCamera�� �⺻���� ����.
        StartCoroutine(shake.ShakeCamera());

        // Instantiate(���������� ������Ʈ, ��ġ, ����);
        // ������ �ʴ� ��ü(Object)�� Ȱ��ȭ ���ִ� �Լ�.
        //Instantiate(bullet, firePos.position, firePos.rotation);

        var _bullet = GameManager.instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        cartidge.Play(); // ź�� ��ƼŬ ���
        muzzleFlash.Play(); // �ѱ� ȭ�� ��ƼŬ ���

        FireSfx(); // ���ݽ� ���� �߻�.

        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        UpdateBulletText();

        if (remainingBullet == 0)
        {
            StartCoroutine(Reloading());
        }
    }

    void FireSfx()
    {
        // ���� ���õ� ������ �ѹ��� �´� ���带 �����ؼ� �������.
        var _sfx = playerSfx.fire[(int)currWeapon];
        // Play = �Ҹ� ���� �Ұ�, PlayOneShot �Ҹ� ���� ����.
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
