using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class InGameManager : UnitySingleton<InGameManager>,
                             IGameEventListener<DeadEvent>,
                             IGameEventListener<PauseEvent>
                             
{
    [SerializeField] private GameObject _gameOverUIPrefab;
    [SerializeField] private GameObject _gamePauseUIPrefab;
    [SerializeField] private GameObject _outGameUIParentPrefab;
    [SerializeField] private GameObject _mainMenuGameUIPrefab;

    private GameObject _gameOverUI;
    private GameObject _gamePauseUI;
    private GameObject _outGameUIParent;
    private GameObject _mainMenuUI;
    public void Handle(DeadEvent @event)
    {
        if(_gameOverUI == null)
        {
            _gameOverUI = Instantiate(_gameOverUIPrefab,  GetOutGameUIHolder().transform);
            DontDestroyOnLoad(_gameOverUI);
        }
        _gameOverUI.SetActive(true);
    }

    public void Handle(PauseEvent @event)
    {
        if(_gamePauseUI == null)
        {
            _gamePauseUI = Instantiate(_gamePauseUIPrefab, GetOutGameUIHolder().transform);
            DontDestroyOnLoad(_gamePauseUI);
        }
        @event.Pause();
        _gamePauseUI.SetActive(PauseEvent.IsPaused);
    }

    protected override void SingletonStarted()
    {
        base.SingletonStarted();
        EventAggregator.Register<DeadEvent>(this);
        EventAggregator.Register<PauseEvent>(this);
        GetMainMenuUI().gameObject.SetActive(true);
    }

    private GameObject GetOutGameUIHolder()
    {
        if(_outGameUIParent == null)
        {
            _outGameUIParent = Instantiate(_outGameUIParentPrefab);
            DontDestroyOnLoad(_outGameUIParent);
        }
        return _outGameUIParent;
    }

    private GameObject GetMainMenuUI()
    {
        if(_mainMenuUI == null)
        {
            _mainMenuUI = Instantiate(_mainMenuGameUIPrefab);
            DontDestroyOnLoad(_mainMenuUI);
        }
        return _mainMenuUI;
    }

    // public void Handle(RestartGameEvent @event)
    // {
    //     GetMainMenuUI().gameObject.SetActive(true);
    // }
}
