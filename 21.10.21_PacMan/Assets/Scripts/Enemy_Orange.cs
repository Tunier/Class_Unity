using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Orange : Enemy
{
    Enemy_Red red;

    Vector3 destinationPos;

    bool isArrive = false;

    int patternType = 0;

    void Start()
    {
        red = FindObjectOfType<Enemy_Red>();

        Invoke("AttackStance", 15f);
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
            case eState.Attack:
                if (isArrive)
                {
                    destinationPos = GetDestinationPos(patternType);

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

        StartCoroutine(ChagePattern());

        destinationPos = GetDestinationPos(patternType);

        nav.SetDestination(destinationPos);
    }

    Vector3 GetDestinationPos(int _type)
    {
        Vector3 pos;

        switch (_type)
        {
            case 0:
                pos = player.transform.position;
                break;
            case 1:
                pos = new Vector3(-red.transform.position.x + player.transform.position.x, 0.5f, -red.transform.position.z + player.transform.position.z);
                break;
            case 2:
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
                break;
            default:
                pos = player.transform.position;
                break;
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

    IEnumerator ChagePattern()
    {
        while (true)
        {
            if (state != eState.Die)
                patternType = Random.Range(0, 3);

            //Debug.Log($"Å¸ÀÔ ¹Ù²ñ {patternType}");

            yield return new WaitForSeconds(7.5f);
        }
    }

    protected override IEnumerator Revive()
    {
        isRevive = true;
        yield return new WaitForSeconds(5f);

        state = eState.Attack;

        if (patternType == 1)
        {
            destinationPos = GetDestinationPos(1);
            nav.SetDestination(destinationPos);
        }

        isRevive = false;
    }
}
