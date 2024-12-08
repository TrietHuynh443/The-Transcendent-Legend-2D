using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _playerObject)
        {
            //Implement it again
        }
    }
}
