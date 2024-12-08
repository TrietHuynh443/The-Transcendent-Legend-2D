using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterOne : BaseScene, IGameEventListener<RestartGameEvent>
{
    [SerializeField] private GameObject _spawnElement;

    private void OnEnable()
    {
        EventAggregator.Register<RestartGameEvent>(this);
        _spawnElement.SetActive(true);
    }

    private void OnDisable()
    {
        EventAggregator.Unregister<RestartGameEvent>(this);
    }

    public void Handle(RestartGameEvent @event)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
