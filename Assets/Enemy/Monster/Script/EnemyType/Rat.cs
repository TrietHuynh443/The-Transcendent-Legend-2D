using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Factory;
using UnityEngine;
using UnityEngine.UI;

public class Rat : Enemy
{
    private Vector2 _startPos;
    private bool _isTurnBack = false;

    [SerializeField] private GameObject _attackTrigger;
    [SerializeField] private float _health = 5f;
    [SerializeField] private Image _healthBar;

    void Awake()
    {
        type = EnemyType.OnGrounded;
        CurrentHealth = 5f;
    }
    public override void AttackStateHandle(ref float timer)
    {
        if(!_isAttackCoolDown)
        {
            _attackTrigger.SetActive(true);
            _isAttackCoolDown = true;
        }


    }
    public override void TakeDamage(float damage)
    {
        _healthBar.DOFillAmount(Mathf.Max(0, (_health - damage)/_health), 0.2f);
        _health -= damage;
        if (_health <= 0f)
        {
            StartCoroutine(MakeSmoothDie());
        }
        else
        {
            PlayGetHitAnimation();
            
        }
    }

    private IEnumerator MakeSmoothDie()
    {
        yield return new WaitForSeconds(0.5f);
        _isDead = true;
    }

    public void OnAnimationAttackTrigger()
    {
        StartCoroutine(TriggerEndAttack());
    }


    private IEnumerator TriggerEndAttack()
    {
        animator.Play("Idle", 0, 0);
        EnemyStateMachine.ChangeState(IdleState);

        yield return new WaitForSeconds(_coolDown);
        _isAttackCoolDown = false;
    }

    public override void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        // throw new System.NotImplementedException();
    }

    public override void IdleStateHandle()
    {

        IsAggroed = !_isAttackCoolDown;
    }

    protected override void Start()
    {
        base.Start();
        _startPos = transform.position;
        _attackTrigger.GetComponent<AttackTrigger>().AttackDamage = 1;
    }

    protected override void Update()
    {
        base.Update();
        if(Vector2.Distance(transform.position, _startPos) >= MoveRange || _isStuck)
        {
            _isTurnBack = true;
            _startPos = transform.position;
        }
    }



    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(_isTurnBack)
        {
            _isTurnBack = false;
            TurnBack();
            _healthBar.fillOrigin = (_healthBar.fillOrigin + 1) % 2;

        }

    }

    public override void Move(Vector2 velocity)
    {
        Rigidbody.velocity = velocity * transform.right;
    }

    public override void MoveStateHandle()
    {
        Move(new Vector2(MoveSpeed, 0));
    }

    public override void DieStateHandle()
    {
    }
}
