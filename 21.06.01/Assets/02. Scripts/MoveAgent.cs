using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MoveAgent : MonoBehaviour
{
    // List�� �迭�̴�.
    // ������ - �������̷μ� ���빰�� ���� ���̰� ����.
    public List<Transform> wayPoints;
    public int nextIdx; // ���� ���� ������ �迭 �ε���

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // �극��ũ�� ���� �ڵ� �������� �ʵ��� ����. �������� ����������� �ӵ��� ���̴� �ɼ�.
        agent.autoBraking = false;
        // ���̾��Ű���� "������Ʈ�̸�" ���ε� ������Ʈ�� �˻�.
        var group = GameObject.Find("WayPointGroup");
        // ������Ʈ ������ ������ ��쿡 �����.
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0); // RemoveAt(i) ����Ʈ�� �� �ִ� ��ҵ� �߿��� ������ �ε���(i)�� ������Ʈ ����.
                                   // �ϱ� ������ �������� 1���� �����ϰ� �ϸ��.
        }       

        // ��������Ʈ �����ϴ� �Լ� ȣ��.
        MoveWayPoint();
    }

    void MoveWayPoint()
    {
        // isPathStale ��� ������� ���� true ������ false ��ȯ.
        // �Ÿ� ������϶��� ���� ��� �������� �ʵ��� �ϱ� ����.
        if (agent.isPathStale)
            return;

        // ������ Point �� �߿��� �� ������ �������� ����.
        agent.destination = wayPoints[nextIdx].position;

        // �׺���̼� ��� Ȱ��ȭ �ؼ� �̵� �����ϵ��� ����.
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �������� �����ߴ��� �Ǵ��ϱ����� ����.
        // �ӵ��� 0.2���� ũ�� ���� �̵��Ÿ��� 0.5 ������ ���.
        // =�������� ���� ����������.
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f // sqrMagnitude �� Magnitude���� ����������.
            && agent.remainingDistance <= 0.5f)
        {
            nextIdx++;
            // 0 1 2 3 4 5 6 ...... wayPoints�� 10�̶�� ����
            // 0 % 10 = 0
            // 1 % 10 = 1
            // 10 % 10 = 0
            // ��ȯ������ �̷�� ���Ͽ� ������ ������ ��.
            // ó������ ������ ������ ������ �ٽ� ó������ ���ư����� �ε��� ����.
            nextIdx = nextIdx % wayPoints.Count;
            // �ε��� ������ �̵����� �ϱ� ���� �Լ� ȣ��.
            MoveWayPoint();
        }

    }
}
