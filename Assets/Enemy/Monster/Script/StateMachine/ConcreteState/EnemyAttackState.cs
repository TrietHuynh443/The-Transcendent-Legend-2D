using UnityEngine;

public class EnemyAttackState : EnemyState {
    // private UnityEngine.Transform _playerTransform;
    private float _timer;
    [SerializeField] public bool IsFacingRight { get; set; } = false;


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

        _timer += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {

        base.PhysicsUpdate();
        enemy.AttackStateHandle(ref _timer);

    }
}
