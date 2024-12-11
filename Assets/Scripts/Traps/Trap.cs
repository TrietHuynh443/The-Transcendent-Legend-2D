using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float _trapDamage = 5f;

    private bool buffer = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (buffer)
            {
                buffer = false;
                return;
            }
            PlayerController player = collision.GetComponent<PlayerController>();
            player.TakeDamage(_trapDamage);
            EventAggregator.RaiseEvent<TrapEncounterAchievementProgress>(new TrapEncounterAchievementProgress()
            {
                Count = 1,
            });
            buffer = true;
        }
    }
}
