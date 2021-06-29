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

    public float moveSpeed; // 이동속도 계수
    public float hp;
    public float hpMax;
    public float exp;

    float dieAfterTime = 0; // 죽고난후 시간.
    public int attackType; // 왼손인지 오른손인지 공격 타입 결정.

    Quaternion targetRot; // 플레이어쪽으로 바라볼 방향;

    Transform tr;
    Rigidbody rb;
    Animator ani;

    PlayerCtrl player;
    public Collider[] weapon;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool 움직이는중
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool 공격중
    readonly int hashAttackType = Animator.StringToHash("AttackType"); // int 공격에니메이션종류
    readonly int hashChase = Animator.StringToHash("IsChase"); // bool 공격중
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool 맞는중
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool 죽음.

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();

        moveSpeed = 2f;
        hp = 11;
        hpMax = 100;
        exp = 500;

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
                    {
                        state = State.ATTACK;
                    }
                    else if (!(ani.GetBool(hashAttack)) && (Vector3.Distance(tr.position, player.transform.position) <= 7f))
                    {
                        state = State.CHASE;
                    }
                    else
                    {
                        state = State.IDLE;
                    }
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


    void ChangeAttackType()
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


    public void Attacking()
    {
        weapon[attackType].enabled = true;
    }

    public void EndAttacking()
    {
        weapon[attackType].enabled = false;

        ani.SetBool(hashAttack, false);
    }

    public void Hit(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
        }

        if (hp < 0)
        {
            hp = 0;
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        state = State.DIE;
        dieAfterTime += Time.deltaTime;

        if (dieAfterTime >= 3f)
        {
            GetComponent<Collider>().enabled = false;
            tr.Translate(new Vector3(0, -0.3f * Time.deltaTime, 0));
        }

        if (tr.position.y <= -1.72f)
        {
            Destroy(gameObject);
        }
    }
}
