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

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");

        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();

        // 시간 지연 변수를 0.3 값으로 설정.
        // 시간 지연 변수는 코루틴 함수에서 사용됨.
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        // OnEnable은 해당 스크립트가 활성화될 때마다 실행됨.
        // 상태 체크하는 코루틴 함수 호출.
        StartCoroutine(CheckState());
    }

    // 코루틴 = 독자적인 시간축을 가짐.
    IEnumerator CheckState() // 상태체크 코루틴 함수.
    {
        while (!isDie) // 적이 살아있는동안 계속 실행되도록 while사용.
        {
            if (state == State.DIE)
                yield break; // 코루틴 함수 정지.
            // Distance(A,B) - A와 B사이의 거리를 계산해주는 함수.
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist) // 공격 사거리 이내면 공격으로 변경.
            {
                state = State.ATTCK;
            }
            else if (dist <= traceDist) // 추적 사거리 이내면 추적으로 변경.
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

    // Update is called once per frame
    void Update()
    {

    }
}
