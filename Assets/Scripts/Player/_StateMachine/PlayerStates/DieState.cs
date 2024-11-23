using System;
using System.Collections.Generic;
using System.Text;

namespace Player.PlayerStates
{
    public class DieState : PlayerState
    {
        public DieState(PlayerController controller, PlayerStateMachine.PlayerStateMachine stateMachine, PlayerProperties.PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
        {
        }
    }
}
