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
        //Debug.Log(animBoolName);
        _isAnimationFinished = false;
        _isExitingState = false;
        Debug.Log(_stateMachine.CurrentState);

    }

    public virtual void Exit()
    {
        if(_hasAnimatorParam)
            _controller.Anim.SetBool(_animBoolName, false);
        _isExitingState = true;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { 
        var horizontalInput = _properties.Input.HorizontalInput;
        if(Mathf.Abs(horizontalInput) >= 0.001f)
        {
            var euler = horizontalInput < -0.001f ? 180 : 0;
            _controller.Flip(euler);
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