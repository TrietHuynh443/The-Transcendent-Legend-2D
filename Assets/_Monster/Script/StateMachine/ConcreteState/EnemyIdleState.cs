using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 _targetPoint;
    private bool _isFacingRight = false;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();


        _targetPoint = enemy.transform.position + new Vector3(-enemy.moveRange, 0, 0);
        Debug.Log(_targetPoint);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector2 direction;
        if (_isFacingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
        enemy.Move(direction * enemy.moveSpeed);

        if ((enemy.transform.position - _targetPoint).sqrMagnitude < 0.01f)
        {
            if (_isFacingRight)
            {
                _targetPoint = enemy.transform.position + new Vector3(-enemy.moveRange, 0, 0) * 2;
            }
            else
            {
                _targetPoint = enemy.transform.position + new Vector3(enemy.moveRange, 0, 0) * 2;
            }
            _isFacingRight = !_isFacingRight;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
