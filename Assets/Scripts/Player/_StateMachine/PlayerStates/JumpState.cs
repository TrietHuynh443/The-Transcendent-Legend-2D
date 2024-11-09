using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class JumpState : InAirState
{
    public int currentJump = 0;
    public JumpState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _isJump = true; 
        _isJumpState = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        _isJump = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (_isJump)
        {
            _isJump = false;
            Debug.Log("Jumping");
            _controller.DoJump();
        }
        if(_isAttack)
        {
            _isAttack = false;
            _stateMachine.ChangeState(_controller.Attack);
        }
    }

}
