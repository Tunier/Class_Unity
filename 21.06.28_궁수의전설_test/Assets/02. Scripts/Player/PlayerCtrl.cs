using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    enum State
    {
        Idle,
        Move,
        Attack,
        Hit,
        Die
    }

    Rigidbody rb;
    Animator ani;

    [SerializeField]
    float moveSpeed;

    Vector3 mousePos;

    State state = State.Idle;

    int layer;

    readonly int hashMove = Animator.StringToHash("Move");
    readonly int hashAttack = Animator.StringToHash("Attack");
    readonly int hashHit = Animator.StringToHash("Hit");

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        moveSpeed = 10f;
        layer = 1 << LayerMask.NameToLayer("RayTarget");
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            if ((state != State.Hit) && (state != State.Die))
            {
                if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && state != State.Attack)
                    state = State.Move;
                else if (Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
                    state = State.Idle;

                if (Input.GetButton("Fire1"))
                    state = State.Attack;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Update()
    {
        StartCoroutine(ChangeState());

        switch (state)
        {
            case State.Idle:
                ani.SetBool(hashMove,false);
                break;
            case State.Move:
                ani.SetBool(hashMove,true);
                break;
            case State.Attack:
                break;
            case State.Hit:
                break;
            case State.Die:
                StopAllCoroutines();
                //ani.SetBool(hashDie, true);
                rb.velocity = Vector3.zero;
                break;
        }
    }
    void FixedUpdate()
    {
        if (state != State.Die)
        {
            SetPlayerMovement();
            SetPlayerRotToMouse();
        }


    }

    void SetPlayerRotToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer))
            mousePos = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;

        transform.forward = mousePos;
    }

    void SetPlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(h, rb.velocity.y, v) * moveSpeed;
    }
}
