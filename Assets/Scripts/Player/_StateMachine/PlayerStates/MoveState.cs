using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerStates.MoveState{
    public class MoveState : OnGroundedState
    {
        public MoveState(PlayerController controller, PlayerStateMachine.PlayerStateMachine stateMachine, PlayerProperties.PlayerProperties properties, string animBoolName) : base(controller, stateMachine, properties, animBoolName)
        {
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if(Mathf.Abs(_properties.Input.HorizontalInput) >= 0.01f)
            {
                var newY = _properties.Input.HorizontalInput < -0.001f ? 180 : 0;
                _controller.transform.SetPositionAndRotation(
                    _controller.transform.position,
                     Quaternion.Euler(new Vector3(_controller.transform.rotation.x, newY))
                     );
            }
            else{
                _stateMachine.ChangeState(_controller.Idle);
            }
            var originVelocity = _controller.Rigidbody.velocity;

            _controller.Rigidbody.velocity = new Vector2(
                _properties.Input.HorizontalInput* _properties.Data.Speed,
                 originVelocity.y
                 );
        }



    }
}
