using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private Animator _animator;
    public EnemyMoveState(Enemy enemy, EnemyStateMachine EnemyStateMachine) : base(enemy, EnemyStateMachine) 
    { 
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        _animator = enemy.GetComponent<Animator>();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        if (enemy.IsWithInStrikingDistance && !enemy.IsAttackCoolDown)
        {
            enemy.EnemyStateMachine.ChangeState(enemy.AttackState);

            _animator.Play("Attack");
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.MoveStateHandle();
    }
}
