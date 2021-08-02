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

        // �ð� ���� ������ 0.3 ������ ����.
        // �ð� ���� ������ �ڷ�ƾ �Լ����� ����.
        ws = new WaitForSeconds(0.3f);

        // Offset�� Speed���� �̿��ؼ� �ִϸ��̼� ������ �����ϰ� ����
        // �ӵ��� ���ݾ� �ٸ��� �������.
        animator.SetFloat(hashOffset, Random.Range(0f, 1f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 1.2f));

        enemyFOV = FindObjectOfType<EnemyFOV>();
    }

    private void OnEnable()
    {
        // OnEnable�� �ش� ��ũ��Ʈ�� Ȱ��ȭ�� ������ �����.
        // ���� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��.
        StartCoroutine(CheckState());
        // ���� ��ȭ�� ���� �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��.
        StartCoroutine(Action());

        // Damage ��ũ��Ʈ�� OnPlayerDieEvent �̺�Ʈ��
        // EnemyAi ��ũ��Ʈ�� OnPlayerDie �Լ��� ���������.
        Damage.OnPlayerDieEvent += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        // ��ũ��Ʈ�� ��Ȱ��ȭ �� ������ �̺�Ʈ�� ����� �Լ� ���� ������ ��Ģ������.
        Damage.OnPlayerDieEvent -= this.OnPlayerDie;
    }

    // �ڷ�ƾ = �������� �ð����� ����.
    IEnumerator CheckState() // ����üũ �ڷ�ƾ �Լ�.
    {
        yield return new WaitForSeconds(1f);

        while (!isDie) // ���� ����ִµ��� ��� ����ǵ��� while���.
        {
            if (state == State.DIE)
                yield break; // �ڷ�ƾ �Լ� ����.
            // Distance(A,B) - A�� B������ �Ÿ��� ������ִ� �Լ�.
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist) // ���� ��Ÿ� �̳��� �������� ����.
            {
                if (enemyFOV.isViewPlayer())
                    state = State.ATTCK; // ��ֹ� ������ ����
                else
                    state = State.TRACE; // ��ֹ� ������ ����
            }
            else if (enemyFOV.isTracePlayer()) // ���� ��Ÿ� �̳��� �������� ����.
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
                    gameObject.tag = "Untagged";

                    isDie = true;
                    enemyFire.isFire = false;

                    moveAgent.Stop();
                    // �������� ���ؼ� �״� ��� ����
                    animator.SetInteger(hashDieIdx, Random.Range(0, 3));
                    animator.SetTrigger(hashDie);
                    // ����� �����ִ� �ݶ��̴� ��Ȱ��ȭ �ؼ� ��� �浹���� �ʵ��� ��.
                    GetComponent<CapsuleCollider>().enabled = false;
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

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        // ��� �ڷ�ƾ �Լ� ����
        // ���ѻ��� �ӽ� ���� �ؾߵ�.
        StopAllCoroutines();

        animator.SetTrigger(hashPlayerDie);
    }
}
