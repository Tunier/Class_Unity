using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Red : Enemy
{
    void Start()
    {
        state = eState.Attack;
    }

    protected override void Move(eState _state)
    {
        switch (_state)
        {
            case eState.Idle:
                nav.ResetPath();
                break;
            case eState.Attack:
                nav.SetDestination(player.transform.position);
                break;
        }
    }
}