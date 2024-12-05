using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InEnemyAttackAreaCheck : MonoBehaviour
{

    private Enemy _enemyController;

    private void Start()
    {
        _enemyController = GetComponentInParent<Enemy>();
    }
    private void Smell()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            transform.right,
            0.5f,
            LayerMask.GetMask("Player")
            );
            
        _enemyController.IsWithInStrikingDistance = hit.collider != null;

    }
    private void Update(){
        Smell();
    }
}
