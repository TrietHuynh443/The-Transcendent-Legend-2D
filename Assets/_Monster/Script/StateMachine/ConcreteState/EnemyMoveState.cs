using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private float _chaseSpeed = 3f;
    private Transform _playerTransform;
    public EnemyMoveState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) 
    { 
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector2 moveDirection = (_playerTransform.position - enemy.transform.position).normalized;

        enemy.Move(moveDirection * _chaseSpeed);

        if (enemy.isWithInStrikingDistance)
        {
            Debug.Log("Hello from Move to Attack");
            enemy.enemyStateMachine.ChangeState(enemy.attackState);
            Animator animator = enemy.GetComponent<Animator>();

            animator.Play("Attack", 0, 0);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
