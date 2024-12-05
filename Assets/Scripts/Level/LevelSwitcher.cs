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
    [SerializeField]
    private bool _isVerticalSwitch = false;
    
    public string LevelSwitchName => _switchName;

    private Fade fade;

    void Start()
    {
        fade = FindObjectOfType<Fade>();
    }

    public IEnumerator ChangeScene(string sceneName, string switchName, Vector2 velocity, bool isVerticalSwitch)
    {
        fade.FadeIn();
        yield return new WaitForSeconds(fade.FadeDuration);
        GameManager.Instance.SwitchScene(sceneName, switchName, velocity, isVerticalSwitch);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            Vector2 velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            
            StartCoroutine(ChangeScene(_nextSceneName, _switchName, velocity, _isVerticalSwitch));
        }
    }
    
}
