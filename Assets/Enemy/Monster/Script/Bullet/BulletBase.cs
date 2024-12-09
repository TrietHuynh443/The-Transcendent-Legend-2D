using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    protected Animator _animator; // Reference to the Animator component
    private float _destroyDelay = 0.5f;  // Delay before bullet is destroyed after explosion animation

    protected BoxCollider2D _bulletCollider;  // Reference to the Bullet's Collider

    protected Rigidbody2D _rb;
    
    protected float damage;

    private float _lifetime = 3f;

    public virtual void Initialize(){
        _bulletCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void OnBecameInvisible();

    protected abstract void OnTriggerEnter2D(Collider2D collision);

    protected abstract void EnqueueBullet();
}
