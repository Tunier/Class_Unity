using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCtrl : MonoBehaviour
{
    public GameObject hpPotion;
    
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
    public int level = 1;
    public float hp;
    public float hpMax;
    public float exp;
    public float moveSpeed; // 이동속도 계수

    float dieAfterTime = 0; // 죽고난후 시간.
    public int attackType = 0; // 왼손인지 오른손인지 공격 타입 결정.

    public bool hitable = true; // 연속으로 히트를 방지하는 변수

    Quaternion targetRot; // 플레이어쪽으로 바라볼 방향

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

        sName = "골렘";
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
                rb.velocity = Vector3.zero;
                CorpseDestroy();
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
        if (hitable)
        {
            hitable = false;
            if (hp > 0)
            {
                hp -= damage;
            }
        }

        if (hp < 0)
            hp = 0;

        Invoke("HitableTranceTure", 0.2f);
    }

    public void HitableTranceTure()
    {
        hitable = true;
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        state = State.DIE;

        for (int i = 0; i < 2; i++)
            weapon[i].enabled = false;
        
        CreateItem();
    }

    void CorpseDestroy()
    {
        dieAfterTime += Time.deltaTime;
        if (dieAfterTime >= 0.6f)
            GetComponent<Collider>().enabled = false;
        else if (dieAfterTime >= 2f)
            tr.Translate(new Vector3(0, -0.7f * Time.deltaTime, 0));

        if (tr.position.y <= -1.72f)
            Destroy(gameObject);
    }

    void CreateItem()
    {
        float itemNum = Random.Range(1f, 100f);

        if (itemNum >= 50f)
        {
            Instantiate(hpPotion, tr.position, Quaternion.identity);
        }
    }
}
