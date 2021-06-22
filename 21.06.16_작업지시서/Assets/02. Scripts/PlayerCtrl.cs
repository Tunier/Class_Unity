using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float hp = 100;

    Ray ray;

    int layerMask;

    public State state = State.IDLE;

    public float moveSpeed = 3f; // �̵��ӵ� ���
    public float AttackingMoveLeagth = 1f; // ���ݽ� �̵� �Ÿ�

    public Transform tr;
    Rigidbody rb;
    Animator ani;

    Vector3 movement = Vector3.zero; // �̵��� ���� �޾ƿ��°�
    Vector3 AttackMovement = Vector3.zero; // ���ݽ� �����̴� ���� �޾ƿ��°�

    Vector3 mousePos; // ���콺 ���� �޾ƿ�    

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool �´���
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool ����.

    float h; // �����̵� �޾ƿð�.
    float v; // �����̵� �޾ƿð�.
    float r; // ���콺 ȸ�� �޾ƿð�.

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        ray = new Ray();
        layerMask = 1 << LayerMask.NameToLayer("RAYTARGET");
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && state != State.ATTACK)
                SetState(State.MOVE);
            else if (Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
                SetState(State.IDLE);

            if (Input.GetButton("Fire1"))
                SetState(State.ATTACK);

            yield return new WaitForSeconds(0.02f);
        }
    }

    void Update()
    {
        StartCoroutine(CheckState());
        SetPlayerMovement();
        SetPlayerRotate();

        switch (state)
        {
            case State.IDLE:
                ani.SetBool(hashMove, false);
                ani.SetBool(hashHit, false);
                rb.velocity = Vector3.zero;
                break;
            case State.MOVE:
                ani.SetBool(hashMove, true);
                rb.velocity = movement;
                break;
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                rb.velocity = AttackMovement;
                break;
            case State.HIT:
                ani.SetBool(hashHit, true);
                break;
            case State.DIE:
                StopAllCoroutines();
                gameObject.SetActive(false);
                rb.velocity = Vector3.zero;
                break;
        }
    }

    public void SetState(State s)
    {
        state = s;
    }
    void EndAttackMotion()
    {
        ani.SetBool(hashAttack, false);
        state = State.IDLE;
    }

    void EndHitMotion()
    {
        ani.SetBool(hashHit, false);
        state = State.IDLE;

        MonsterWeaponCtrl mobWeapon = GetComponent<MonsterWeaponCtrl>();

        mobWeapon.GetComponent<Collider>().enabled = false;
    }

    void SetPlayerRotate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 500f, Color.green); // ���̽ð�ȭ.

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
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
        AttackMovement = new Vector3(h, 0, v) * AttackingMoveLeagth;
    }
}
