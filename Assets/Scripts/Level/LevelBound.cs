using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class LevelBound : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            GameManager.Instance.RespawnPlayer(GameManager.Instance.PlayerQuickRespawnData, player);
            EventAggregator.RaiseEvent<OnHitEvent>(
                new OnHitEvent()
                {
                }
            );
            player.TakeDamage(10);
        }
    }
}