using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int level;
    public float hp;
    public float hpMax;
    public float hpRegen;
    public float mp;
    public float mpMax;
    public float mpRegen;
    public float exp;
    public float expMax;
    public float str;
    public float dex;
    public float def;

    public int statPoint;
}

public class PlayerCtrl : MonoBehaviour
{
    PlayerData playerdata;

    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        HIT,
        DIE
    }

    public string name;
    public int level;
    public float hp;
    public float hpMax;
    public float hpRegen;
    public float mp;
    public float mpMax;
    public float mpRegen;
    public float exp;
    public float expMax;
    public float str;
    public float dex;
    public float def;

    public int statPoint;

    public float attackSpeed = 1f;
    public float critcalChance;
    public float maxDamage;
    public float minDamage;
    public float resultDamage;

    public bool hitable;

    public int gold;
    public int maxGold;

    [SerializeField]
    Toggle infinityMpToggle; // 옵션에서 마나 무한 켜져있는지 받아오는데 사용함.
    [SerializeField]
    Toggle infinityGoldToggle; // 옵션에서 돈 무한 켜져있는지 받아오는데 사용함.

    public MonsterCtrl hitmob = null;

    Ray ray;

    int layerMask;

    public State state = State.IDLE;

    public float moveSpeed; // 이동속도 계수
    public float AttackingMoveLeagth; // 공격시 이동 거리

    Transform tr;
    Rigidbody rb;
    Animator ani;

    public GameObject playerWeapon;

    Vector3 movement = Vector3.zero; // 이동시 백터 받아오는값
    Vector3 AttackMovement = Vector3.zero; // 공격시 움직이는 백터 받아오는값

    Vector3 mousePos; // 마우스 백터 받아옴    

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool 움직이는중
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool 공격중
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool 맞는중
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool 죽음.

    float h; // 세로이동 받아올것.
    float v; // 가로이동 받아올것.

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();

        ray = new Ray();

        layerMask = 1 << LayerMask.NameToLayer("RAYTARGET");

#if UNITY_EDITOR
        level = 1;
#else
        level = PlayerPrefs.GetInt("PlayerLevel");
#endif

        hp = 50;
        hpMax = 50;
        hpRegen = 0.2f;
        mp = 20;
        mpMax = 20;
        mpRegen = 1f;
        exp = 0;
        expMax = level * 100 + 100;
        str = (level - 1) * 5 + 10;
        dex = (level - 1) * 2.5f + 5;
        def = 0;

        gold = 0;
        maxGold = 100000000;

        statPoint = 0;

        attackSpeed = 1f;
        moveSpeed = 4f;
        AttackingMoveLeagth = moveSpeed * 0.02f;

        hitable = true;

        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if ((state != State.HIT) && (state != State.DIE) && !GameManager.instance.isPause)
            {
                if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && state != State.ATTACK)
                    state = State.MOVE;
                else if (h == 0 && v == 0)
                    state = State.IDLE;

                if (!EventSystem.current.IsPointerOverGameObject()) // UI클릭시 동작 안하게함.
                {
                    if (Input.GetButton("Fire1"))
                        state = State.ATTACK;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update()
    {
        if (state != State.DIE)
        {
            if (!GameManager.instance.isPause)
            {
                Playing();
            }
            critcalChance = Mathf.Round(dex + 10f); // 나중에 아이템 효과로 추가되는 값 더해주게 수정
            minDamage = Mathf.Round(str) + playerWeapon.GetComponent<PlayerWeaponCtrl>().weaponDamage;
            maxDamage = Mathf.Round(str) + playerWeapon.GetComponent<PlayerWeaponCtrl>().weaponDamage + 2f;
            resultDamage = Random.Range(minDamage, maxDamage);
        }

        switch (state)
        {
            case State.IDLE:
                ani.SetBool(hashMove, false);
                ani.SetBool(hashHit, false);
                ani.SetBool(hashAttack, false);
                rb.velocity = Vector3.zero;
                break;
            case State.MOVE:
                ani.SetBool(hashMove, true);
                rb.velocity = movement;
                break;
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                //rb.velocity = v3.zero;
                rb.AddForce(AttackMovement, ForceMode.Impulse);
                break;
            case State.HIT:
                ani.SetBool(hashHit, true);
                EndAttacking();
                hitable = false;
                break;
            case State.DIE:
                StopAllCoroutines();
                ani.SetBool(hashDie, true);
                rb.velocity = Vector3.zero;
                hitable = false;
                break;
        }
    }

    /// <summary>
    /// 마우스 커서가 있는 방향으로 플레이어를 회전시킴.<br/>
    /// 마우스에서 레이를 쏴서 레이케스트 타깃(레이어로 구분)에 부딛힌 위치의 x,z축만 받아서 그쪽으로 플레이어의 앞쪽을 돌림.
    /// </summary>
    void SetPlayerRotate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green); // 레이시각화.

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            mousePos = new Vector3(hit.point.x, tr.position.y, hit.point.z) - tr.position;
        }

        tr.forward = mousePos;
    }

    ///<summary>
    ///키보드 입력을 받아서 플레이어를 이동시킴.
    ///</summary>
    void SetPlayerMovement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        movement = new Vector3(h, 0, v) * moveSpeed;

        if (v >= 0.8)
            AttackMovement = new Vector3(h, 0, v) * AttackingMoveLeagth;
        else
            AttackMovement = Vector3.zero;
    }

    ///<summary>
    ///SetPlayerMoveMent 함수, SetPlayerRotate 함수와<br/>
    ///수치 최대 최소값 제한, hp,mp 리젠 등 플레이에 필요한 부분 실행. 
    ///</summary>
    void Playing()
    {
        SetPlayerMovement();
        SetPlayerRotate();

        if (hp > hpMax)
            hp = hpMax;
        else if (hp < hpMax && hp > 0)
            hp += hpRegen * Time.deltaTime;
        else if (hp < 0)
            hp = 0;

        if (mp > mpMax)
            mp = mpMax;
        else if (mp < mpMax && mp > 0)
            mp += mpRegen * Time.deltaTime;
        else if (mp < 0)
            mp = 0;

        if (infinityMpToggle.isOn)
            mp = mpMax;

        if (infinityGoldToggle.isOn)
            gold = maxGold;
    }

    ///<summary>
    ///에니메이션 이벤트에 붙어있다. 검을 앞으로 휘두루기 시작할때 호출.
    ///</summary>
    public void Attacking()
    {
        playerWeapon.GetComponent<Collider>().enabled = true;
    }

    ///<summary>
    ///에니메이션 이벤트에 붙어있다. 검을 휘두른후 회수할때부터 콜라이더를 끄기 위하여 사용.
    ///</summary>
    public void EndAttacking()
    {
        playerWeapon.GetComponent<Collider>().enabled = false;
    }

    ///<summary>
    ///에니메이션 이벤트에 붙어있다. 검 회수가 끝나면 호출.
    ///</summary>
    public void EndAttackMotion()
    {
        state = State.IDLE;
        ani.SetBool(hashAttack, false);
        playerWeapon.GetComponent<PlayerWeaponCtrl>().mobList.Clear();
    }

    ///<summary>
    ///Hit(데미지, 크리티컬인지 아닌지)
    ///</summary>
    public void Hit(float damage)
    {
        hitable = false;
        state = State.HIT;

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    // 에니메이션 이벤트에 붙어있다.
    public void EndHit()
    {
        ani.SetBool(hashHit, false);
        hitable = true;
        state = State.IDLE;
    }


    ///<summary>
    ///경험치를 레벨업에 필요한 경험치만큼 빼고, 레벨업에 필요한 경험치량 증가시킨후 레벨업 시킴.<br/>
    ///그후 여러가지 스텟들 증가.
    ///</summary>
    public void LevelUp()
    {
        exp -= expMax;
        expMax = level * 100 + 100;
        level++;
        str = (level - 1) * 5 + 10;
        dex = (level - 1) * 2.5f + 5;
        hpMax += 10;
        hp = hpMax;
        mpMax += 5;
        mp = mpMax;

        statPoint += 3;
#if UNITY_EDITOR

#else
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.Save();
#endif
    }

    public void Die()
    {
        StopAllCoroutines();

        state = State.DIE;
    }

    ///<summary>
    ///크리티컬이 떴는지 계산해서 Bool값 반환해줌.
    ///</summary>
    public bool CritCal()
    {
        bool isCrit;

        int critcalRandom = Random.Range(0, 100);

        if (critcalRandom >= 100 - critcalChance)
        {
            isCrit = true;
        }
        else
        {
            isCrit = false;
        }

        return isCrit;
    }
}
