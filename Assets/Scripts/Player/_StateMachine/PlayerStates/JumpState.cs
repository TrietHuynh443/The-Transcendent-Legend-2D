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

        if (_properties.Input.IsJumpInput && currentJump < _properties.Data.MaxJump) {
            _isJump = true;
        }


    }
    public override void Exit()
    {
        base.Exit();
        currentJump = 0;
        _isJump = false;
        _controller.HandleOnGround();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector2 currentVelocity = _controller.Rigidbody.velocity;
        var horizontalMove = _properties.Input.HorizontalInput* _properties.Data.Speed;
        var newVelocity = new Vector2(
            0,
            _properties.Data.JumpForce
            );

        if (_isJump)
        {
            ++currentJump;
            _isJump = false;
            _properties.Input.IsJumpInput = false;
            if(currentJump == _properties.Data.MaxJump)
            {
                newVelocity /= Mathf.Sqrt(2);
            }
            _controller.Rigidbody.AddForce(newVelocity, ForceMode2D.Impulse);
            Debug.Log("current Jump: " + currentJump);
        }
        _controller.Rigidbody.AddForce(new Vector2(horizontalMove, 0), ForceMode2D.Force);
    }

}
