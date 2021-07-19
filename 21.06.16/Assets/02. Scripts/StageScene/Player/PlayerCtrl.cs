using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCtrl : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        HIT,
        DIE
    }

    public int level = 1;
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

    public float attackSpeed;
    public float critcalChance;
    public float resultDamage;

    public bool isCrit;
    public bool hitable;

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

        level = 1;//PlayerPrefs.GetInt("PlayerLevel");
        hp = 50;
        hpMax = 50;
        hpRegen = 0.5f;
        mp = 20;
        mpMax = 20;
        mpRegen = 1f;
        exp = 0;
        expMax = level * 100 + 100;
        str = (level - 1) * 5 + 10;
        dex = (level - 1) * 2.5f + 5;
        def = 0;
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
            if (exp >= expMax)
            {
                LevelUp();
            }
            critcalChance = Mathf.Round(dex + 10f); // 나중에 아이템 효과로 추가되는 값 더해주게 수정
            resultDamage = Mathf.Round(str) + playerWeapon.GetComponent<PlayerWeaponCtrl>().weaponDamage;
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

    void Playing()
    {
        SetPlayerMovement();
        SetPlayerRotate();

        if (hp > hpMax)
            hp = hpMax;
        if (mp > mpMax)
            mp = mpMax;

        hp += hpRegen * Time.deltaTime;
        mp += mpRegen * Time.deltaTime;
    }

    public void Attacking()
    {
        playerWeapon.GetComponent<Collider>().enabled = true;
    }

    public void EndAttacking()
    {
        playerWeapon.GetComponent<Collider>().enabled = false;
    }

    public void EndAttackMotion()
    {
        state = State.IDLE;
        ani.SetBool(hashAttack, false);
        playerWeapon.GetComponent<PlayerWeaponCtrl>().mobList.Clear();
    }

    public void Hit(float damage)
    {
        hitable = false;
        state = State.HIT;

        if (hp > 0)
        {
            hp -= damage;
        }
    }

    void EndHit()
    {
        ani.SetBool(hashHit, false);
        hitable = true;
        state = State.IDLE;
    }

    void LevelUp()
    {
        exp -= expMax;
        expMax = level * 100 + 100;
        level++;
        str = (level - 1) * 5 + 10;
        dex = (level - 1) * 2.5f + 5;
        hpMax += 10;
        hp = hpMax;
        mpMax += 10;
        mp = mpMax;
#if UNITY_EDITOR

#else
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.Save();
#endif
    }

    public void Die()
    {
        state = State.DIE;
    }

    public bool CritCal()
    {
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
