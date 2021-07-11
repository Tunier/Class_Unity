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

    public int level;
    public float hp;
    public float hpMax;
    public float mp;
    public float mpMax;
    public float exp;
    public float expMax;
    public float str;
    public float dex;
    public float def;

    public bool hitable;

    Ray ray;

    int layerMask;

    public State state = State.IDLE;

    public float moveSpeed; // �̵��ӵ� ���
    public float AttackingMoveLeagth; // ���ݽ� �̵� �Ÿ�

    Transform tr;
    Rigidbody rb;
    Animator ani;

    GameManager gameManager;
    public GameObject playerWeapon;
    Status status;

    Vector3 movement = Vector3.zero; // �̵��� ���� �޾ƿ��°�
    Vector3 AttackMovement = Vector3.zero; // ���ݽ� �����̴� ���� �޾ƿ��°�
    Vector3 hitMovement = Vector3.zero; // �ǰݽ� �̵��ϴ� ���Ͱ�

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

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        status = FindObjectOfType<Status>();

        ray = new Ray();

        layerMask = 1 << LayerMask.NameToLayer("RAYTARGET");

        level = PlayerPrefs.GetInt("PlayerLevel");
        hp = 60;
        hpMax = 100;
        mp = 20;
        mpMax = 20;
        exp = 0;
        expMax = level * 100 + 100;
        str = (level - 1) * 5 + 10;
        dex = (level - 1) * 2.5f + 5;
        def = 0;

        moveSpeed = 4f;
        AttackingMoveLeagth = moveSpeed * 0.02f;

        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if ((state != State.HIT) && (state != State.DIE) && !gameManager.isPause)
            {
                hitable = true;

                if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && state != State.ATTACK)
                    SetState(State.MOVE);
                else if (h == 0 && v == 0)
                    SetState(State.IDLE);
                if (!EventSystem.current.IsPointerOverGameObject()) // UIŬ���� ���� ���ϰ���.
                {
                    if (Input.GetButton("Fire1"))
                        SetState(State.ATTACK);
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update()
    {
        if (state != State.DIE)
        {
            if (!gameManager.isPause)
            {
                Playing();
            }
            
            if (exp >= expMax)
            {
                LevelUp();
            }
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

    public void SetState(State s)
    {
        state = s;
    }

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
    }

    public void Hit(float damage)
    {
        hitable = false;
        SetState(State.HIT);

        if (hp > 0)
        {
            hp -= damage;
        }
    }

    void EndHit()
    {
        ani.SetBool(hashHit, false);
        hitable = true;
        SetState(State.IDLE);
    }

    void LevelUp()
    {
        exp -= expMax;
        expMax = level * 100 + 100;
        level++;
        str = (level - 1) * 5 + 10;
        dex = (level - 1) * 2.5f + 5;
#if UNITY_EDITOR

#else
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.Save();
#endif
    }

    public void Die()
    {
        SetState(State.DIE);
    }
}
