using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Bullet : BulletBase
{
    [SerializeField] private float _damage = 4f;

    public override void Initialize()
    {
        _bulletCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnBecameInvisible()
    {
        // Destroy the object when it goes out of the camera's view
        ObjectPooler.EnqueueObject(this, "Bullet");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Debug.Log("Hi from Explde!!");
            _rb.velocity = Vector2.zero;
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(_damage);
            if (_animator != null)
            {
                // Debug.Log("Explode!!!");
                _animator.Play("BulletExplode", 0, 0);
                EventAggregator.RaiseEvent<ExplodeSoundRaiseEvent>(new ExplodeSoundRaiseEvent());
                float time = _animator.GetCurrentAnimatorStateInfo(0).length;

                Invoke(nameof(EnqueueBullet), time);
            }
            else
            {
                EnqueueBullet();
            }

            // Disable the collider to prevent further collisions
            if (_bulletCollider != null)
            {
                _bulletCollider.enabled = false;
            }

        }
    }

    protected override void EnqueueBullet()
    {
        ObjectPooler.EnqueueObject(this, "Bullet");
    }
}