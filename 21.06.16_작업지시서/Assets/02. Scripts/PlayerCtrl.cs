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
        DIE
    }

    private Camera playerCam;

    Ray ray;

    int layerMask;

    public State state = State.IDLE;

    public float moveSpeed = 3f; // 이동속도 계수
    public float rotSpeed = 1f; // 회전속도 계수
    public float AttackingMoveLeagth = 0.5f; // 공격시 이동 거리

    Rigidbody rb;
    Transform tr;
    Animator ani;

    Vector3 movement; // 이동시 백터 받아오는값
    Vector3 AttackMovement; // 공격시 움직이는 백터 받아오는값

    Vector3 mousePos; // 마우스 백터 받아옴    

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool 움직이는중
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool 공격중

    float h; // 세로이동 받아올것.
    float v; // 가로이동 받아올것.
    float r; // 마우스 회전 받아올것.

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        ray = new Ray();
        layerMask = 1 << LayerMask.NameToLayer("LAYTARGET");
        playerCam = Camera.main;
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

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        movement = new Vector3(h, 0, v) * moveSpeed;
        AttackMovement = new Vector3(h, 0, v) * AttackingMoveLeagth;

        switch (state)
        {
            case State.IDLE:
                ani.SetBool(hashMove, false);
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
        }

        mousePos = Input.mousePosition; // 화면의 마우스 포지션 받아옴.

        ray = playerCam.ScreenPointToRay(mousePos); // 카메라에서 보는 마우스 위치 포지션 받아옴.

        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green); // 레이시각화.

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1 << 8)) // 레이케스트에 충돌하는 물체가 있는지 판별.
        {
            mousePos = new Vector3(raycastHit.point.x,tr.position.y,raycastHit.point.z) - tr.position;
        }

        tr.forward = mousePos;
        //tr.LookAt(mousePos);
    }

    void EndAttackEvent()
    {
        state = State.IDLE;
        ani.SetBool(hashAttack, false);
    }

    void SetState(State s)
    {
        state = s;
    }
}
