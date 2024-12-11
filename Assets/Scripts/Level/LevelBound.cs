using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class LevelBound : MonoBehaviour
{
    [SerializeField] private float _voidDamage = 10f;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            GameManager.Instance.RespawnPlayer(GameManager.Instance.PlayerQuickRespawnData, player);
            if (player.gameObject.activeSelf)
            {
                player.TakeDamage(_voidDamage);
            }
            
        }
    }
}