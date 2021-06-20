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

    public float moveSpeed = 3f; // �̵��ӵ� ���
    public float rotSpeed = 1f; // ȸ���ӵ� ���
    public float AttackingMoveLeagth = 0.5f; // ���ݽ� �̵� �Ÿ�

    Rigidbody rb;
    Transform tr;
    Animator ani;

    Vector3 movement; // �̵��� ���� �޾ƿ��°�
    Vector3 AttackMovement; // ���ݽ� �����̴� ���� �޾ƿ��°�

    Vector3 mousePos; // ���콺 ���� �޾ƿ�    

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������

    float h; // �����̵� �޾ƿð�.
    float v; // �����̵� �޾ƿð�.
    float r; // ���콺 ȸ�� �޾ƿð�.

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

        mousePos = Input.mousePosition; // ȭ���� ���콺 ������ �޾ƿ�.

        ray = playerCam.ScreenPointToRay(mousePos); // ī�޶󿡼� ���� ���콺 ��ġ ������ �޾ƿ�.

        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green); // ���̽ð�ȭ.

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1 << 8)) // �����ɽ�Ʈ�� �浹�ϴ� ��ü�� �ִ��� �Ǻ�.
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
