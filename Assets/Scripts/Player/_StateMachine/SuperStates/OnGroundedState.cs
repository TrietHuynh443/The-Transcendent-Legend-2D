using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class OnGroundedState : PlayerState
{
    public OnGroundedState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();        
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_properties.Input.IsJumpInput){
            Debug.Log("jump Input");
            _stateMachine.ChangeState(_controller.Jump);
        }
        else if (_properties.Input.IsAttackInput) {
            Debug.Log("attack Input");
            _stateMachine.ChangeState(_controller.Attack);
        }

    }
}
