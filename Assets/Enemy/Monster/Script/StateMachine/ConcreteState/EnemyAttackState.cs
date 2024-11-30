using UnityEngine;

public class EnemyAttackState : EnemyState {
    // private UnityEngine.Transform _playerTransform;
    private float _timer = 0;
    [SerializeField] public bool IsFacingRight { get; set; } = false;

    private float _exitTimer;
    private float _timeTillExit = 3f;
    private float _distanceToCountExit = 3f;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine EnemyStateMachine) : base(enemy, EnemyStateMachine) 
    {
        // _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // if (Vector2.Distance(enemy.PlayerTransform.position, enemy.transform.position) > _distanceToCountExit)
        // {
        //     _exitTimer += Time.deltaTime;

        //     if (_exitTimer > _timeTillExit)
        //     {
        //         enemy.EnemyStateMachine.ChangeState(enemy.MoveState);

        //         Animator animator = enemy.GetComponent<Animator>();

        //         _timer = 0;

        //         animator.Play("Move", 0, 0);
        //     }
        // }
        // else
        // {
        //     _exitTimer = 0f;
        // }

        // _timer += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        enemy.AttackStateHandle(ref _timer);

        base.PhysicsUpdate();
    }
}
