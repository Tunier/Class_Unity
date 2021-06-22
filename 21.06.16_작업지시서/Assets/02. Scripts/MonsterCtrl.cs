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

    public float moveSpeed = 3f; // �̵��ӵ� ���

    Transform tr;
    Rigidbody rb;
    Animator ani;

    PlayerCtrl player;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool �´���
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool ����.

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
