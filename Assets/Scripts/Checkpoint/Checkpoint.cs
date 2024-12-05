using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _playerObject)
        {
            // GameManager.Instance.SetPlayerRespawnLocation(new Vector3(transform.position.x, this.transform.position.y + 0.5f));
            EventAggregator.RaiseEvent<PassCheckpointEvent>(
                new PassCheckpointEvent()
                {
                    SceneName = SceneManager.GetActiveScene().name,
                    CheckPointPosition = new Vector3(transform.position.x, transform.position.y + 0.5f)
                }
                );
        }
    }
}
