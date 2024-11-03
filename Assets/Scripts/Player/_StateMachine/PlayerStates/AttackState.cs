using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
    {
    }

}
