using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class InGameManager : UnitySingleton<InGameManager>,
    IGameEventListener<PlayerDieEvent>,
    IGameEventListener<PauseEvent>,
    IGameEventListener<RespawnEvent>,
    IGameEventListener<QuitToMenuEvent>

{
    [SerializeField] private GameObject _gameOverUIPrefab;
    [SerializeField] private GameObject _gamePauseUIPrefab;
    [SerializeField] private GameObject _outGameUIParentPrefab;
    [SerializeField] private GameObject _mainMenuGameUIPrefab;

    private GameObject _gameOverUI;
    private GameObject _gamePauseUI;
    private GameObject _outGameUIParent;
    private GameObject _mainMenuUI;
    private GameObject _player;

    public GameObject Player {
        get => _player;
        set => _player = value;
    }
    
    public void Handle(PlayerDieEvent @event)
    {
        StartCoroutine(CheckDieAnimationFinished(@event));
    }

    private IEnumerator CheckDieAnimationFinished(PlayerDieEvent @event)
    {
        yield return new WaitForSeconds(@event.DieAnimationTime);
        if (_gameOverUI == null)
        {
            _gameOverUI = Instantiate(_gameOverUIPrefab, GetOutGameUIHolder().transform);
            // DontDestroyOnLoad(_gameOverUI);
        }

        _gameOverUI.SetActive(true);
    }

    public void Handle(PauseEvent @event)
    {
        if (_gamePauseUI == null)
        {
            _gamePauseUI = Instantiate(_gamePauseUIPrefab, GetOutGameUIHolder().transform);
            // DontDestroyOnLoad(_gamePauseUI);
        }

        @event.Pause();
        _gamePauseUI.SetActive(PauseEvent.IsPaused);
    }

    protected override void SingletonStarted()
    {
        base.SingletonStarted();
        EventAggregator.Register<PlayerDieEvent>(this);
        EventAggregator.Register<PauseEvent>(this);
        EventAggregator.Register<RespawnEvent>(this);
        EventAggregator.Register<QuitToMenuEvent>(this);
        GetMainMenuUI().gameObject.SetActive(false); //spawn UI
        GetOutGameUIHolder().gameObject.SetActive(false); //spawn UI
    }

    private GameObject GetOutGameUIHolder()
    {
        if (_outGameUIParent == null)
        {
            _outGameUIParent = Instantiate(_outGameUIParentPrefab);
            DontDestroyOnLoad(_outGameUIParent);
        }

        _outGameUIParent.SetActive(true);
        return _outGameUIParent;
    }

    public GameObject GetMainMenuUI()
    {
        if (_mainMenuUI == null)
        {
            _mainMenuUI = Instantiate(_mainMenuGameUIPrefab);
            DontDestroyOnLoad(_mainMenuUI);
        }

        _mainMenuUI.SetActive(true);
        return _mainMenuUI;
    }

    // public void Handle(RestartGameEvent @event)
    // {
    //     GetMainMenuUI().gameObject.SetActive(true);
    // }
    public void Handle(RespawnEvent @event)
    {
        _gameOverUI.SetActive(false);
        if (_player != null)
        {
            _player.GetComponent<Animator>().enabled = false;
            _player.SetActive(true);
            _player.GetComponent<Animator>().enabled = true;

        };
    }

    public void Handle(QuitToMenuEvent @event)
    {
        GetMainMenuUI().gameObject.SetActive(true);
        GetOutGameUIHolder().gameObject.SetActive(false);
    }
}