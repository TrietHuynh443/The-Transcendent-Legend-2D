using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && _enemy != null)
        {
            _enemy.SetTarget(collision.gameObject);
            _enemy.SetAggroStatus(true);
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && _boss != null)
        {
            _boss.SetAggroStatus(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")  && _enemy != null)
        {
            _enemy.SetAggroStatus(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && _boss != null)
        {
            _boss.SetAggroStatus(false);
        }
    }
}
