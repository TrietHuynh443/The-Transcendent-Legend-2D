using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntity : MonoBehaviour, IDamageable
{
    public abstract void Die();

    public abstract void TakeDamage(float damage);
}
