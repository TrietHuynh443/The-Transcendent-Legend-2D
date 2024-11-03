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
    

    private Rigidbody2D _rigidbody;
    private Animator _animator;


    #region Player State Controller
    private PlayerStateMachine _playerStateMachine;
    private IdleState _idleState;
    private MoveState _moveState;
    private AttackState _attackState;
    private JumpState _jumpState;

    private OnGroundedState _onGroundState;

    #endregion Player State Controller
    private bool _isGrounded = false;
    private bool _isAttacking = false;

    #region Public Properties
    public Animator Anim => _animator;

    public Rigidbody2D Rigidbody => _rigidbody;

    public IdleState Idle => _idleState;
    public MoveState MoveState => _moveState;
    public AttackState AttackState => _attackState;
    public JumpState Jump => _jumpState;

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
        _attackState = new AttackState(this, _playerStateMachine, _properties, "Attack");
        _onGroundState = new OnGroundedState(this, _playerStateMachine, _properties, "Grounded");
        _playerStateMachine.Initialize(_idleState);
    }

    // Update is called once per frame
    void Update()
    {
        // _horizontalInput = Input.GetAxis("Horizontal");

        // float velocityY = _rigidbody.velocity.y;

        // CheckGrounded();

        // if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        // {
        //     if(_isGrounded || _animator.GetBool("Jumping") && _jumpCount < _maxJump)
        //     {
        //         _currentJumpSpeed = _jumpSpeed;
        //         _animator.SetBool("Jumping", true);
        //         _jumpCount++;
        //         _isGrounded = false;
        //     }
        // }

        // if (velocityY == 0 && _currentJumpSpeed != _jumpSpeed)
        // {
        //     _animator.SetBool("Jumping", false);
        //     _jumpCount = 0;
        //     _rigidbody.gravityScale = _originGravityScale;
        // }

        // if (!_isAttacking && Input.GetMouseButtonDown(0))
        // {
        //     Attack();
        // }    
        _properties.Input.HorizontalInput = Input.GetAxis("Horizontal");
        _properties.Input.IsJumpInput = Input.GetKeyDown(KeyCode.Space);
        CheckGrounded();
        _playerStateMachine.CurrentState.LogicUpdate();
    }

    public void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        _properties.Status.IsGrounded = hit.collider != null;
        // Debug.Log(hit.collider);
    }

    public void HandleOnGround()
    {
       _rigidbody.gravityScale = _originGravityScale;
    }

    private void Attack()
    {
        _isAttacking = true;
        _animator.SetBool("Attack", true);
        _animator.Play("Attack");
    }
    
    private void EndAttack()
    {
        _isAttacking = false;
        _animator.SetBool("Attack", false);
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
}
