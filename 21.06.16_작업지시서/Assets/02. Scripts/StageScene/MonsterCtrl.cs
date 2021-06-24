using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int attackType;

    Transform tr;
    Rigidbody rb;
    Animator ani;

    PlayerCtrl player;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������
    readonly int hashAttackType = Animator.StringToHash("AttackType"); // int ���ݿ��ϸ��̼�����
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool �´���
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool ����.

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if (player.state != PlayerCtrl.State.DIE)
                if (Vector3.Distance(tr.position, GameObject.FindWithTag("PLAYER").GetComponent<Transform>().position) <= 3f)
                {
                    state = State.ATTACK;
                }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        StartCoroutine(CheckState());

        //print(Vector3.Distance(tr.position, GameObject.FindWithTag("PLAYER").GetComponent<Transform>().position));
        switch (state)
        {
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                break;
        }
    }


    void ChangeAttackType()
    {
        if (attackType == 0)
            attackType++;
        else
            attackType--;

        if (attackType == 0)
            ani.SetInteger(hashAttackType, 0);
        else
            ani.GetComponent<Animator>().SetInteger(hashAttackType, 1);
    }

    void EndAttackMotion()
    {
        ani.SetBool(hashAttack, false);
        state = State.IDLE;
    }
}
