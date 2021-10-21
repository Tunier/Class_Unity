using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStatus
{
    Idle,   // 대기
    Wander, // 주변 배회
    Chase,  // 추적
    Attack, // 공격
    Run,    // 도망
}

public class Enemy : MonoBehaviour
{
    public GameObject target;
    Player player;

    public eStatus myStatus = eStatus.Idle;
    public float curHp = 100;

    #region 공격, 도망 거리 관련
    float dist => Vector3.Distance(transform.position, target.transform.position);
    Vector3 dir => (target.transform.position - transform.position).normalized;
    #endregion

    #region 순찰 관련
    public Transform[] wanderSpot;
    int spotIndex;
    Vector3 wanDir => (wanderSpot[spotIndex].position - transform.position).normalized;
    float wanDist => Vector3.Distance(wanderSpot[spotIndex].position, transform.position);
    #endregion

    #region 공격 속도 관련
    float attackDelay = 1f;
    float attackAfterTime = 0;
    bool canAttack => attackAfterTime >= attackDelay;
    #endregion

    void Awake()
    {
        player = target.GetComponent<Player>();
    }

    void Start()
    {
        spotIndex = Random.Range(0, wanderSpot.Length);
    }

    void FixedUpdate()
    {
        Action();
    }

    void Update()
    {
        ChageStatus();

        if (attackAfterTime < attackDelay)
            attackAfterTime += Time.deltaTime;
    }

    void ChageStatus()
    {
        if (dist <= 1)
            myStatus = eStatus.Attack;
        else if (dist <= 5)
        {
            if (curHp <= 30)
            {
                myStatus = eStatus.Run;
                return;
            }

            myStatus = eStatus.Chase;
        }
        else
            myStatus = eStatus.Wander;
    }

    void Action()
    {
        switch (myStatus)
        {
            case eStatus.Idle:
                break;
            case eStatus.Wander:
                if (wanDist >= 0.1f)
                    transform.Translate(wanDir * 2f * Time.fixedDeltaTime);
                else
                    spotIndex = Random.Range(0, wanderSpot.Length);
                break;
            case eStatus.Chase:
                transform.Translate(dir * 2f * Time.fixedDeltaTime);
                break;
            case eStatus.Attack:
                if (canAttack)
                {
                    player.curHp -= 10;
                    attackAfterTime = 0;
                }
                break;
            case eStatus.Run:
                transform.Translate(-dir * 2f * Time.fixedDeltaTime);
                break;
        }
    }
}