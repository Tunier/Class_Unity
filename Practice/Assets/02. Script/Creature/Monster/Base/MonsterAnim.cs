using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    GameObject attackCollision;

    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashDie = Animator.StringToHash("IsDie");
    readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    readonly int hashHit = Animator.StringToHash("IsHit");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashIdleIdx = Animator.StringToHash("IdleIdx");
    readonly int hashWalk = Animator.StringToHash("IsWalk");
    readonly int hashTaunt = Animator.StringToHash("IsTaunt");
    readonly int hashCombo = Animator.StringToHash("IsCombo");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(bool _true, float _speed)
    {
        animator.SetBool(hashMove, _true);
        animator.SetInteger(hashWalk, (int)_speed);
    }

    public void OnDie()
    {
        animator.SetTrigger(hashDie);
    }

    public void OnDieIdx()
    {
        animator.SetInteger(hashDieIdx, Random.Range(0, 2));
    }

    public void OnAttack()
    {
        animator.SetTrigger(hashAttack);
    }

    public void OnHit()
    {
        animator.SetTrigger(hashHit);
    }

    public void OnIdle()
    {
        animator.SetInteger(hashIdleIdx, Random.Range(0, 3));
    }

    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }

    public void OnTaunt()
    {
        animator.SetTrigger(hashTaunt);
    }

    public void OnComboAttack()
    {
        animator.SetTrigger(hashCombo);
    }
}

