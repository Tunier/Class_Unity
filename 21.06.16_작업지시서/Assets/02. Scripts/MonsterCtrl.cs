using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public float moveSpeed = 3f; // 이동속도 계수

    Transform tr;
    Rigidbody rb;
    Animator ani;

    PlayerCtrl player;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool 움직이는중
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool 공격중
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool 맞는중
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool 죽음.

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();        
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(0.1f);
    }

    void Update()
    {
        switch (state)
        {
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                break;
        }
    }
}
