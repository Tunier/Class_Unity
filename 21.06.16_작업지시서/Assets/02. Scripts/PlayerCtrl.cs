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

    public float moveSpeed = 3f; // 이동속도 계수
    public float AttackingMoveLeagth = 1f; // 공격시 이동 거리

    public Transform tr;
    Rigidbody rb;
    Animator ani;

    Vector3 movement = Vector3.zero; // 이동시 백터 받아오는값
    Vector3 AttackMovement = Vector3.zero; // 공격시 움직이는 백터 받아오는값

    Vector3 mousePos; // 마우스 백터 받아옴    

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool 움직이는중
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool 공격중
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool 맞는중
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool 죽음.

    float h; // 세로이동 받아올것.
    float v; // 가로이동 받아올것.
    float r; // 마우스 회전 받아올것.

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
        Debug.DrawRay(ray.origin, ray.direction * 500f, Color.green); // 레이시각화.

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
