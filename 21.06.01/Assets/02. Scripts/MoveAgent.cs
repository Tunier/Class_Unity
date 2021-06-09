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
    public int nextIdx; // 다음 순찰 지점의 배열 인덱스

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // 브레이크를 꺼서 자동 감속하지 않도록 해줌. 목적지에 가까워질수록 속도를 줄이는 옵션.
        agent.autoBraking = false;
        // 하이어라키에서 "오브젝트이름" 으로된 오브젝트를 검색.
        var group = GameObject.Find("WayPointGroup");
        // 오브젝트 정보가 존재할 경우에 실행됨.
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0); // RemoveAt(i) 리스트에 들어가 있는 요소들 중에서 지정된 인덱스(i)의 오브젝트 삭제.
                                   // 하기 싫으면 랜덤에서 1부터 시작하게 하면됨.
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

    // Update is called once per frame
    void Update()
    {
        // 목적지에 도착했는지 판단하기위한 조건.
        // 속도가 0.2보다 크고 남은 이동거리가 0.5 이하일 경우.
        // =목적지에 거의 도착했을때.
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f // sqrMagnitude 가 Magnitude보다 성능이좋음.
            && agent.remainingDistance <= 0.5f)
        {
            nextIdx++;
            // 0 1 2 3 4 5 6 ...... wayPoints를 10이라고 가정
            // 0 % 10 = 0
            // 1 % 10 = 1
            // 10 % 10 = 0
            // 순환구조를 이루기 위하여 나머지 연산을 함.
            // 처음부터 마지막 순찰지 돌고나면 다시 처음으로 돌아가도록 인덱스 변경.
            nextIdx = nextIdx % wayPoints.Count;
            // 인덱스 변경후 이동시작 하기 위해 함수 호출.
            MoveWayPoint();
        }

    }
}
