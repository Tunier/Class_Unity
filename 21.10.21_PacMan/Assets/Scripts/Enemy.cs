using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    public enum eState
    {
        Idle,
        Attack,
        Run,
        Die,
    }

    protected Player player;
    protected Transform homeTr;
    protected Transform runPointTr;

    protected Collider col;
    protected NavMeshAgent nav;

    [SerializeField] Material myMaterial;

    public eState state = eState.Idle;

    protected float speed = 3;
    protected float score = 50;

    protected bool isRevive = false;
    Color baseColor;

    protected void Awake()
    {
        player = FindObjectOfType<Player>();
        homeTr = GameObject.Find("Home").GetComponent<Transform>();
        runPointTr = GameObject.Find("RunPoint").GetComponent<Transform>();

        col = GetComponent<Collider>();
        nav = GetComponent<NavMeshAgent>();

        baseColor = myMaterial.color;

        nav.speed = speed;
    }

    protected virtual void Update()
    {
        Move(state);

        switch (state)
        {
            case eState.Run:
                myMaterial.color = Color.cyan;
                nav.SetDestination(runPointTr.position);
                break;
            case eState.Die:
                nav.SetDestination(homeTr.position);
                nav.speed = speed * 1.5f;

                col.enabled = false;
                myMaterial.color = Color.black;

                if (!isRevive)
                    StartCoroutine(Revive());
                break;
            default:
                col.enabled = true;
                myMaterial.color = baseColor;
                nav.speed = speed;
                break;
        }

        if (Stage_Manager.Instance.isClear || player.isDie)
            myMaterial.color = baseColor;
    }

    protected abstract void Move(eState _state);

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.canEat)
            {
                state = eState.Die;
                player.score += 50;
            }
            else
                player.isDie = true;
        }
    }

    protected virtual IEnumerator Revive()
    {
        isRevive = true;
        yield return new WaitForSeconds(5f);

        state = eState.Attack;
        isRevive = false;
    }
}