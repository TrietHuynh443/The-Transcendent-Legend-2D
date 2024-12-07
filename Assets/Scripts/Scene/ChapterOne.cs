using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterOne : BaseScene, IGameEventListener<StartGameEvent>, IGameEventListener<RestartGameEvent>
{
    [SerializeField] private GameObject _spawnElement;
    public void Handle(StartGameEvent @event)
    {
        if(@event.Level != 1) return;
        _spawnElement.SetActive(true);
    }

    private void OnEnable()
    {
        EventAggregator.Register<StartGameEvent>(this);
        EventAggregator.Register<RestartGameEvent>(this);
    }

    private void OnDisable()
    {
        EventAggregator.Unregister<StartGameEvent>(this);
        EventAggregator.Unregister<RestartGameEvent>(this);
    }

    public void Handle(RestartGameEvent @event)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
