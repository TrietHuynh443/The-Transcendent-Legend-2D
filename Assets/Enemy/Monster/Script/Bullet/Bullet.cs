using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Bullet : BulletBase
{

    private Animator _animator;  // Reference to the Animator component
    // private float _destroyDelay = 0.5f;  // Delay before bullet is destroyed after explosion animation

    private BoxCollider2D _bulletCollider;  // Reference to the Bullet's Collider

    private Rigidbody2D _rb;

    // private float _lifetime = 5f;

    [SerializeField] private float _damage = 4f;

    public void Initialize()
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
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

    protected override void OnBecameInvisible()
    {
        // Destroy the object when it goes out of the camera's view
        ObjectPooler.EnqueueObject(this, "Bullet"); // Change Name Here
    }


    protected override void EnqueueBullet()
    {
        ObjectPooler.EnqueueObject(this, "Bullet"); // Change Name Here
    }
}