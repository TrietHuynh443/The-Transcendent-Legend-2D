using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Bullet : BulletBase
{
protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            // Debug.Log("Hi from Explde!!");
            _rb.velocity = Vector2.zero;

            if (_animator != null)
            {

                // Debug.Log("Explode!!!");
                _animator.Play("BulletExplode", 0, 0);

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