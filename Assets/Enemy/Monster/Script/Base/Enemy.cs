using System;
using System.Collections;
using Factory;
using UnityEngine;

/// <summary>
/// 
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : BaseEntity, IEnemyMoveable, ITriggerCheckable
{
    [SerializeField] public float MaxHealth { get; set; } = 100f;

    //public bool IsFacingRight { get; set; } = false;
    [SerializeField] protected float _coolDown = 1f;
    protected bool _isAttackCoolDown = false;

    [HideInInspector] public Transform PlayerTransform;
    protected GameObject _target;

    public float CurrentHealth { get; set; }
    public Rigidbody2D Rigidbody { get; set; }

    public Animator animator {  get; set; }

    public bool IsAggroed { get; set; }
    public bool IsWithInStrikingDistance { get; set; }

    public bool IsAttackCoolDown => _isAttackCoolDown;

    protected EnemyType type = EnemyType.OnGrounded;
    #region IdleVariable
    public float MoveRange = 5f;
    [SerializeField] protected float MoveSpeed = 1f;
    protected bool _onHit = false;
    protected bool _isStuck = false;
    private bool _isGrounded;

    protected bool _isDead = false;
    public bool IsDead => _isDead;

    #endregion

    #region StateMachineVariable
    public EnemyStateMachine EnemyStateMachine { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyDieState DieState { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyMoveState MoveState { get; set; }

    #endregion

    #region Health / Die Functions
    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        PlayGetHitAnimation();

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void PlayGetHitAnimation()
    {
        if (animator == null || _isDead)
        {
            return;
        }

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (!_onHit)
        {
            animator.Play("OnHit", 0, 0);
            StartCoroutine(ReturnToPreviousState(currentState));
        }
    }
    public void PlayDieAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator not assigned!");
            return;
        }
        animator.SetTrigger("Die");

    }
    private IEnumerator ReturnToPreviousState(AnimatorStateInfo previousState)
    {
        _onHit = true;
     
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        _onHit = false;
      
        animator.Play(previousState.shortNameHash);
    }
    #endregion

    #region Move Function
    public abstract void CheckForLeftOrRightFacing(Vector2 velocity);
    public abstract void Move(Vector2 velocity);    
    #endregion

    #region Abstract Function
    public abstract void IdleStateHandle();
    public abstract void MoveStateHandle();
    public abstract void AttackStateHandle(ref float timer);
    public abstract void DieStateHandle();
    #endregion

    #region Animation Trigger Events

    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        EnemyStateMachine.CurrentState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyMove,
        EnemyAttack,
    }
    #endregion

    #region Distance Check
    public void SetAggroStatus(bool aggroStatus)
    {
        IsAggroed = aggroStatus;
    }

    public void SetStrikingDistanceBool(bool strikingDistanceBool)
    {
        IsWithInStrikingDistance = strikingDistanceBool;
    }
    #endregion

    protected virtual void Update()
    {
        CheckGround();
        if (_onHit || (!_isGrounded && type == EnemyType.OnGrounded))
            return;
        EnemyStateMachine.CurrentState.FrameUpdate();
    }

    private void CheckGround()
    {
        RaycastHit2D hitDown = Physics2D.BoxCast(
            transform.position,
            new Vector2(0.6f, 1f),
            0f,
            -transform.up,
            0.5f,
            LayerMask.GetMask("Ground")
        );
        RaycastHit2D hitRight = Physics2D.Raycast(
            transform.position,
            transform.right,
            1.5f,
            LayerMask.GetMask("Ground")
        );
        _isStuck = hitRight.collider != null;
        _isGrounded = hitDown.collider != null;
        
    }

    protected virtual void FixedUpdate()
    {
        if (_onHit || (!_isGrounded && type == EnemyType.OnGrounded))
            return;
        EnemyStateMachine.CurrentState.PhysicsUpdate();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        EnemyStateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, EnemyStateMachine);
        AttackState = new EnemyAttackState(this, EnemyStateMachine);
        MoveState = new EnemyMoveState(this, EnemyStateMachine);
        DieState = new EnemyDieState(this, EnemyStateMachine);
        animator = GetComponent<Animator>();

        CurrentHealth = MaxHealth;

        Rigidbody = GetComponent<Rigidbody2D>();

        // Debug.Log("Hello from Enemy Start");
        EnemyStateMachine.Initialize(IdleState);
    }

    public override void Die()
    {
    }

    public void TurnBack()
    {
        Rigidbody.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(
                                                                                    transform.rotation.eulerAngles.x,
                                                                                    transform.rotation.eulerAngles.y + 180,
                                                                                    transform.rotation.eulerAngles.z
                                                                                    )
                                                                            ));
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void SetTarget(GameObject gameObject)
    {
        PlayerTransform = gameObject.transform;
        _target = gameObject;
    }
}
