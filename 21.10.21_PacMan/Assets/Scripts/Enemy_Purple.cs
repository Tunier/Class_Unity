using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Purple : Enemy
{
    Vector3 destinationPos;

    void Start()
    {
        Invoke("AttackStance", 5);
    }

    protected override void Move(eState _state)
    {
        switch (_state)
        {
            case eState.Idle:
                nav.ResetPath();
                break;
            case eState.Attack:
                destinationPos = GetDestinationPos();

                nav.SetDestination(destinationPos);
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
        Vector3 pos;
        RaycastHit hit;

        if (player.x == 1)
        {
            if (Physics.Raycast(player.transform.position, player.transform.right, out hit, 4f))
                pos = hit.point;
            else
                pos = player.transform.position + player.transform.forward * 4;
        }
        else if (player.x == -1)
        {
            if (Physics.Raycast(player.transform.position, -player.transform.right, out hit, 4f))
                pos = hit.point;
            else
                pos = player.transform.position + player.transform.forward * 4;
        }
        else if (player.z == -1)
        {
            if (Physics.Raycast(player.transform.position, -player.transform.forward, out hit, 4f))
                pos = hit.point;
            else
                pos = player.transform.position + player.transform.forward * 4;
        }
        else
        {
            if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 4f))
                pos = hit.point;
            else
                pos = player.transform.position + player.transform.forward * 4;
        }

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
}
