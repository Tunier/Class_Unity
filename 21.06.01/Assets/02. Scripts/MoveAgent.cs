using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgent : MonoBehaviour
{
    // List도 배열이다.
    // 차이점 - 가변길이로서 내용물에 따라 길이가 변함.
    public List<Transform> wayPoints;
    public int nextIdx; // 다음 순찰 지점의 배열 인덱스.

    NavMeshAgent agent;

    float damping = 1f; // 회전 속도 조절하는 계수.
    Transform enemyTr;

    readonly float patrolSpeed = 1.5f; // 읽기전용 순찰 속도 변수.
    readonly float traceSpeed = 4f; // 추적 속도 변수.

    bool _patrolling; // 순찰 여부 판단 변수.

    // 프로퍼티 작성
    // 프로퍼티 : 함수인데 변수처럼 쓰임.
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            // set 동작시 전달받은 값은 value에 들어감.
            // value에 있는값을 _patrolling에 전달함.
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1f;
                MoveWayPoint();
            }
        }
    }

    Vector3 _traceTaget;
    public Vector3 traceTarget
    {
        get { return _traceTaget; }
        set
        {
            _traceTaget = value;
            agent.speed = traceSpeed;
            damping = 7f;
            // 추적 대상 지정 함수 호출.
            TraceTarget(_traceTaget);
        }
    }

    public float speed // agent 이동속도를 가져오는 프로퍼티 정의
    {
        // get만 존재하므로 따로 설정은 하지 못하고, 값만 가져올 수 있음.
        get { return agent.velocity.magnitude; }
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // 브레이크를 꺼서 자동 감속하지 않도록 해줌. 목적지에 가까워질수록 속도를 줄이는 옵션.
        agent.autoBraking = false;
        agent.speed = patrolSpeed;
        // 자동으로 회전하는 기능 비활성화.
        agent.updateRotation = false;

        enemyTr = GetComponent<Transform>();

        // 하이어라키에서 "오브젝트이름" 으로된 오브젝트를 검색.
        var group = GameObject.Find("WayPointGroup");
        // 오브젝트 정보가 존재할 경우에 실행됨.
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0); // RemoveAt(i) 리스트에 들어가 있는 요소들 중에서 지정된 인덱스(i)의 오브젝트 삭제.
                                   // 하기 싫으면 랜덤에서 1부터 시작하게 하면됨.
            nextIdx = Random.Range(0, wayPoints.Count);
        }

        // 웨이포인트 변경하는 함수 호출.
        MoveWayPoint();
    }

    void MoveWayPoint()
    {
        // isPathStale 경로 계산중일 때는 true 끝나면 false 반환.
        // 거리 계산중일때는 순찰 경로 변경하지 않도록 하기 위함.
        if (agent.isPathStale)
            return;

        // 만들어둔 Point 들 중에서 한 곳으로 목적지를 설정.
        agent.destination = wayPoints[nextIdx].position;

        // 네비게이션 기능 활성화 해서 이동 시작하도록 변경.
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos; // 추적 대상 지정;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        // 바로 정지하기위하여 잔여속도 0으로 초기화.
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.isStopped) // 적이 움직이는 중일때
        {
            // 적이 진행해야될 방향 벡터를 통해서 회전 각도를 계산.
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            // 보간 함수를 사용해서 점진적으로 회전(천천히 돌려라)
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (!_patrolling)
            return;

        // 목적지에 도착했는지 판단하기위한 조건.
        // 속도가 0.2보다 크고 남은 이동거리가 0.5 이하일 경우.
        // =목적지에 거의 도착했을때.
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f // sqrMagnitude 가 Magnitude보다 성능이좋음.
            && agent.remainingDistance <= 0.5f)
        {
            // nextIdx++;
            // 0 1 2 3 4 5 6 ...... wayPoints를 10이라고 가정
            // 0 % 10 = 0
            // 1 % 10 = 1
            // 10 % 10 = 0
            // 순환구조를 이루기 위하여 나머지 연산을 함.
            // 처음부터 마지막 순찰지 돌고나면 다시 처음으로 돌아가도록 인덱스 변경.
            // nextIdx = nextIdx % wayPoints.Count;
            // 위 코드는 순찰 지점을 순차적으로 순환하도록 했으므로 주석처리함.

            nextIdx = Random.Range(0, wayPoints.Count);

            // 인덱스 변경후 이동시작 하기 위해 함수 호출.
            MoveWayPoint();
        }
    }
}
