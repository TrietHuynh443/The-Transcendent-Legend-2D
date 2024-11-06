using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class AttackState : PlayerState
{
    private GameObject _attackCollider;
    public AttackState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName, GameObject attackCollider) : base(controller, stateMachine, properties, animBoolName)
    {
        _attackCollider = attackCollider;
    }
    public override void Enter()
    {
        base.Enter();
        _attackCollider.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        _attackCollider.SetActive(false);
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        _controller.HandleVerticalVelocity();
        _controller.HandlerHorizontal();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        if (_properties.Status.IsGrounded)
        {
            _stateMachine.ChangeState(_controller.Idle);
        }
        else
        {
            _stateMachine.ChangeState(_controller.InAir);
        }
    }
}
