using System.Collections;
using System.Collections.Generic;
using GameEvent;
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
        _isAnimationFinished = false;
        EventAggregator.RaiseEvent<PlayerAttackEvent>(new PlayerAttackEvent());
        EventAggregator.RaiseEvent<AttackAchievementProgress>(new AttackAchievementProgress(){Count = 1, Damage = 0});
    }

    public override void Exit()
    {
        base.Exit();
        _attackCollider.SetActive(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(_isJump)
        {
            _isJump = false;
            _controller.DoJump();
        }
        if(_isAnimationFinished){
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

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        
        _isAnimationFinished = true;
        Debug.Log("on animation end");
    }
}
