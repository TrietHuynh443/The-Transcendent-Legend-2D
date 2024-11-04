using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private Animator _animator;  // Reference to the Animator component
    private float _destroyDelay = 0.5f;  // Delay before bullet is destroyed after explosion animation

    private BoxCollider2D _bulletCollider;  // Reference to the Bullet's Collider

    private Rigidbody2D _rb;

    private float _lifetime = 5f;

    void Start()
    {
        _bulletCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(_lifetime);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform != transform.parent)
        {
            _rb.velocity = Vector2.zero;

            if (_animator != null)
            {
                _animator.Play("BulletExplode", 0, 0);
            }

            // Disable the collider to prevent further collisions
            if (_bulletCollider != null)
            {
                _bulletCollider.enabled = false;
            }

            // Destroy the bullet after the delay
            Destroy(gameObject, _destroyDelay);
        }
    }
}