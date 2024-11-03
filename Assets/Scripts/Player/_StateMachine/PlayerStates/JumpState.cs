using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class JumpState : InAirState
{
    public int currentJump = 0;
    private bool _isJump = false;
    public JumpState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _isJump = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_isJump && _properties.Status.IsGrounded){
            // _isJump = true;
            _stateMachine.ChangeState(_controller.Idle);
        }
    }
    public override void Exit()
    {
        base.Exit();
        currentJump = 0;
        _isJump = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector2 currentVelocity = _controller.Rigidbody.velocity;
        
        var newVelocity = new Vector2(
            _properties.Input.HorizontalInput,
            _properties.Data.JumpForce
            );

        if(_isJump && currentJump < _properties.Data.MaxJump){
            _isJump = false;
            _controller.Rigidbody.AddForce(newVelocity, ForceMode2D.Impulse);
            currentJump++;
        }
    }

}
