using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private float _chaseSpeed = 3f;
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

        Vector2 moveDirection = (enemy.PlayerTransform.position - enemy.transform.position).normalized;

        enemy.Move(moveDirection * _chaseSpeed);

        if (enemy.IsWithInStrikingDistance)
        {
            enemy.EnemyStateMachine.ChangeState(enemy.AttackState);

            _animator.Play("Attack", 0, 0);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
