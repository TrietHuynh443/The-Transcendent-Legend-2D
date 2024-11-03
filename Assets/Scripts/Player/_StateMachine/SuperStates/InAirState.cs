using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using Unity.VisualScripting;
using UnityEngine;

public class InAirState : PlayerState
{
    public InAirState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _controller.HandleInAir();
    }

}
