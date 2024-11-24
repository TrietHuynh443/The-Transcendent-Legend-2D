using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage (float damage);

    void Die();

    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
}
