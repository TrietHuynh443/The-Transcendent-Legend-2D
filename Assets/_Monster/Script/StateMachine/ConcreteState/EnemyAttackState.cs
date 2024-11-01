using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState {
    private Transform _playerTransform;
    private float _timer;
    private float _timerBetweenShots = 1.5f;

    private float _exitTimer;
    private float _timeTillExit = 3f;
    private float _distanceToCountExit = 3f;

    private float _bulletSpeed = 10f;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) 
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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

        enemy.Move(Vector2.zero);

        if (_timer > _timerBetweenShots)
        {
            _timer = 0f;

            Vector2 dir = (_playerTransform.position - enemy.transform.position).normalized;

            Rigidbody2D bullet = GameObject.Instantiate(enemy.bulletPrefabs, enemy.transform.position, Quaternion.identity);

            bullet.velocity = dir * _bulletSpeed;
        }

        if (Vector2.Distance(_playerTransform.position, enemy.transform.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeTillExit)
            {
                enemy.enemyStateMachine.ChangeState(enemy.moveState);

                Animator animator = enemy.GetComponent<Animator>();

                animator.Play("Move", 0, 0);
            }
        }
        else
        {
            _exitTimer = 0f;
        }

        _timer += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
