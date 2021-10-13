using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAction : MonsterBase
{
    public PlayerInfo player;

    public int attackMetod;             //0:�������� 1:ȭ����� 2:��������

    public GameObject group;            //���ͺ� ��������Ʈ ���� ���� �־��ֱ�
    public int nextIdx;                 //���� ���� ������ �ε���
    public float minDist = 4f;          //�ּ� ���ݰŸ�
    public float maxDist = 10f;
    public float attackDist = 14f;       //�ִ� ���ݰŸ�
    public float traceDist = 20f;       //���� �Ÿ�

    public float attackRate = 0.2f;     //���ݵ�����
    public float nextFire = 0f;

    public float patrolSpeed = 4f;      //�ϻ�ӵ�
    public float traceSpeed = 10f;      //�����ӵ�
    public float backSpeed = 20f;       //�ڷ� �������� �ӵ�

    private int obstacleLayer;          //��ֹ� ���̾�
    private int playerLayer;            //�÷��̾� ���̾�
    private int monsterLayer;           //���� ���̾�

    private Collider monsterCollider;

    public bool isDie = false;
    public bool isAttack = false;

    Vector3 monsterTr;
    NavMeshAgent agent;
    MonsterAnim monsterAnim;
    MonsterFire monsterFire;
    MonsterAi monsterAi;

    //public Collider[] monsters; //�׽�Ʈ
    public List<Collider> monsters = new List<Collider>();

    private void Awake()
    {
        monsterFire = GetComponent<MonsterFire>();
        monsterAnim = GetComponent<MonsterAnim>();
        monsterAi = GetComponent<MonsterAi>();
        monsterCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        playerLayer = LayerMask.NameToLayer("Player");
        monsterLayer = LayerMask.NameToLayer("Monster");
        monsterTr = transform.position + (Vector3.up * 2);

        if (group)
        {
            movePoints.AddRange(group.GetComponentsInChildren<Transform>());
            movePoints.RemoveAt(0);
            nextIdx = Random.Range(0, movePoints.Count);
        }

        dropGold = 15;
    }

    private void OnEnable()
    {
        finalMaxHp = 50f;
        curHp = finalMaxHp;
    }

    private void Update()
    {
        foreach (var obj in monsters)
        {
            if (obj == null)
            {
                monsters.Remove(obj);
            }
        }
    }


    /// <summary>
    /// �������� �̵� & ���� �޽Ľð�
    /// </summary>
    public void MovePoint()
    {
        isAttack = false;

        if (agent.isPathStale)
            return;

        agent.isStopped = false;
        agent.destination = movePoints[nextIdx].position;
        agent.speed = patrolSpeed;
        monsterAnim.OnMove(true, agent.speed);

        if (agent.velocity.magnitude < 1.5f && agent.remainingDistance <= 1.5f)
        {
            nextIdx = Random.Range(0, movePoints.Count);
        }
    }

    public void Chase(Vector3 _target)
    {
        isAttack = false;

        if (agent.isPathStale)
            return;

        monsterAnim.OnMove(true, agent.speed);
        agent.speed = traceSpeed;
        agent.destination = _target;
        agent.isStopped = false;
    }

    public void Attack(Vector3 _target)
    {
        Vector3 dir = (_target - transform.position).normalized;
        if (Physics.Raycast(transform.position + (Vector3.up * 2.5f), transform.forward, attackDist * 1.5f, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //�þ߰�
        {
            if (isAttack == false)
            {
                isAttack = true;
            }
            Stop();
            if (Time.time >= nextFire)
            {
                monsterAnim.OnAttack();
                nextFire = Time.time + attackRate + Random.Range(0.5f, 1f);
            }
        }
        else
        {
            if (isAttack == true)
            {
                isAttack = false;
            }

            monsterAnim.OnMove(true, agent.speed);
            agent.SetDestination(_target);
            agent.speed = backSpeed;
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);

        }
    }

    public void BackMove(Vector3 _target)
    {
        agent.isStopped = false;
        isAttack = false;
        monsterAnim.OnMove(true, agent.speed);
        Vector3 dir = (transform.position - _target).normalized;
        if (Physics.Raycast(monsterTr, -transform.forward, 5f, 1 << obstacleLayer))
        {
            Debug.Log("�ɽ�ŸŬ����");
            //�ڷ� �����Ҷ� �ɽ�ŸŬ ���ϴ� ��ġ�� �������� �����ϴ� �Լ� �ֱ�
        }
        else
        {
            Debug.Log("�ɽ�ŸŬ ����");
            agent.SetDestination(transform.position + dir * traceDist);
            agent.speed = backSpeed;
            if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f)
            {
                if (agent.remainingDistance <= 0.5f)
                {
                    state = STATE.Chase;
                }
            }
        }
    }

    /// <summary>
    /// Agent ���ߴ� �Լ�
    /// </summary>
    public void Stop()
    {
        monsterAnim.OnMove(false, agent.speed);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    public override void Die()
    {
        monsterAnim.OnDie();
        monsterAnim.OnDieIdx();

        Stop();
        isDie = true;
        isAttack = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public override void DropItem()
    {
    }

    public override void Hit(float _damage)
    {
        curHp -= _damage - finalNormalDef;

        if (curHp <= 0)
        {
            state = STATE.Die;

            player.stats.CurExp += exp;
            player.stats.Gold += dropGold;

            Debug.Log("����ġ "+ exp + "ȹ��");
            Debug.Log("��� " + dropGold + "ȹ��");

            if (player.stats.CurExp >= player.stats.MaxExp)
                player.LevelUp();

            monsterAi.StopCoroutine(monsterAi.checkState);

            return;
        }

        if (!monsters.Contains(monsterCollider))
        {
            monsters.AddRange(Physics.OverlapSphere(monsterTr, traceDist * 3f, 1 << monsterLayer));
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == null)
                continue;
            else
            {
                var mob = monsters[i].GetComponent<MonsterAction>();

                if (mob.isAnger == false)
                {
                    mob.isAnger = true;
                }
            }
        }

        monsterAnim.OnHit();
    }
}

