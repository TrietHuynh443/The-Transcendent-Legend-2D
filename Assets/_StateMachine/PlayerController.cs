using System;
using System.Collections;
using System.Collections.Generic;
using Player.PlayerProperties;
using Player.PlayerStates.MoveState;
using Player.PlayerStates.PlayerStateMachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _onJumpGravityScale = 3f;
    [SerializeField] private float _originGravityScale = 1f;
    [SerializeField] private PlayerProperties _properties;
    [SerializeField] private GameObject _normalAttack;
    

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

    #endregion Player State Controller
    private bool _isGrounded = false;
    private bool _isAttacking = false;

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

    // Start is called before the first frame update
    void Start()
    {   
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _rigidbody.gravityScale = _originGravityScale;

        InitPlayerStates();
    }

    private void InitPlayerStates()
    {
        _playerStateMachine = new PlayerStateMachine();
        _idleState = new IdleState(this, _playerStateMachine, _properties, "Idle");
        _moveState = new MoveState(this, _playerStateMachine, _properties, "Move");
        _jumpState = new JumpState(this, _playerStateMachine, _properties, "Jump");
        _attackState = new AttackState(this, _playerStateMachine, _properties, "Attack", _normalAttack);
        _onGroundState = new OnGroundedState(this, _playerStateMachine, _properties, "Grounded");
        _inAirState = new InAirState(this, _playerStateMachine, _properties, "InAir");
        _playerStateMachine.Initialize(_idleState);
    }

    // Update is called once per frame
    void Update()
    {
        _properties.Input.HorizontalInput = Input.GetAxis("Horizontal");
        _properties.Input.IsJumpInput = _properties.Status.CurrentJump < _properties.Data.MaxJump && Input.GetKeyDown(KeyCode.Space);
        _properties.Input.IsAttackInput = Input.GetMouseButtonDown(0);
        CheckGrounded();
        _playerStateMachine.CurrentState.LogicUpdate();
    }

    public void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        _properties.Status.IsGrounded = hit.collider != null;
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
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(transform.rotation.x, euler)));
    }

    public void HandleInAir()
    {
       _rigidbody.gravityScale = _onJumpGravityScale;
    }

    public void DoJump()
    {
        if(_properties.Status.CurrentJump >= _properties.Data.MaxJump)
            return;

        ++_properties.Status.CurrentJump;
        HandleInAir();
        var newVelocity = new Vector2(
            _properties.Input.HorizontalInput,
            _properties.Data.JumpForce
            );
        if(_properties.Status.CurrentJump == _properties.Data.MaxJump)
        {
            newVelocity.y += _rigidbody.velocity.y / Mathf.Sqrt(2);
        }
        _rigidbody.velocity = newVelocity;
    }

    public void ResetJump()
    {
        _properties.Status.CurrentJump = 0;
    }

}
