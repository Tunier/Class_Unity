using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public GameObject[] item;

    public enum State
    {
        IDLE,
        MOVE,
        CHASE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public string sName;
    public int level;
    public float hp;
    public float hpMax;
    public float exp;
    public float moveSpeed; // �̵��ӵ� ���

    float dieAfterTime = 0; // �װ��� �ð�.
    public int attackType = 0; // �޼����� ���������� ���� Ÿ�� ����.

    Quaternion targetRot; // �÷��̾������� �ٶ� ����

    Transform tr;
    Rigidbody rb;
    Animator ani;
    NavMeshAgent nav;
    public Collider[] weapon;

    PlayerCtrl player;
    MonsterSpawnerCtrl mobSpawner;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������
    readonly int hashAttackType = Animator.StringToHash("AttackType"); // int ���ݿ��ϸ��̼�����
    readonly int hashChase = Animator.StringToHash("IsChase"); // bool ������
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool �´���
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool ����.

    Vector3 destination;

    void Start()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
        mobSpawner = GameObject.Find("MonsterSpawner").GetComponent<MonsterSpawnerCtrl>();

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        sName = "��";
        exp = 100f;
        moveSpeed = 1.5f;
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if (state != State.DIE)
            {
                if (player.state != PlayerCtrl.State.DIE)
                {
                    if (!(ani.GetBool(hashAttack)) && (Vector3.Distance(tr.position, player.transform.position) <= 7f))
                        state = State.CHASE;
                    else if (Vector3.Distance(tr.position, player.transform.position) <= 2.7f)
                        state = State.ATTACK;
                    else
                        state = State.IDLE;
                }
                else
                {
                    state = State.IDLE;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Update()
    {
        StartCoroutine(CheckState());

        destination = player.transform.position;

        switch (state)
        {
            case State.IDLE:
                ani.SetBool(hashChase, false);
                nav.ResetPath();
                break;
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                ani.SetBool(hashChase, false);
                targetRot = Quaternion.LookRotation(player.transform.position - tr.position);
                tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, 60 * Time.deltaTime);
                nav.ResetPath();
                break;
            case State.CHASE:
                ani.SetBool(hashChase, true);
                nav.SetDestination(destination);
                nav.speed = moveSpeed;
                break;
            case State.DIE:
                CorpseDestroy();
                ani.SetBool(hashDie, true);
                for (int i = 0; i < 2; i++)
                    weapon[i].enabled = false;
                nav.enabled = false;
                break;
        }
    }

    // ���ϸ��̼� �̺�Ʈ�� �پ��ִ�.
    public virtual void ChangeAttackType()
    {
        if (attackType == 0)
            attackType++;
        else
            attackType--;

        if (attackType == 0)
            ani.SetInteger(hashAttackType, 0);
        else
            ani.SetInteger(hashAttackType, 1);
    }

    // ���ϸ��̼� �̺�Ʈ�� �پ��ִ�.
    public virtual void Attacking()
    {
        weapon[attackType].enabled = true;
    }

    // ���ϸ��̼� �̺�Ʈ�� �پ��ִ�.
    public virtual void EndAttacking()
    {
        weapon[attackType].enabled = false;

        ani.SetBool(hashAttack, false);
    }

    ///<summary>
    ///Hit(������, ũ��Ƽ������ �ƴ���)
    ///</summary>
    public virtual void Hit(float damage, bool crit)
    {
        hp -= damage;

        UIManager.instance.PrintDamageText(damage, crit);

        if (hp <= 0)
        {
            hp = 0;
            Die();
            player.exp += exp;
        }
    }

    public IEnumerator MultyHit(float damage, int attackTimes, float delay, bool isCrit)
    {
        for (int i = 0; i < attackTimes; i++)
        {
            Hit(damage, isCrit);
            yield return new WaitForSeconds(delay);
        }
    }

    // Ÿ�� ��󿡼� �ڷ�ƾ�� �����ϸ� Ÿ�� ����� ��������(ex : ��ų�� �ð��� ������ ������) �ڷ�ƾ�� �����⶧���� ���Ϳ��� �ڷ�ƾ ����
    ///<summary>
    ///StartMultyHit(������, Ÿ��Ƚ��, Ÿ�ݰ��� ������, ũ��Ƽ������ �ƴ���)
    ///</summary>
    public void StartMultyHit(float damage, int attackTimes, float delay, bool isCrit)
    {
        StartCoroutine(MultyHit(damage, attackTimes, delay, isCrit));
    }

    ///<summary>
    ///����ڷ�ƾ����, ���� ������� ����, ���Ӹ޴����� �� ����Ʈ���� ����(���� ���� -1), ������ ��� ����
    ///</summary>
    public virtual void Die()
    {
        StopAllCoroutines();

        state = State.DIE;

        for (int i = 0; i < 2; i++)
            weapon[i].enabled = false;

        mobSpawner.mobList.Remove(gameObject);

        DropItem();

        if (player.exp >= player.expMax)
        {
            player.LevelUp();
        }
    }


    ///<summary>
    ///���°� ����� �ȴ��� 0.5�ʰ� ������ �ݶ��̴� ��Ȱ��ȭ, 2�ʰ� ������ ��ü�� ����ɱ� ����, �����̻� ��������� Destroy
    ///</summary>
    void CorpseDestroy()
    {
        dieAfterTime += Time.deltaTime;
        if (dieAfterTime >= 0.5f)
            GetComponent<Collider>().enabled = false;

        if (dieAfterTime >= 2f)
            tr.Translate(Vector3.down * 0.2f * Time.deltaTime);

        if (tr.position.y <= -1.62f)
            Destroy(gameObject);
    }

    ///<summary>
    ///��� ����ؼ�(����� �׽�Ʈ�� ���Ͽ� 70%Ȯ���� å���Ǿ�����) ������ ������ ���.
    ///</summary>
    void DropItem()
    {
        float itemDropChance = Random.Range(1f, 100f); // �÷԰��� 1�� 100 ��� ����.
        int itemType = Random.Range(0, item.Length); // ��Ʈ�� ����̶� 1������

        if (itemDropChance >= 30f)
        {
            var obj = Instantiate(item[itemType], tr.position, Quaternion.identity);

            obj.transform.Translate(new Vector3(0, 0.85f, 0));
        }
    }
}
