using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sword : BulletBase
{
    public override void Initialize()
    {
        base.Initialize();
        damage = 10;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);

            // Debug.Log("Hi from Explde!!");
            _rb.velocity = Vector2.zero;

            if (_animator != null)
            {

                // Debug.Log("Explode!!!");
                _animator.Play("BulletExplode", 0   , 0);

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
        ObjectPooler.EnqueueObject(this, "Bullet");
    }


    protected override void EnqueueBullet()
    {
        ObjectPooler.EnqueueObject(this, "Bullet");
    }
}