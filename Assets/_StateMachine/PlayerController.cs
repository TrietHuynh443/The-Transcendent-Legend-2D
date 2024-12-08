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
public class PlayerController : BaseEntity, IGameEventListener<PlayerDieEvent>, IGameEventListener<PassCheckpointEvent>, IGameEventListener<OnHitEvent>
{
    [SerializeField]
    private float _onJumpGravityScale = 3f;

    [SerializeField]
    private float _originGravityScale = 1f;

    [SerializeField]
    private PlayerProperties _properties;

    [SerializeField]
    private AttackTrigger _normalAttack;
    private PlayerDataSO _playerDataSO;
    [SerializeField]
    private SceneSaveDataSO _sceneSaveDataSO;
    

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
    private bool _isDead;
    private float _startPressJumpTime;
    private bool _isHit;

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
    private void OnEnable()
    {
        _startInAirTime = Time.time;
        _isDead = false;
        _isHit = false;
    }
    private void Start()
    {
        DontDestroyOnLoad(this);
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerDataSO = ResourcesRoute.Instance.PlayerDataSO;
        _rigidbody.gravityScale = _originGravityScale;

        InitPlayerStates();
        PlacePlayerOnSceneSwitch();
        _playerDataSO.Init();
        _normalAttack.AttackDamage = _playerDataSO.CurrentStats.Attack;
        EventAggregator.Register<PlayerDieEvent>(this);
        EventAggregator.Register<PassCheckpointEvent>(this);
        EventAggregator.Register<OnHitEvent>(this);
    }

    void OnDestroy()
    {
        EventAggregator.Unregister<PlayerDieEvent>(this);
        EventAggregator.Unregister<PassCheckpointEvent>(this);
        EventAggregator.Unregister<OnHitEvent>(this);
    }
    public void Reset()
    {
        PlayerPrefs.SetInt("IsPlayerInit", 0);
        _playerDataSO.Init();
    }

    private void PlacePlayerOnSceneSwitch()
    {
        Vector2 switchVelocity = GameManager.Instance.GetSwitchVelocity();
        string levelSwitchName = GameManager.Instance.GetLevelSwitchName();
        bool isSwitchingLeftToRight = switchVelocity.x > 0;
        bool isSwitchingTopToBottom = switchVelocity.y < 0;
        bool isVerticalSwitch = GameManager.Instance.IsVerticalSwitch();
        GameObject[] sceneSwitchTriggers = GameObject.FindGameObjectsWithTag("Scene Trigger");
        foreach (GameObject sceneSwitchTrigger in sceneSwitchTriggers)
        {
            if (sceneSwitchTrigger.GetComponent<LevelSwitcher>().LevelSwitchName == levelSwitchName)
            {
                Vector2 size = sceneSwitchTrigger.GetComponent<BoxCollider2D>().size;
                Vector3 offset;
                float offsetRate = 2;
                if (!isVerticalSwitch)
                {
                    int offsetDirection = isSwitchingLeftToRight ? 1 : -1;
                    offset = new Vector3(offsetDirection * offsetRate * size.x, 0, 0);
                }
                else
                {
                    int offsetDirection = isSwitchingTopToBottom ? -1 : 1;
                    offset = new Vector3(0, offsetDirection * offsetRate * size.y, 0);
                }
                
                transform.position = sceneSwitchTrigger.transform.position + offset;
                Flip(isSwitchingLeftToRight ? 0 : 180);
                break;
            }
        }
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
        if(PauseEvent.IsPaused || _isDead || _isHit)
            return;

        _properties.Input.HorizontalInput = Input.GetAxis("Horizontal");
        HandleJumpInput();

        _properties.Input.IsAttackInput = Input.GetMouseButtonDown(0);
        CheckGrounded();
        var currentInAirTime = Time.time - _startInAirTime;
        if (!_properties.Status.IsGrounded && _properties.Status.CurrentJump == 0 && currentInAirTime > 0.2f)
        {
            if(currentInAirTime > 4f)
            {
                Die();
                return;
            }

            HandleInAir();
            StartCoroutine(StartCoyate());
        }
        else if (_properties.Status.IsGrounded)
        {
            HandleOnGround();
        }

        if (_properties.Status.StuckWall * _properties.Input.HorizontalInput > 0)
        {
            _properties.Input.HorizontalInput = 0;
        }
        _playerStateMachine.CurrentState.LogicUpdate();
    }

    private void HandleJumpInput()
    {

        _properties.Input.IsJumpInput = _properties.Status.CurrentJump < _properties.Data.MaxJump && Input.GetKeyDown(KeyCode.Space);
            
    }

    private IEnumerator StartCoyate()
    {
        int countFrame = 0;
        _properties.Status.IsInCoyateTime = true;
        while (countFrame++ < 4)
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
            newVelocity.y /= Mathf.Sqrt(2);
            _rigidbody.gravityScale *= Mathf.Sqrt(2);
            _rigidbody.AddForce(newVelocity, ForceMode2D.Impulse);
        }
        else
            _rigidbody.velocity = newVelocity;
    }

    public void ResetJump()
    {
        _properties.Status.CurrentJump = 0;
    }

    public override void TakeDamage(float damage)
    {
         _animator.Play("OnHit");
        //Handle health
        _playerDataSO.LoseHealth(damage);
        EventAggregator.RaiseEvent<OnHitEvent>(new OnHitEvent());
    }

    public override void Die()
    {
        EventAggregator.RaiseEvent<PlayerDieEvent>(new PlayerDieEvent());
    }

    public void Handle(PlayerDieEvent @event)
    {
        _animator.SetTrigger("Die");
        _isDead = true;
        StartCoroutine(OnDead());
    }

    public IEnumerator OnDead()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        Reset();
    }

    void OnDrawGizmos()
    {
        Vector2 boxSize = new Vector2(0.6f, 1f);
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.right * 0.5f, boxSize);
    }

    public void Handle(PassCheckpointEvent @event)
    {
        _sceneSaveDataSO.SceneName = @event.SceneName;
        _sceneSaveDataSO.CheckPointPos = @event.CheckPointPosition;
    }

    public void Handle(OnHitEvent @event)
    {
        StartCoroutine(GetHit());
    }

    private IEnumerator GetHit()
    {
        _isHit = true;
        yield return new WaitForSeconds(0.15f);
        _isHit = false;
    }
}
