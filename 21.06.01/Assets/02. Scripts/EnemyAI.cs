using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL, // ����
        TRACE, // ����
        ATTCK, // ����
        DIE // ���
    }

    public State state = State.PATROL; // �ʱ� ���� ����.

    Transform playerTr; // �÷��̾� ��ġ ���� ����.
    Transform enemyTr; // ��ĳ���� ��ġ ���� ����.

    public float attackDist = 5f; // ���� ��Ÿ�.
    public float traceDist = 10f; // ���� ��Ÿ�.
    public bool isDie = false; // ��� ���� �Ǵ� ����.

    WaitForSeconds ws; // �ð� ���� ����.

    MoveAgent moveAgent;
    EnemyFire enemyFire;

    Animator animator;
    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashSpeed = Animator.StringToHash("Speed");    

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");

        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();
        enemyFire = GetComponent<EnemyFire>();

        // �ð� ���� ������ 0.3 ������ ����.
        // �ð� ���� ������ �ڷ�ƾ �Լ����� ����.
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        // OnEnable�� �ش� ��ũ��Ʈ�� Ȱ��ȭ�� ������ �����.
        // ���� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��.
        StartCoroutine(CheckState());
        // ���� ��ȭ�� ���� �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��.
        StartCoroutine(Action());
    }

    // �ڷ�ƾ = �������� �ð����� ����.
    IEnumerator CheckState() // ����üũ �ڷ�ƾ �Լ�.
    {
        while (!isDie) // ���� ����ִµ��� ��� ����ǵ��� while���.
        {
            if (state == State.DIE)
                yield break; // �ڷ�ƾ �Լ� ����.
            // Distance(A,B) - A�� B������ �Ÿ��� ������ִ� �Լ�.
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist) // ���� ��Ÿ� �̳��� �������� ����.
            {
                state = State.ATTCK;
            }
            else if (dist <= traceDist) // ���� ��Ÿ� �̳��� �������� ����.
            {
                state = State.TRACE;
            }
            else // ���ݵ� ������ �ƴϸ� �������·� ����.
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
                    moveAgent.Stop();
                    break;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        // �ִϸ����� ������ Set �Լ����� ������ ���������� ����.
        // SetFloat �ش��Լ��� (�ؽ��� / �Ķ���� �̸�, �����ϰ��� �ϴ� ��) ���·� ����.
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
