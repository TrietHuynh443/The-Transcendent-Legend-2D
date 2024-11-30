using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using DG.Tweening;
using GameEvent;
using Player.PlayerProperties;
using Player.PlayerStates.MoveState;
using Player.PlayerStates.PlayerStateMachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : BaseEntity, IGameEventListener<DeadEvent>
{
    [SerializeField]
    private float _onJumpGravityScale = 3f;

    [SerializeField]
    private float _originGravityScale = 1f;

    [SerializeField]
    private PlayerProperties _properties;

    [SerializeField]
    private AttackTrigger _normalAttack;

    [SerializeField]
    private PlayerDataSO _playerDataSO;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    #region Player State Controller
    private PlayerStateMachine _playerStateMachine;
    private IdleState _idleState;
    private MoveState _moveState;
    private AttackState _attackState;
    private JumpState _jumpState;
    private InAirState _inAirState;

    private OnGroundedState _onGroundState;
    private float _startInAirTime;

    #endregion Player State Controller
    // private bool _isGrounded = false;
    // private bool _isAttacking = false;
    // private bool _inCoyate = false;

    #region Public Properties
    public Animator Anim => _animator;

    public Rigidbody2D Rigidbody => _rigidbody;
    public IdleState Idle => _idleState;
    public MoveState Move => _moveState;
    public AttackState Attack => _attackState;
    public JumpState Jump => _jumpState;
    public InAirState InAir => _inAirState;

    public OnGroundedState OnGroundState => _onGroundState;
    #endregion Public Properties

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rigidbody.gravityScale = _originGravityScale;

        InitPlayerStates();
        _playerDataSO.Init();
        _normalAttack.AttackDamage = _playerDataSO.CurrentStats.Attack;
        EventAggregator.Register<DeadEvent>(this);
    }

    private void InitPlayerStates()
    {
        _playerStateMachine = new PlayerStateMachine();
        _idleState = new IdleState(this, _playerStateMachine, _properties, "Idle");
        _moveState = new MoveState(this, _playerStateMachine, _properties, "Move");
        _jumpState = new JumpState(this, _playerStateMachine, _properties, "Jump");
        _attackState = new AttackState(
            this,
            _playerStateMachine,
            _properties,
            "Attack",
            _normalAttack.gameObject
        );
        _onGroundState = new OnGroundedState(this, _playerStateMachine, _properties, "Grounded");
        _inAirState = new InAirState(this, _playerStateMachine, _properties, "InAir");
        _playerStateMachine.Initialize(_idleState);
    }

    private void Update()
    {
        _properties.Input.HorizontalInput = Input.GetAxis("Horizontal");
        _properties.Input.IsJumpInput =
            _properties.Status.CurrentJump < _properties.Data.MaxJump
            && Input.GetKeyDown(KeyCode.Space);
        _properties.Input.IsAttackInput = Input.GetMouseButtonDown(0);
        CheckGrounded();
        if (!_properties.Status.IsGrounded && _properties.Status.CurrentJump == 0 && Time.time - _startInAirTime > 0.2f)
        {
            HandleInAir();
            StartCoroutine(StartCoyate());
        }
        else if (_properties.Status.IsGrounded)
        {
            HandleOnGround();
        }

        if (_properties.Status.StuckWall * _properties.Input.HorizontalInput > 0)
        {
            Debug.Log("Stuck Wall");
            _properties.Input.HorizontalInput = 0;
        }
        _playerStateMachine.CurrentState.LogicUpdate();
    }

    private IEnumerator StartCoyate()
    {
        int countFrame = 0;
        _properties.Status.IsInCoyateTime = true;
        while (countFrame++ < 1)
        {
            yield return null;
        }
        _properties.Status.IsInCoyateTime = false;
    }

    public void CheckGrounded()
    {
        RaycastHit2D hitDown = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            1f,
            LayerMask.GetMask("Ground")
        );

        RaycastHit2D hitRight = Physics2D.BoxCast(
            transform.position,
            new Vector2(0.6f, 1f),
            0f,
            transform.right,
            0.5f,
            LayerMask.GetMask("Ground")
        );
        _properties.Status.StuckWall =
            hitRight.collider != null ? (int)_properties.Input.HorizontalInput : 0;
        _properties.Status.IsGrounded = hitDown.collider != null;
        if(_properties.Status.IsGrounded) _startInAirTime = Time.time;
    }

    public void HandleOnGround()
    {
        _rigidbody.gravityScale = _originGravityScale;
    }

    private void EndAnimationTrigger()
    {
        _playerStateMachine.CurrentState.AnimationFinishTrigger();
    }

    void FixedUpdate()
    {
        // var abs = Mathf.Abs(_horizontalInput);
        // if(abs >= 0.001f)
        // {
        //     var newY = _horizontalInput < -0.001f ? 180 : 0;
        //     transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(transform.rotation.x, newY)));
        // }

        // _rigidbody.velocity = new Vector2(_horizontalInput * _speed, _rigidbody.velocity.y + _currentJumpSpeed);
        // _animator?.SetFloat("Move", abs);
        // _currentJumpSpeed = 0;
        _playerStateMachine.CurrentState.PhysicsUpdate();
    }

    public void Flip(float euler)
    {
        transform.SetPositionAndRotation(
            transform.position,
            Quaternion.Euler(new Vector3(transform.rotation.x, euler))
        );
    }

    public void HandleInAir()
    {
        _rigidbody.gravityScale = _onJumpGravityScale;
    }

    public void DoJump()
    {
        if (_properties.Status.CurrentJump >= _properties.Data.MaxJump)
            return;

        ++_properties.Status.CurrentJump;
        HandleInAir();
        var newVelocity = new Vector2(
            _properties.Input.HorizontalInput,
            _properties.Data.JumpForce
        );
        if (_properties.Status.CurrentJump == _properties.Data.MaxJump)
        {
            newVelocity.y += _rigidbody.velocity.y / Mathf.Sqrt(2);
        }
        _rigidbody.velocity = newVelocity;
    }

    public void ResetJump()
    {
        _properties.Status.CurrentJump = 0;
    }

    public override void TakeDamage(float damage)
    {
        _animator.SetTrigger("OnHit");
        //Handle health
        _playerDataSO.LoseHealth(damage);
    }

    public override void Die()
    {
        _animator.SetTrigger("Die");
        EventAggregator.RaiseEvent<DeadEvent>(new DeadEvent());
    }

    public void Handle(DeadEvent @event)
    {
        _animator.SetTrigger("Die");
    }

    void OnDrawGizmos()
    {
        Vector2 boxSize = new Vector2(0.6f, 1f);
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.right * 0.5f, boxSize);
    }

}
