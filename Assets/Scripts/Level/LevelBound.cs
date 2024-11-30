using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBound : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.RespawnPlayer(collision.gameObject);
        }
    }
}
