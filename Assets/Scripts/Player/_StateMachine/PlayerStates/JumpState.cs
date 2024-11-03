using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class JumpState : InAirState
{
    public int currentJump = 0;
    private bool _isJump = true;
    public JumpState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        ++currentJump;
        _isJump = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_isJump && _properties.Status.IsGrounded){
            _isJump = false;
            _stateMachine.ChangeState(_controller.OnGroundState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        currentJump = 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector2 currentVelocity = _controller.Rigidbody.velocity;
        if(_properties.Input.IsJumpInput && currentJump < _properties.Data.MaxJump){
            _controller.Rigidbody.AddForce(Vector2.up * _properties.Data.JumpForce);
            currentJump++;
        }
        _properties.Input.IsJumpInput = false;
        _controller.Rigidbody.velocity = new Vector2(
            _properties.Input.HorizontalInput,
            currentVelocity.y
            );
    }

}
