using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster4 : Enemy
{
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _timerBetweenShots = 1.5f;
    [SerializeField] private float _bulletSpeed = 10f;
    private bool _isFacingRight = false;
    private Vector3 _targetPoint;

    //[SerializeField] public bool IsFacingRight { get; set; } = false;

    protected override void Start()
    {
        base.Start();
        _targetPoint = transform.position + new Vector3(-MoveRange, 0, 0);
    }

    public override void IdleStateHandle()
    {
        Vector2 direction;
        if (_isFacingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
        Move(direction * MoveSpeed);

        if ((transform.position - _targetPoint).sqrMagnitude < 0.01f)
        {
            if (_isFacingRight)
            {
                _targetPoint = transform.position + new Vector3(-MoveRange, 0, 0) * 2;
            }
            else
            {
                _targetPoint = transform.position + new Vector3(MoveRange, 0, 0) * 2;
            }
            _isFacingRight = !_isFacingRight;
        }
    }

    public override void MoveStateHandle()
    {
        Vector2 moveDirection = (PlayerTransform.position - transform.position).normalized;

        Move(moveDirection * _chaseSpeed);
    }

    public override void AttackStateHandle(ref float timer)
    {
        Move(Vector2.zero);

        // if ((PlayerTransform.position.x > transform.position.x) && (!_isFacingRight))
        // {
        //     Debug.Log("nhut Thanh");
        //     Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
        //     transform.rotation = Quaternion.Euler(rotator);
        //     _isFacingRight = !_isFacingRight;
        //     Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y);
        // }

        // if ((PlayerTransform.position.x < transform.position.x) && _isFacingRight)
        // {
        //     Debug.Log("Cong Triet");
        //     Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
        //     transform.rotation = Quaternion.Euler(rotator);
        //     _isFacingRight = !_isFacingRight;
        //     Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y);
        // }

        if ((PlayerTransform.position.x > transform.position.x && !_isFacingRight) ||
    (PlayerTransform.position.x < transform.position.x && _isFacingRight))
        {
            _isFacingRight = !_isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            Debug.Log($"Rotation: {transform.eulerAngles}");
        }


        if (timer > _timerBetweenShots)
        {
            timer = 0;

            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Bullet instance = ObjectPooler.DequeueObject<Bullet>("Bullet");
        Vector2 dir = (PlayerTransform.position - transform.position).normalized;
        instance.gameObject.SetActive(true);
        Collider2D collider = instance.gameObject.GetComponent<Collider2D>();
        collider.enabled = true;
        instance.Initialize();
        instance.transform.position = transform.position;
        instance.GetComponent<Rigidbody2D>().velocity = dir * _bulletSpeed;
    }


    public override void DieStateHandle()
    {
        Debug.Log("Monster is dead.");
        // Add death logic here (e.g., animation, destruction)
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
        CheckForLeftOrRightFacing(velocity);
    }
}
