using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float _trapDamage = 5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.TakeDamage(_trapDamage);
            EventAggregator.RaiseEvent<TrapEncounterAchievementProgress>(new TrapEncounterAchievementProgress()
            {
                Count = 1,
            });
        }
    }
}
