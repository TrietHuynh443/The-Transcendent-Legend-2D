using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
{
    private Enemy _enemy;
    private Boss _boss;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
        _boss = GetComponentInParent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((LayerMask.LayerToName(collision.gameObject.layer) == "Player") && _boss != null)
        {
            _boss.SetStrikingDistanceBool(true);
        }

        if ((LayerMask.LayerToName(collision.gameObject.layer) == "Player") && _enemy != null)
        {
            _enemy.SetStrikingDistanceBool(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((LayerMask.LayerToName(collision.gameObject.layer) == "Player") && _boss != null)
        {
            _boss.SetStrikingDistanceBool(false);
        }
        if ((LayerMask.LayerToName(collision.gameObject.layer) == "Player") && _enemy != null)
        {
            _enemy.SetStrikingDistanceBool(false);
        }
    }
}
