using System.Collections;
using DG.Tweening;
using Factory;
using UnityEngine;
using UnityEngine.UI;

public class Monster4 : Enemy
{
    private float _chaseSpeed = 3f;
    private bool _isFacingRight = false;
    private bool _needTurnBack = false;
    private Vector3 _targetPoint;

    private float _timerBetweenShots = 1.5f;

    private float _bulletSpeed = 10f;

    // private GameObject _target;
    private Vector3 _originPostion;
    private float _startAttackTime;

    private float _exitTimer;
    private float _timeTillExit = 3f;
    private float _distanceToCountExit = 3f;
    [SerializeField] private float _health = 5f;
    [SerializeField] private Image _healthBar;
    private bool _isFirstAttackTime = true;

    protected override void Start()
    {
        base.Start();
        _originPostion = transform.position;
        _targetPoint = transform.position + new Vector3(MoveRange * transform.right.x, 0, 0);
        type = EnemyType.Flying;
        _needTurnBack = false;
        animator.Play("Idle", 0, 0);
        _startAttackTime = Time.time;
    }

    public override void IdleStateHandle()
    {
        if(Vector2.Distance(transform.position, _originPostion) >= MoveRange || _isStuck)
        {
            _needTurnBack = true;
            _originPostion = transform.position;
        }
        Move(transform.right * MoveSpeed);
    }

    public override void MoveStateHandle()
    {
        if(_target == null) return;
        Vector2 moveDirection = (_target.transform.position - transform.position).normalized;
        var targetPoint = _target.transform.position;

        targetPoint.y = transform.position.y;
        targetPoint.z = 0;
        Vector2 direction = (targetPoint - transform.position).normalized;
        // Calculate the angle in degrees
        float angle = Vector2.Angle(transform.right, direction);
        if (Mathf.Abs(angle) > 0) // Check absolute value for both left and right cases
        {
            _needTurnBack = true;
        }
        Move(moveDirection * _chaseSpeed);
    }

    public override void AttackStateHandle(ref float timer)
    {
        if(_target == null) return;

        Move(Vector2.zero);
        var targetPoint = _target.transform.position;
        targetPoint.y = transform.position.y;
        targetPoint.z = 0;
        Vector2 direction = (targetPoint - transform.position).normalized;
        float angle = Vector2.Angle(transform.right, direction);
        if (Mathf.Abs(angle) > 0) // Check absolute value for both left and right cases
        {
            _needTurnBack = true;
        }
        if(_isFirstAttackTime)
        {
            _isFirstAttackTime = false;
            _startAttackTime = Time.time;
        }
        else if ((Time.time - _startAttackTime) > _coolDown)
        {
            animator.Play("Attack");
            _startAttackTime = Time.time;
            //Handle in animation
        }

        if (Vector2.Distance(_target.transform.position, transform.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeTillExit)
            {
                EnemyStateMachine.ChangeState(MoveState);

                Animator animator = GetComponent<Animator>();

                animator.Play("Move", 0, 0);
                _isFirstAttackTime = true;
            }
        }
        else
        {
            _exitTimer = 0f;
        }
    }

    public override void DieStateHandle()
    {
        // throw new System.NotImplementedException();
    }

    public override void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        //Debug.Log(velocity.x);
        //Debug.Log(_isFacingRight);

        if (_isFacingRight && velocity.x > 0f)
        {
            Debug.Log("Here");
            Vector3 rotator = new Vector3(this.transform.rotation.x, 180f, this.transform.rotation.z);
            this.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotator));
            Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y);
        }

        else if (!_isFacingRight && velocity.x < 0f)
        {
            Debug.Log("Hi");
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y);
        }
    }
    public override void Move(Vector2 velocity)
    {
        Rigidbody.velocity = velocity;
    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(_needTurnBack || _isStuck)
        {
            _needTurnBack = false;
            // animator.enabled = false;
            TurnBack();
            _healthBar.fillOrigin = (_healthBar.fillOrigin + 1) % 2;
            StartCoroutine(TurnBackBlockAnimation());

        }

    }

    public override void TakeDamage(float damage)
    {
        _healthBar.DOFillAmount(Mathf.Max(0, (_health - damage)/_health), 0.1f);
        _health -= damage;

        if (_health <= 0f)
        {
            _isDead = true;
            Rigidbody.gravityScale = 4.5f;
        }
        else
        {
            PlayGetHitAnimation();
        }
    }

    private IEnumerator TurnBackBlockAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        // animator.enabled = true;
    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if(_isDead || _onHit) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && EnemyStateMachine.CurrentState != IdleState)
        {
            Debug.Log("stuck ground");
            EnemyStateMachine.ChangeState(IdleState);
            animator.Play("Idle", 0, 0);
        }
    }
}
