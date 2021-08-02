using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public GameObject[] item;

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
    public float moveSpeed; // 이동속도 계수

    float dieAfterTime = 0; // 죽고난후 시간.
    public int attackType = 0; // 왼손인지 오른손인지 공격 타입 결정.

    Quaternion targetRot; // 플레이어쪽으로 바라볼 방향

    Transform tr;
    Rigidbody rb;
    Animator ani;
    NavMeshAgent nav;
    public Collider[] weapon;

    PlayerCtrl player;
    MonsterSpawnerCtrl mobSpawner;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool 움직이는중
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool 공격중
    readonly int hashAttackType = Animator.StringToHash("AttackType"); // int 공격에니메이션종류
    readonly int hashChase = Animator.StringToHash("IsChase"); // bool 공격중
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool 맞는중
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool 죽음.

    Vector3 destination;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
        mobSpawner = GameObject.Find("MonsterSpawner").GetComponent<MonsterSpawnerCtrl>();

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        sName = "골렘";
        exp = 100f;
        moveSpeed = 1.5f;
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if (state != State.DIE)
            {
                if (player.state != PlayerCtrl.State.DIE)
                {
                    if (!(ani.GetBool(hashAttack)) && (Vector3.Distance(tr.position, player.transform.position) <= 7f))
                        state = State.CHASE;
                    else if (Vector3.Distance(tr.position, player.transform.position) <= 2.7f)
                        state = State.ATTACK;
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

        destination = player.transform.position;

        switch (state)
        {
            case State.IDLE:
                ani.SetBool(hashChase, false);
                nav.ResetPath();
                break;
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                ani.SetBool(hashChase, false);
                targetRot = Quaternion.LookRotation(player.transform.position - tr.position);
                tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, 60 * Time.deltaTime);
                nav.ResetPath();
                break;
            case State.CHASE:
                ani.SetBool(hashChase, true);
                nav.SetDestination(destination);
                nav.speed = moveSpeed;
                break;
            case State.DIE:
                CorpseDestroy();
                ani.SetBool(hashDie, true);
                for (int i = 0; i < 2; i++)
                    weapon[i].enabled = false;
                nav.enabled = false;
                break;
        }
    }

    // 에니메이션 이벤트에 붙어있다.
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

    // 에니메이션 이벤트에 붙어있다.
    public virtual void Attacking()
    {
        weapon[attackType].enabled = true;
    }

    // 에니메이션 이벤트에 붙어있다.
    public virtual void EndAttacking()
    {
        weapon[attackType].enabled = false;

        ani.SetBool(hashAttack, false);
    }

    ///<summary>
    ///Hit(데미지, 크리티컬인지 아닌지)
    ///</summary>
    public virtual void Hit(float damage, bool crit)
    {
        hp -= damage;

        UIManager.instance.PrintDamageText(damage, crit);

        if (hp <= 0)
        {
            hp = 0;
            Die();
            player.exp += exp;
        }
    }

    public IEnumerator MultyHit(float damage, int attackTimes, float delay, bool isCrit)
    {
        for (int i = 0; i < attackTimes; i++)
        {
            Hit(damage, isCrit);
            yield return new WaitForSeconds(delay);
        }
    }

    // 타격 대상에서 코루틴을 시작하면 타격 대상이 없어지면(ex : 스킬이 시간이 지나서 없어짐) 코루틴이 끝나기때문에 몬스터에서 코루틴 시작
    ///<summary>
    ///StartMultyHit(데미지, 타격횟수, 타격간의 딜레이, 크리티컬인지 아닌지)
    ///</summary>
    public void StartMultyHit(float damage, int attackTimes, float delay, bool isCrit)
    {
        StartCoroutine(MultyHit(damage, attackTimes, delay, isCrit));
    }

    ///<summary>
    ///모든코루틴정지, 상태 사망으로 변경, 게임메니져에 몹 리스트에서 삭제(리젠 제한 -1), 아이템 드롭 실행
    ///</summary>
    public virtual void Die()
    {
        StopAllCoroutines();

        state = State.DIE;

        for (int i = 0; i < 2; i++)
            weapon[i].enabled = false;

        mobSpawner.mobList.Remove(gameObject);

        DropItem();

        if (player.exp >= player.expMax)
        {
            player.LevelUp();
        }
    }


    ///<summary>
    ///상태가 사망이 된다음 0.5초가 지나면 콜라이더 비활성화, 2초가 지나면 시체가 가라앉기 시작, 일정이상 가라앉으면 Destroy
    ///</summary>
    void CorpseDestroy()
    {
        dieAfterTime += Time.deltaTime;
        if (dieAfterTime >= 0.5f)
            GetComponent<Collider>().enabled = false;

        if (dieAfterTime >= 2f)
            tr.Translate(Vector3.down * 0.2f * Time.deltaTime);

        if (tr.position.y <= -1.62f)
            Destroy(gameObject);
    }

    ///<summary>
    ///드롭 계산해서(현재는 테스트를 위하여 70%확률로 책정되어있음) 랜덤한 아이템 드롭.
    ///</summary>
    void DropItem()
    {
        float itemDropChance = Random.Range(1f, 100f); // 플롯값은 1과 100 모두 포함.
        int itemType = Random.Range(0, item.Length); // 인트값 계산이라 1더해줌

        if (itemDropChance >= 30f)
        {
            var obj = Instantiate(item[itemType], tr.position, Quaternion.identity);

            obj.transform.Translate(new Vector3(0, 0.85f, 0));
        }
    }
}
