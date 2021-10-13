using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAi : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    MonsterAnim monsterAnim;
    MonsterAction monsterAction;
    Creature creature;

    NavMeshAgent agent;
    Transform playerTr;

    float dist; //플레이어와 적의 거리

    public Coroutine checkState;


    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
        monsterAction = GetComponent<MonsterAction>();
        creature = GetComponent<Creature>();
        agent = GetComponent<NavMeshAgent>();
        creature.state = STATE.Patrol;

        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
    }
    private void Update()
    {
        dist = Vector3.Distance(playerTr.position, transform.position);
    }

    private void OnEnable()
    {
        StartCoroutine(Action());
        checkState = StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(0.1f);

        while (monsterAction.state != STATE.Die || monsterAction.curHp >= 0) //죽지않고 Anger가 true일때 상태변화
        {
            if (monsterAction.isAnger)
            {
                if (dist <= monsterAction.maxDist)
                {
                    if (dist < monsterAction.minDist)
                    {
                        creature.state = STATE.Backing;
                    }
                }
                else if (dist <= monsterAction.attackDist && dist > monsterAction.maxDist)
                {
                    creature.state = STATE.Attacking;
                }
                else if (dist <= monsterAction.traceDist && dist > monsterAction.attackDist)
                {
                    creature.state = STATE.Chase;
                }
                else
                {
                    creature.state = STATE.Patrol;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 스테이터스에 따른 애니메이션,행동을 제어하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            switch (creature.state)
            {
                case STATE.Patrol:
                    monsterAction.MovePoint();
                    break;
                case STATE.Chase:
                    monsterAction.Chase(playerTr.position);
                    break;
                case STATE.Attacking:
                    agent.velocity = Vector3.zero;
                    monsterAction.Attack(playerTr.position);
                    break;
                case STATE.Backing:
                    monsterAction.BackMove(playerTr.position);
                    break;
                case STATE.Die:
                    monsterAction.Die();
                    monsterAction.DropItem();
                    yield break;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
