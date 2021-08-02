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
    Toggle infinityMpToggle; // �ɼǿ��� ���� ���� �����ִ��� �޾ƿ��µ� �����.
    [SerializeField]
    Toggle infinityGoldToggle; // �ɼǿ��� �� ���� �����ִ��� �޾ƿ��µ� �����.

    public MonsterCtrl hitmob = null;

    Ray ray;

    int layerMask;

    public State state = State.IDLE;

    public float moveSpeed; // �̵��ӵ� ���
    public float AttackingMoveLeagth; // ���ݽ� �̵� �Ÿ�

    Transform tr;
    Rigidbody rb;
    Animator ani;

    public GameObject playerWeapon;

    Vector3 movement = Vector3.zero; // �̵��� ���� �޾ƿ��°�
    Vector3 AttackMovement = Vector3.zero; // ���ݽ� �����̴� ���� �޾ƿ��°�

    Vector3 mousePos; // ���콺 ���� �޾ƿ�    

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool �´���
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool ����.

    float h; // �����̵� �޾ƿð�.
    float v; // �����̵� �޾ƿð�.

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

                if (!EventSystem.current.IsPointerOverGameObject()) // UIŬ���� ���� ���ϰ���.
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
            critcalChance = Mathf.Round(dex + 10f); // ���߿� ������ ȿ���� �߰��Ǵ� �� �����ְ� ����
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
    /// ���콺 Ŀ���� �ִ� �������� �÷��̾ ȸ����Ŵ.<br/>
    /// ���콺���� ���̸� ���� �����ɽ�Ʈ Ÿ��(���̾�� ����)�� �ε��� ��ġ�� x,z�ุ �޾Ƽ� �������� �÷��̾��� ������ ����.
    /// </summary>
    void SetPlayerRotate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green); // ���̽ð�ȭ.

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            mousePos = new Vector3(hit.point.x, tr.position.y, hit.point.z) - tr.position;
        }

        tr.forward = mousePos;
    }

    ///<summary>
    ///Ű���� �Է��� �޾Ƽ� �÷��̾ �̵���Ŵ.
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
    ///SetPlayerMoveMent �Լ�, SetPlayerRotate �Լ���<br/>
    ///��ġ �ִ� �ּҰ� ����, hp,mp ���� �� �÷��̿� �ʿ��� �κ� ����. 
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
    ///���ϸ��̼� �̺�Ʈ�� �پ��ִ�. ���� ������ �ֵη�� �����Ҷ� ȣ��.
    ///</summary>
    public void Attacking()
    {
        playerWeapon.GetComponent<Collider>().enabled = true;
    }

    ///<summary>
    ///���ϸ��̼� �̺�Ʈ�� �پ��ִ�. ���� �ֵθ��� ȸ���Ҷ����� �ݶ��̴��� ���� ���Ͽ� ���.
    ///</summary>
    public void EndAttacking()
    {
        playerWeapon.GetComponent<Collider>().enabled = false;
    }

    ///<summary>
    ///���ϸ��̼� �̺�Ʈ�� �پ��ִ�. �� ȸ���� ������ ȣ��.
    ///</summary>
    public void EndAttackMotion()
    {
        state = State.IDLE;
        ani.SetBool(hashAttack, false);
        playerWeapon.GetComponent<PlayerWeaponCtrl>().mobList.Clear();
    }

    ///<summary>
    ///Hit(������, ũ��Ƽ������ �ƴ���)
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

    // ���ϸ��̼� �̺�Ʈ�� �پ��ִ�.
    public void EndHit()
    {
        ani.SetBool(hashHit, false);
        hitable = true;
        state = State.IDLE;
    }


    ///<summary>
    ///����ġ�� �������� �ʿ��� ����ġ��ŭ ����, �������� �ʿ��� ����ġ�� ������Ų�� ������ ��Ŵ.<br/>
    ///���� �������� ���ݵ� ����.
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
    ///ũ��Ƽ���� ������ ����ؼ� Bool�� ��ȯ����.
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
