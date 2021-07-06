using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCtrl : MonoBehaviour
{
    public GameObject hpPotion;
    public GameObject damageText;
    public GameManager gm;
    public Canvas canvas;

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
    public int level = 1;
    public float hp;
    public float hpMax;
    public float exp;
    public float moveSpeed; // �̵��ӵ� ���

    float dieAfterTime = 0; // �װ��� �ð�.
    public int attackType = 0; // �޼����� ���������� ���� Ÿ�� ����.

    public bool hitable = true; // �������� ��Ʈ�� �����ϴ� ����

    Quaternion targetRot; // �÷��̾������� �ٶ� ����

    Transform tr;
    Rigidbody rb;
    Animator ani;

    PlayerCtrl player;
    public Collider[] weapon;

    readonly int hashMove = Animator.StringToHash("IsMove"); // bool �����̴���
    readonly int hashAttack = Animator.StringToHash("IsAttack"); // bool ������
    readonly int hashAttackType = Animator.StringToHash("AttackType"); // int ���ݿ��ϸ��̼�����
    readonly int hashChase = Animator.StringToHash("IsChase"); // bool ������
    readonly int hashHit = Animator.StringToHash("IsHit"); // bool �´���
    readonly int hashDie = Animator.StringToHash("IsDie"); // bool ����.

    private void Awake()
    {
        player = GameObject.FindWithTag("PLAYER").GetComponent<PlayerCtrl>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    void Start()
    {
        sName = "��";
        exp = 150f;
        moveSpeed = 2f;

        canvas = GameObject.Find("DamageTextCanvas").GetComponent<Canvas>();
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            if (state != State.DIE)
            {
                if (player.state != PlayerCtrl.State.DIE)
                {
                    if (Vector3.Distance(tr.position, player.transform.position) <= 2.7f)
                        state = State.ATTACK;
                    else if (!(ani.GetBool(hashAttack)) && (Vector3.Distance(tr.position, player.transform.position) <= 7f))
                        state = State.CHASE;
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

        switch (state)
        {
            case State.IDLE:
                //ani.SetBool(hashAttack, false);
                ani.SetBool(hashChase, false);
                rb.velocity = Vector3.zero;
                break;
            case State.ATTACK:
                ani.SetBool(hashAttack, true);
                ani.SetBool(hashChase, false);
                targetRot = Quaternion.LookRotation(player.transform.position - tr.position);
                tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, 60 * Time.deltaTime);
                rb.velocity = Vector3.zero;
                break;
            case State.CHASE:
                ani.SetBool(hashChase, true);
                targetRot = Quaternion.LookRotation(player.transform.position - tr.position);
                tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, 120 * Time.deltaTime);
                rb.velocity = tr.forward * moveSpeed;
                break;
            case State.DIE:
                ani.SetBool(hashDie, true);
                for (int i = 0; i < 2; i++)
                    weapon[i].enabled = false;
                rb.velocity = Vector3.zero;
                CorpseDestroy();
                break;
        }
    }


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


    public virtual void Attacking()
    {
        weapon[attackType].enabled = true;
    }

    public virtual void EndAttacking()
    {
        weapon[attackType].enabled = false;

        ani.SetBool(hashAttack, false);
    }

    public virtual void Hit(float damage)
    {
        if (hitable)
        {
            hitable = false;
            if (hp > 0)
            {
                hp -= damage;
                PrintDamageText(damage);
            }
        }

        if (hp < 0)
            hp = 0;

        Invoke("HitableTranceTure", 0.25f);
    }

    public void PrintDamageText(float damage)
    {
        GameObject obj = Instantiate(damageText, canvas.gameObject.transform);

        obj.GetComponent<FloatingTextCtrl>().damage = damage;
    }

    public void HitableTranceTure()
    {
        hitable = true;
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        state = State.DIE;

        for (int i = 0; i < 2; i++)
            weapon[i].enabled = false;

        gm.mobList.Remove(gameObject);

        CreateItem();
    }

    void CorpseDestroy()
    {
        dieAfterTime += Time.deltaTime;
        if (dieAfterTime >= 0.5f)
            GetComponent<Collider>().enabled = false;
        
        if (dieAfterTime >= 1.5f)
            tr.Translate(Vector3.down * 0.3f * Time.deltaTime);

        if (tr.position.y <= -1.62f)
            Destroy(gameObject);
    }

    void CreateItem()
    {
        float itemNum = Random.Range(1f, 100f);

        if (itemNum >= 50f)
        {
            Instantiate(hpPotion, tr.position, Quaternion.identity);
        }
    }
}
