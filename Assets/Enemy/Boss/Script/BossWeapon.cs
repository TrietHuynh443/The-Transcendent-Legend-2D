using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BossWeapon : MonoBehaviour
{
    public float AttackDamage = 5f;
    public Vector3 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public float knockbackForce = 5f; // Add a public variable for knockback force

    public void Attack()
    {
        // Calculate the attack position
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        // Check for collision with the player
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            var player = colInfo.GetComponent<PlayerController>();
            if (player != null)
            {
                // Deal damage to the player
                
                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, transform.right, 4f, LayerMask.GetMask("Ground"));
                if(hit.collider == null)
                {
                    player.transform.DOMoveX(player.transform.position.x+transform.right.x*knockbackForce, 0.25f);
                }
                
                player.TakeDamage(AttackDamage);

                // Apply knockback force
                
            }
        }
        
    }


    
  
}
