using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class InAirState : PlayerState
{
    protected bool _isJumpState = false;
    public InAirState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _controller.HandleInAir();
        _isJump = false;
        _isJumpState = false;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    public override void DoChecks()
    {
        base.DoChecks();
        if((_properties.Status.CurrentJump > 0 || !_isJump) && _properties.Status.IsGrounded)
        {
            _stateMachine.ChangeState(_controller.Idle);
        }
        else if(!_isJumpState)
        {
            _isJump = false;
        }
    }
    public override void Exit() { 
        base.Exit();
        _isJump = false;
    }

    

}
