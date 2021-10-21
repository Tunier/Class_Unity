using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Blue : Enemy
{
    Enemy_Red red;

    Vector3 destinationPos;

    bool isArrive = false;

    void Start()
    {
        red = FindObjectOfType<Enemy_Red>();

        Invoke("AttackStance", 10f);
    }

    protected override void Update()
    {
        base.Update();

        isArrive = Vector3.Distance(transform.position, destinationPos) < 2f;
    }

    protected override void Move(eState _state)
    {
        switch (_state)
        {
            case eState.Idle:
                nav.ResetPath();
                break;
            case eState.Run:
                destinationPos = transform.position;
                break;
            case eState.Attack:
                if (isArrive)
                {
                    destinationPos = GetDestinationPos();

                    nav.SetDestination(destinationPos);
                }
                break;
            default:
                CancelInvoke();
                break;
        }
    }

    void AttackStance()
    {
        state = eState.Attack;

        destinationPos = GetDestinationPos();

        nav.SetDestination(destinationPos);
    }

    Vector3 GetDestinationPos()
    {
        var pos = new Vector3(-red.transform.position.x + player.transform.position.x, 0.5f, -red.transform.position.z + player.transform.position.z);

        if (pos.x < -10)
            pos.x = -10;
        else if (pos.x > 10)
            pos.x = 10;

        if (pos.z > 8.5f)
            pos.z = 8.5f;
        else if (pos.z < -8.5f)
            pos.z = -8.5f;

        return pos;
    }

    protected override IEnumerator Revive()
    {
        isRevive = true;
        yield return new WaitForSeconds(5f);

        state = eState.Attack;
        
        destinationPos = GetDestinationPos();
        nav.SetDestination(destinationPos);
        
        isRevive = false;
    }
}