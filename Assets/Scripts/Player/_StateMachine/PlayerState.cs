using System;
using Player.PlayerProperties;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

public class PlayerState
{
    protected PlayerController _controller;
    protected PlayerStateMachine _stateMachine;
    protected PlayerProperties _properties;

    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    protected float _startTime;

    protected string _animBoolName;

    private Action _onStateChangeAction;
    private bool _hasAnimatorParam;
    protected bool _isAttack;
    protected bool _isJump;

    public PlayerController Controller { get; }
    public PlayerStateMachine StateMachine { get; }
    public string AnimBoolName { get; }

    public PlayerState(PlayerController controller, PlayerStateMachine stateMachine, PlayerProperties properties,string animBoolName)
    {
        _controller = controller;
        _stateMachine = stateMachine;
        _properties = properties;
        _animBoolName = animBoolName;
        // core = player.Core;
        _hasAnimatorParam = AnimatorExtensions.HasParameter(_controller.Anim, _animBoolName);
    }

    public virtual void Enter()
    {
        if(_hasAnimatorParam)
            _controller.Anim.SetBool(_animBoolName, true);
        _startTime = Time.time;
        _isAnimationFinished = false;
        _isExitingState = false;
    }

    public virtual void Exit()
    {
        if(_hasAnimatorParam)
            _controller.Anim.SetBool(_animBoolName, false);
        _isExitingState = true;
    }

    public virtual void LogicUpdate()
    {
        if (_properties.Input.IsJumpInput || _properties.Status.IsInCoyateTime) 
        {
            if(_properties.Input.IsJumpInput)
                _isJump = true;
            _controller.HandleInAir();
        }
        if(_properties.Input.IsAttackInput){
            _isAttack = true;
        }
    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
        var horizontalMove = _properties.Input.HorizontalInput* _properties.Data.Speed;
        _controller.Rigidbody.velocity = new Vector2(horizontalMove, _controller.Rigidbody.velocity.y);
    }

    public virtual void DoChecks() { 
        var horizontalInput = _properties.Input.HorizontalInput;
        if(Mathf.Abs(horizontalInput) >= 0.001f)
        {
            var euler = horizontalInput < -0.001f ? 180 : 0;
            _controller.Flip(euler);
        }
        if(!_isJump && _properties.Status.IsGrounded)
        {
            _controller.ResetJump();
            _controller.HandleOnGround();
        }
    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => _isAnimationFinished = true;

    public void OnStateChange()
    {
        _onStateChangeAction?.Invoke();
    }

    public void AddOnStateChangeEventListener(Action action)
    {
        _onStateChangeAction += action;
    }

    public void RemoveOnStateChangeEventListener(Action action)
    {
        _onStateChangeAction -= action;
    }
    

}