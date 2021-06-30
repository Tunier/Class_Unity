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
        CHASE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public string sName;
    public int level;
    public float hp;
    public float hpMax;
    public float exp;
    public float moveSpeed; // �̵��ӵ� ���

    float dieAfterTime = 0; // �װ��� �ð�.
    public int attackType; // �޼����� ���������� ���� Ÿ�� ����.

    Quaternion targetRot; // �÷��̾������� �ٶ� ����;

    Transform tr;
    Rigidbody rb;
    Animator ani;

    PlayerCtrl player;
    public Collider[] weapon;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������
    readonly int hashAttackType = Animator.StringToHash("AttackType"); // int ���ݿ��ϸ��̼�����
    readonly int hashChase = Animator.StringToHash("IsChase"); // bool ������
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool �´���
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool ����.

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        sName = "��";
        level = 1;
        hp = 30f;
        hpMax = 100f;
        exp = 150f;
        moveSpeed = 2f;

        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if (state != State.DIE)
            {
                if (player.state != PlayerCtrl.State.DIE)
                {
                    if (Vector3.Distance(tr.position, player.transform.position) <= 2.7f)
                        state = State.ATTACK;
                    else if (!(ani.GetBool(hashAttack)) && (Vector3.Distance(tr.position, player.transform.position) <= 7f))
                        state = State.CHASE;
                    else
                        state = State.IDLE;
                }
                else
                {
                    state = State.IDLE;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Update()
    {
        StartCoroutine(CheckState());

        switch (state)
        {
            case State.IDLE:
                //ani.SetBool(hashAttack, false);
                ani.SetBool(hashChase, false);
                rb.velocity = Vector3.zero;
                break;
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                ani.SetBool(hashChase, false);
                rb.velocity = Vector3.zero;
                break;
            case State.CHASE:
                ani.SetBool(hashChase, true);
                targetRot = Quaternion.LookRotation(player.transform.position - tr.position);
                tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, 120 * Time.deltaTime);
                rb.velocity = tr.forward * moveSpeed;
                break;
            case State.DIE:
                ani.SetBool(hashDie, true);
                Die();
                rb.velocity = Vector3.zero;
                break;
        }
    }


    public virtual void ChangeAttackType()
    {
        if (attackType == 0)
            attackType++;
        else
            attackType--;

        if (attackType == 0)
            ani.SetInteger(hashAttackType, 0);
        else
            ani.SetInteger(hashAttackType, 1);
    }


    public virtual void Attacking()
    {
        weapon[attackType].enabled = true;
    }

    public virtual void EndAttacking()
    {
        weapon[attackType].enabled = false;

        ani.SetBool(hashAttack, false);
    }

    public virtual void Hit(float damage)
    {
        if (hp > 0)
            hp -= damage;

        if (hp < 0)
            hp = 0;
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        state = State.DIE;
        dieAfterTime += Time.deltaTime;

        for (int i = 0; i < 2; i++)
            weapon[i].enabled = false;

        if (dieAfterTime >= 1.2f)
            GetComponent<Collider>().enabled = false;
        else if (dieAfterTime >= 3f)
            tr.Translate(new Vector3(0, -0.3f * Time.deltaTime, 0));

        if (tr.position.y <= -1.72f)
            Destroy(gameObject);
    }
}
