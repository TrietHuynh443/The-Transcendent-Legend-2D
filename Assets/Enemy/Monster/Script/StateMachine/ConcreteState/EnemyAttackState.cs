using UnityEngine;

public class EnemyAttackState : EnemyState {
    // private UnityEngine.Transform _playerTransform;
    private float _timer;
    [SerializeField] public bool IsFacingRight { get; set; } = false;

    private float _bulletSpeed = 10f;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine EnemyStateMachine) : base(enemy, EnemyStateMachine) 
    {
        // _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        SoundManager.Instance.PlayMonster4GrowlSFX();

        Bullet instance = ObjectPooler.DequeueObject<Bullet>("Bullet");
        Vector2 dir = (enemy.PlayerTransform.position - enemy.gameObject.transform.position).normalized;
        instance.gameObject.SetActive(true);
        Collider2D collider = instance.gameObject.GetComponent<Collider2D>();
        // collider.enabled = false;
        collider.enabled = true;
        instance.Initialize();
        instance.transform.position = enemy.gameObject.transform.position;
        instance.GetComponent<Rigidbody2D>().velocity = dir * _bulletSpeed;
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
