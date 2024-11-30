using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField]
    private string _nextSceneName;
    [SerializeField]
    private string _switchName;

    private bool pauseSelfExitTrigger = false;
    public string LevelSwitchName => _switchName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameManager.Instance.IsLevelSwitching())
            {
                pauseSelfExitTrigger = true;
                bool isSwitchingLeftToRight = collision.GetComponent<Rigidbody2D>().velocity.x > 0;
                GameManager.Instance.SwitchScene(_nextSceneName, _switchName, isSwitchingLeftToRight);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pauseSelfExitTrigger)
        {
            pauseSelfExitTrigger = false;
            return;
        }
        
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance.IsLevelSwitching())
            {
                GameManager.Instance.EndLevelSwitch();
            }
        }
    }
    
}
