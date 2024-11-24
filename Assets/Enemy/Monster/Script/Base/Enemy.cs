using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : BaseEntity, IEnemyMoveable, ITriggerCheckable
{
    [SerializeField] public float MaxHealth { get; set; } = 100f;

    //public bool IsFacingRight { get; set; } = false;

    [SerializeField] public Transform PlayerTransform;
    public float CurrentHealth { get; set; }
    public Rigidbody2D Rigidbody { get; set; }

    public GameObject BulletPrefabs;

    public Animator animator {  get; set; }

    public bool IsAggroed { get; set; }
    public bool IsWithInStrikingDistance { get; set; }

    #region IdleVariable
    [SerializeField] public float MoveRange = 5f;
    [SerializeField] public float MoveSpeed = 1f;
    private bool _onHit = false;
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
        Debug.Log(gameObject.name + " " + CurrentHealth);

        PlayGetHitAnimation();

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void PlayGetHitAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator not assigned!");
            return;
        }

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (!_onHit)
        {
            animator.Play("GetHit");
            StartCoroutine(ReturnToPreviousState(currentState));
        }
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

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
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

    private void Awake()
    {
        EnemyStateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, EnemyStateMachine);
        AttackState = new EnemyAttackState(this, EnemyStateMachine);
        MoveState = new EnemyMoveState(this, EnemyStateMachine);
        DieState = new EnemyDieState(this, EnemyStateMachine);
    }

    private void Update()
    {
        if (_onHit)
            return;
        EnemyStateMachine.CurrentState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        if (_onHit)
            return;
        EnemyStateMachine.CurrentState.PhysicsUpdate();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        CurrentHealth = MaxHealth;

        Rigidbody = GetComponent<Rigidbody2D>();

        EnemyStateMachine.Initialize(IdleState);
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}
