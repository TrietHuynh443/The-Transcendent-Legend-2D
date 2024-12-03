using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutGameUIHandler : MonoBehaviour, IGameEventListener<PauseGameEvent>
{
    [SerializeField] private GameObject _pauseUI;
    public void Handle(PauseGameEvent @event)
    {
        _pauseUI.SetActive(true);
    }

    // Start is called before the first frame update
    void OnDestroy()
    {
        EventAggregator.Unregister<PauseGameEvent>(this);
    }
    void Awake()
    {

    }
    void Start()
    {
        EventAggregator.Register<PauseGameEvent>(this);
    }

}
