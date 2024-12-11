using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 _targetPoint;
    private bool _isFacingRight = false;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine EnemyStateMachine) : base(enemy, EnemyStateMachine) { }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        // _targetPoint = enemy.transform.position + new Vector3(-enemy.MoveRange, 0, 0);
        // Debug.Log(_targetPoint);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (enemy.IsAggroed)
        {
            // Debug.Log("Hello from Idle to Move");
            enemy.EnemyStateMachine.ChangeState(enemy.MoveState);

            Animator animator = enemy.GetComponent<Animator>();

            animator.Play("Move", 0, 0);

        }
        enemy.IdleStateHandle();
    }
}
