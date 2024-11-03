using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.MoveState;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class IdleState : OnGroundedState
{
    public IdleState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }
    public override void LogicUpdate()
    {
        if(Mathf.Abs(_properties.Input.HorizontalInput) >= 0.001f){
            Debug.Log("Idle " + _properties.Input.HorizontalInput);
            _stateMachine.ChangeState(_controller.MoveState);
        }
    }
}
