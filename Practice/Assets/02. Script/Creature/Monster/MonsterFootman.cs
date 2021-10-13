using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFootman : MonsterBase
{
    [SerializeField]
    GameObject attackEffect;

    GameObject playerGo;
    public GameObject minimapCube;
    public GameObject hpCanvas;

    PlayerInfo player;
    Transform playerTr;
    Inventory inven;

    public GameObject group;            //���ͺ� ��������Ʈ ���� ���� �־��ֱ�
    public int nextIdx;                 //���� ���� ������ �ε���
    public float minDist = -1f;          //�ּ� ���ݰŸ�
    public float maxDist = 0f;
    public float attackDist = 4f;       //�ִ� ���ݰŸ�
    public float traceDist = 15f;       //���� �Ÿ�

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
    public bool isHit = false;

    float dist; //�÷��̾�� ���� �Ÿ�
    Vector3 monsterTr;
    NavMeshAgent agent;
    MonsterAnim monsterAnim;

    //public Collider[] monsters; //�׽�Ʈ
    public List<Collider> monsters = new List<Collider>();
    public Coroutine checkState;
    private void Awake()
    {
        monsterAnim = GetComponent<MonsterAnim>();
        monsterCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        playerGo = GameObject.FindGameObjectWithTag("Player");
        player = playerGo.GetComponent<PlayerInfo>();
        inven = FindObjectOfType<Inventory>();

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

        if (playerGo != null)
        {
            playerTr = playerGo.GetComponent<Transform>();
        }
        stats.s_Name = "���̷��� ���";
    }

    private void OnEnable()
    {
        StartCoroutine(Action());
        checkState = StartCoroutine(CheckState());

        minimapCube.SetActive(true);
        hpCanvas.SetActive(true);

        state = STATE.Patrol;
        GetComponent<CapsuleCollider>().enabled = true;

        isDie = false;
        isAnger = true;

        exp = 75f;
        dropGold = 60 + Random.Range(0, 6);
        finalNormalAtk = 30f;
        finalMaxHp = 125f;
        finalNormalDef = 0f;
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
        dist = Vector3.Distance(playerTr.position, transform.position);
    }

    public void MovePoint()
    {
        agent.enabled = true;

        if (agent.isPathStale)
            return;

        if (!isAttack)
        {
            agent.isStopped = false;
            agent.destination = movePoints[nextIdx].position;
            agent.speed = patrolSpeed;
            monsterAnim.OnMove(true, agent.speed);

            if (agent.velocity.magnitude < 1.5f && agent.remainingDistance <= 1.5f)
            {
                nextIdx = Random.Range(0, movePoints.Count);
            }
        }
    }

    public void Chase(Vector3 _target)
    {
        agent.enabled = true;

        if (agent.isPathStale)
            return;

        if (!isAttack)
        {
            monsterAnim.OnMove(true, agent.speed);
            agent.speed = traceSpeed;
            agent.destination = _target;
            agent.isStopped = false;
        }
    }

    public void Attack(Vector3 _target)
    {

        if (Physics.Raycast(transform.position + (Vector3.up * 2.5f), transform.forward, attackDist * 1.5f, 1 << playerLayer))//Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f) //�þ߰�
        {
            agent.enabled = true;
            Stop();

            if (Time.time >= nextFire && !isAttack)
            {
                monsterAnim.OnAttack();
                nextFire = Time.time + attackRate + Random.Range(0.5f, 1f);
                isAttack = true;
            }
        }
        else
        {
            monsterAnim.OnMove(true, agent.speed);
            agent.enabled = false;

            Vector3 dir = new Vector3(_target.x, 0, _target.z) - new Vector3(transform.position.x, 0, transform.position.z);
            dir = dir.normalized;
            //agent.SetDestination(_target);
            //agent.speed = backSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 120f);
        }
    }

    public void BackMove(Vector3 _target)
    {
        agent.isStopped = false;
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

        movePoints.Clear();

        if (agent.enabled)
        {
            Stop();
        }

        isDie = true;
        isAttack = false;
        isHit = false;
        GetComponent<CapsuleCollider>().enabled = false;
        attackEffect.SetActive(false);

    }

    public override void DropItem()
    {
        int random = Random.Range(0, 10000);

        if (random >= 8000)
        {
            var item = ItemDatabase.instance.newItem("0000003");
            inven.GetItem(item);

            SystemText_ScrollView_Ctrl.Instance.PrintText(item.Name + " �� ȹ���߽��ϴ�.");
        }
        else if (random >= 6000)
        {
            var item = ItemDatabase.instance.newItem("0000007");
            inven.GetItem(item);

            SystemText_ScrollView_Ctrl.Instance.PrintText(item.Name + " �� ȹ���߽��ϴ�.");
        }
    }

    public override void Hit(float _damage)
    {
        isHit = true;
        agent.enabled = true;
        Stop();
        curHp -= _damage - finalNormalDef;

        if (curHp <= 0)
        {
            state = STATE.Die;

            player.GetExp(exp);
            player.GetGold(dropGold);

            if (player.stats.CurExp > player.stats.MaxExp)
                player.LevelUp();

            StopCoroutine(checkState);

            return;
        }

        var ary_monster = Physics.OverlapSphere(transform.position, traceDist * 3, 1 << monsterLayer);

        foreach (var monster in ary_monster)
        {
            if (!monsters.Contains(monster))
            {
                monsters.Add(monster);
            }
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == null)
                continue;
            else
            {
                var mob = monsters[i].GetComponent<MonsterBase>();

                if (mob.isAnger == false)
                {
                    mob.isAnger = true;
                }
            }
        }

        monsterAnim.OnHit();
    }

    public void ExitAttackMotion()
    {
        isAttack = false;
    }

    public void ExitHitMotion()
    {
        isHit = false;
    }
    public IEnumerator OnOffAttackEffect()
    {
        yield return new WaitForSeconds(0.1f);
        if (!isHit)
        {
            attackEffect.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        attackEffect.SetActive(false);
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(0.1f);

        while (state != STATE.Die || curHp >= 0) //�����ʰ� Anger�� true�϶� ���º�ȭ
        {
            if (isAnger)
            {
                if (dist <= maxDist)
                {
                    if (dist < minDist)
                    {
                        state = STATE.Backing;
                    }
                }
                else if (dist <= attackDist && dist > maxDist)
                {
                    state = STATE.Attacking;
                }
                else if (dist <= traceDist && dist > attackDist)
                {
                    state = STATE.Chase;
                }
                else
                {
                    state = STATE.Patrol;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// �������ͽ��� ���� �ִϸ��̼�,�ൿ�� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Action()
    {
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            switch (state)
            {
                case STATE.Patrol:
                    MovePoint();
                    break;
                case STATE.Chase:
                    Chase(playerTr.position);
                    break;
                case STATE.Attacking:
                    agent.velocity = Vector3.zero;
                    Attack(playerTr.position);
                    break;
                case STATE.Backing:
                    BackMove(playerTr.position);
                    break;
                case STATE.Die:
                    Die();
                    DropItem();
                    hpCanvas.SetActive(false);
                    minimapCube.SetActive(false);
                    StartCoroutine(co_MonsterDie());
                    yield break;
            }

            if (state == STATE.Die)
                break;

            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator co_MonsterDie()
    {
        yield return new WaitForSeconds(3f);

        spawner.spawnCount--;
        gameObject.SetActive(false);
    }
}
