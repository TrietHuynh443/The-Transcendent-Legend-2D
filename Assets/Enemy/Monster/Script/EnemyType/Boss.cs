using System;
using System.Collections;
using System.Collections.Generic;
using GameEvent;

// using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : BaseEntity, IEnemyMoveable, ITriggerCheckable, IGameEventListener<PlayerDieEvent>
{
    [SerializeField] public float MaxHealth { get; set; } = 200f;

    LayerMask _playerLayer;
    public Transform PlayerTransform;
    // [SerializeField] private float _chaseSpeed = 3f;
    // [SerializeField] private float _bulletSpeed = 10f;
    // [SerializeField] private GameObject _bulletPrefab;
    // [SerializeField] private int _attack1CountBeforeDefence = 2;
    // [SerializeField] private float _timerBetweenAttack = 1.5f;
    // private float _timer = 0f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D Rigidbody { get; set; }
    public Animator Animator { get; set; }
    public bool IsAggroed { get; set; }
    public bool IsWithInStrikingDistance { get; set; }

    private bool _isFacingRight = true;

    private bool _isFlipped = false;

    private bool _isIntroPlayed = false;
    private int _attackCounter = 0;

    public void Move(Vector2 velocity)
    {
        Rigidbody.velocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (_isFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(this.transform.rotation.x, 0f, this.transform.rotation.z);
            this.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotator));
        }
        else if (!_isFacingRight && velocity.x < 0f)
        {
            Debug.Log("Hi");
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }

    public void LookAtPLayer(){
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > PlayerTransform.position.x && _isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            _isFlipped = false;
        }
        else if (transform.position.x < PlayerTransform.position.x && !_isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            _isFlipped = true;
        }
    }

    public void SetAggroStatus(bool aggroStatus)
    {
        Debug.Log("Boss is aggroed.");
        IsAggroed = aggroStatus;
        Debug.Log(IsAggroed);
    }

    public void SetStrikingDistanceBool(bool strikingDistanceBool)
    {
        Debug.Log("Boss is within striking distance.");
        IsWithInStrikingDistance = strikingDistanceBool;
    }

    public override void Die()
    {
        Move(Vector2.zero);
        Animator.Play("Die");
        StartCoroutine(DestroyObjectAfterDie());
    }
    
    // Destroy object after animation Die
    private IEnumerator DestroyObjectAfterDie()
    {
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length + 2f);
        Destroy(gameObject);
        EventAggregator.RaiseEvent<QuitToMenuEvent>(new QuitToMenuEvent()
        {
            
        });
    }

    public override void TakeDamage(float damage)
    {   
        Debug.Log("Current Health: " + CurrentHealth);
        Animator.Play("OnHit");
        CurrentHealth -= damage;
        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CurrentHealth = MaxHealth;
        _playerLayer = LayerMask.NameToLayer("Player");
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == _playerLayer)
            {
                Debug.Log("Player found");
                PlayerTransform = obj.transform;
            }
        }

    }

    void OnEnable()
    {
        SoundManager.Instance.PlayBossGrowlSFX();
    }

    void Update()
    {
        // Get mouse click event
        // if (Input.GetMouseButtonDown(0))
        // {
        //     TakeDamage(10f);
        // }
    }

    public void Handle(PlayerDieEvent @event)
    {
        Animator.Play("Die");
    }
    

    // void IdleStateHandle()
    // {
    //     if (!_isIntroPlayed)
    //     {
    //         Debug.Log("Hello from Boss Idle");
    //         Animator.Play("Intro");
    //         StartCoroutine(PlayIdleAfterIntro());
    //         _isIntroPlayed = true;
    //     }
    //     else
    //     {
    //         Animator.Play("Idle");
    //     }
    // }

    // void MoveStateHandle()
    // {
    //     if (!_isIntroPlayed)
    //     {
    //         Animator.Play("Intro");
    //         StartCoroutine(PlayWalkAfterIntro());
    //         _isIntroPlayed = true;
    //     }
    //     else
    //     {
    //         Animator.Play("Walk");
    //         Vector2 moveDirection = (PlayerTransform.position - transform.position).normalized;
    //         Move(moveDirection * _chaseSpeed);
    //     }
    // }

    // void BossLogic()
    // {
    //     if (IsAggroed && !IsWithInStrikingDistance)
    //     {
    //         MoveStateHandle();
    //     }
    //     else if (IsWithInStrikingDistance)
    //     {
    //         AttackStateHandle();
    //     }
    //     // else
    //     // {
    //     //     Animator.Play("Idle");
    //     // }
    // }

    // private void AttackStateHandle()
    // {
    //     Move(Vector2.zero);

    //     if (_timer > _timerBetweenAttack)
    //     {
    //         _timer = 0;
    //         Attack();
    //     }
    //     else
    //     {
    //         _timer += Time.deltaTime;
    //     }
    // }

    // private void Attack()
    // {
    //     if (_attackCounter < _attack1CountBeforeDefence)
    //     {
    //         Animator.Play("Attack1");
    //         _attackCounter++;
    //     }
    //     else
    //     {
    //         Animator.Play("Defence");
    //         _attackCounter = 0;
    //         StartCoroutine(LaunchBulletsAfterDefence());
    //     }
    // }

    // private IEnumerator PlayIdleAfterIntro()
    // {
    //     yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
    //     Animator.Play("Idle");
    // }

    // private IEnumerator PlayWalkAfterIntro()
    // {
    //     yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
    //     Animator.Play("Walk");
    // }

    // private IEnumerator LaunchBulletsAfterDefence()
    // {
    //     yield return new WaitForSeconds(1f);

    //     float angleStep = 360f / 10;
    //     float angle = 0f;

    //     for (int i = 0; i < 10; i++)
    //     {
    //         float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
    //         float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);
    //         Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0);
    //         Vector2 bulletDir = (bulletMoveVector - transform.position).normalized;
    //         Bullet bullet = ObjectPooler.DequeueObject<Bullet>("Bullet");
    //         bullet.gameObject.SetActive(true);
    //         Collider2D collider = bullet.gameObject.GetComponent<Collider2D>();
    //         collider.enabled = true;
    //         bullet.Initialize();
    //         bullet.transform.position = transform.position;
    //         bullet.GetComponent<Rigidbody2D>().velocity = bulletDir * _bulletSpeed;

    //         angle += angleStep;
    //     }
    // }

    // void Update()
    // {
    //     BossLogic();
    // }

    // public override void DieStateHandle()
    // {
    //     _animator.Play("TakeHit");
    //     StartCoroutine(DieAfterAnimation());
    // }

    // private IEnumerator DieAfterAnimation()
    // {
    //     yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
    //     // Add your death logic here (e.g., removing the boss, game over screen, etc.)
    //     Destroy(gameObject);
    // }

    // public override void CheckForLeftOrRightFacing(Vector2 velocity)
    // {
    //     //Debug.Log(velocity.x);
    //     //Debug.Log(_isFacingRight);

    //     if (_isFacingRight && velocity.x > 0f)
    //     {
    //         Debug.Log("Here");
    //         Vector3 rotator = new Vector3(this.transform.rotation.x, 180f, this.transform.rotation.z);
    //         this.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(rotator));
    //         Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y);
    //     }

    //     else if (!_isFacingRight && velocity.x < 0f)
    //     {
    //         Debug.Log("Hi");
    //         Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
    //         transform.rotation = Quaternion.Euler(rotator);
    //         Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y);
    //     }
    // }

    // public override void Move(Vector2 velocity)
    // {
    //     Rigidbody.velocity = velocity;
    // }
}
