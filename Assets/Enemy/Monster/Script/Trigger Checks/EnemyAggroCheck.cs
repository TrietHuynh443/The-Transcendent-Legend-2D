using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    [SerializeField] public GameObject PlayerTarget;
    private ITriggerCheckable _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<ITriggerCheckable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Aggro Check");
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            _enemy.SetAggroStatus(true);
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Aggro Check Exit");  
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            _enemy.SetAggroStatus(false);
        }
    }
}
