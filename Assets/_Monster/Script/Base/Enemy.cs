using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float maxHealth { get; set; } = 100f;
    [SerializeField] public bool isFacingRight { get; set; } = false;
    public float currentHealth { get; set; }
    public Rigidbody2D RB { get; set; }

    public Rigidbody2D bulletPrefabs;

    public bool isAggroed { get; set; }
    public bool isWithInStrikingDistance { get; set; }

    #region IdleVariable
    [SerializeField] public float moveRange = 5f;
    [SerializeField] public float moveSpeed = 1f;
    #endregion

    #region StateMachineVariable
    public EnemyStateMachine enemyStateMachine { get; set; }
    public EnemyAttackState attackState { get; set; }
    public EnemyDieState dieState { get; set; }
    public EnemyIdleState idleState { get; set; }
    public EnemyMoveState moveState { get; set; }
    #endregion

    #region Health / Die Functions
    public void Damage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Move Function
    public void Move(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (isFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            Debug.Log("Hi");
            isFacingRight = !isFacingRight;
        } else if (!isFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            Debug.Log("Ho");
            isFacingRight = !isFacingRight;
        }
    }
    #endregion

    #region Animation Trigger Events

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        enemyStateMachine.currentState.AnimationTriggerEvent(triggerType);
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
        isAggroed = aggroStatus;
    }

    public void SetStrikingDistanceBool(bool strikingDistanceBool)
    {
        isWithInStrikingDistance = strikingDistanceBool;
    }
    #endregion

    private void Awake()
    {
        enemyStateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, enemyStateMachine);
        attackState = new EnemyAttackState(this, enemyStateMachine);
        moveState = new EnemyMoveState(this, enemyStateMachine);
        dieState = new EnemyDieState(this, enemyStateMachine);
    }

    private void Update()
    {
        enemyStateMachine.currentState.FrameUpdate();
    }
    private void FixedUpdate()
    {
        enemyStateMachine.currentState.PhysicsUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        RB = GetComponent<Rigidbody2D>();

        enemyStateMachine.Initialize(idleState);
    }
}
