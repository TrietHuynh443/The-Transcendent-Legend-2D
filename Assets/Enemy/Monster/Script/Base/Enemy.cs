using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : BaseEntity, IEnemyMoveable, ITriggerCheckable
{
    [SerializeField] public float MaxHealth { get; set; } = 100f;
    [SerializeField] public bool IsFacingRight { get; set; } = false;

    [SerializeField] public Transform PlayerTransform;
    public float CurrentHealth { get; set; }
    public Rigidbody2D Rigidbody { get; set; }

    public GameObject BulletPrefabs;

    public bool IsAggroed { get; set; }
    public bool IsWithInStrikingDistance { get; set; }

    #region IdleVariable
    [SerializeField] public float MoveRange = 5f;
    [SerializeField] public float MoveSpeed = 1f;
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
        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    #endregion

    #region Move Function
    public void Move(Vector2 velocity)
    {
        Rigidbody.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            Debug.Log("Hi");
            IsFacingRight = !IsFacingRight;
        } else if (!IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            Debug.Log("Ho");
            IsFacingRight = !IsFacingRight;
        }
    }
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
        EnemyStateMachine.CurrentState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        EnemyStateMachine.CurrentState.PhysicsUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;

        Rigidbody = GetComponent<Rigidbody2D>();

        EnemyStateMachine.Initialize(IdleState);
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}