using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL, // 순찰
        TRACE, // 추적
        ATTCK, // 공격
        DIE // 사망
    }

    public State state = State.PATROL; // 초기 상태 지정.

    Transform playerTr; // 플레이어 위치 저장 변수.
    Transform enemyTr; // 적캐릭터 위치 저장 변수.

    public float attackDist = 5f; // 공격 사거리.
    public float traceDist = 10f; // 추적 사거리.
    public bool isDie = false; // 사망 여부 판단 변수.

    WaitForSeconds ws; // 시간 지연 변수.

    MoveAgent moveAgent;
    EnemyFire enemyFire;

    Animator animator;
    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashSpeed = Animator.StringToHash("Speed");
    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    readonly int hashOffset = Animator.StringToHash("Offset");
    readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    EnemyFOV enemyFOV;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");

        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();
        enemyFire = GetComponent<EnemyFire>();

        // 시간 지연 변수를 0.3 값으로 설정.
        // 시간 지연 변수는 코루틴 함수에서 사용됨.
        ws = new WaitForSeconds(0.3f);

        // Offset과 Speed값을 이용해서 애니메이션 동작을 다향하게 구성
        // 속도도 조금씩 다르게 만들어줌.
        animator.SetFloat(hashOffset, Random.Range(0f, 1f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 1.2f));

        enemyFOV = FindObjectOfType<EnemyFOV>();
    }

    private void OnEnable()
    {
        // OnEnable은 해당 스크립트가 활성화될 때마다 실행됨.
        // 상태 체크하는 코루틴 함수 호출.
        StartCoroutine(CheckState());
        // 상태 변화에 따라 행동을 지시하는 코루틴 함수 호출.
        StartCoroutine(Action());

        // Damage 스크립트의 OnPlayerDieEvent 이벤트에
        // EnemyAi 스크립트의 OnPlayerDie 함수를 연결시켜줌.
        Damage.OnPlayerDieEvent += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        // 스크립트가 비활성화 될 때에는 이벤트와 연결된 함수 연결 해제를 원칙으로함.
        Damage.OnPlayerDieEvent -= this.OnPlayerDie;
    }

    // 코루틴 = 독자적인 시간축을 가짐.
    IEnumerator CheckState() // 상태체크 코루틴 함수.
    {
        yield return new WaitForSeconds(1f);

        while (!isDie) // 적이 살아있는동안 계속 실행되도록 while사용.
        {
            if (state == State.DIE)
                yield break; // 코루틴 함수 정지.
            // Distance(A,B) - A와 B사이의 거리를 계산해주는 함수.
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist) // 공격 사거리 이내면 공격으로 변경.
            {
                if (enemyFOV.isViewPlayer())
                    state = State.ATTCK; // 장애물 없으면 공격
                else
                    state = State.TRACE; // 장애물 있으면 추적
            }
            else if (enemyFOV.isTracePlayer()) // 추적 사거리 이내면 추적으로 변경.
            {
                state = State.TRACE;
            }
            else // 공격도 추적도 아니면 순찰상태로 변경.
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTCK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if (enemyFire.isFire == false)
                        enemyFire.isFire = true;
                    break;
                case State.DIE:
                    gameObject.tag = "Untagged";

                    isDie = true;
                    enemyFire.isFire = false;

                    moveAgent.Stop();
                    // 랜덤값에 의해서 죽는 모션 변경
                    animator.SetInteger(hashDieIdx, Random.Range(0, 3));
                    animator.SetTrigger(hashDie);
                    // 사망후 남아있는 콜라이더 비활성화 해서 계속 충돌하지 않도록 함.
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        // 애니메이터 변수의 Set 함수들의 종류는 여러가지가 있음.
        // SetFloat 해당함수는 (해쉬값 / 파라메터 이름, 전달하고자 하는 값) 형태로 사용됨.
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        // 모든 코루틴 함수 종료
        // 유한상태 머신 정지 해야됨.
        StopAllCoroutines();

        animator.SetTrigger(hashPlayerDie);
    }
}
