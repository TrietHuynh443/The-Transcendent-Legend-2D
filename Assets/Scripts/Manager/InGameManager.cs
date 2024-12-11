using System.Collections;
using System.Collections.Generic;
using GameEvent;
using UnityEngine;

public class InGameManager : UnitySingleton<InGameManager>,
    IGameEventListener<PlayerDieEvent>,
    IGameEventListener<PauseEvent>,
    IGameEventListener<RespawnEvent>,
    IGameEventListener<QuitToMenuEvent>,
    IGameEventListener<ResumeEvent>,
    IGameEventListener<DisplayAchievement>

{
    [SerializeField] private GameObject _gameOverUIPrefab;
    [SerializeField] private GameObject _gamePauseUIPrefab;
    [SerializeField] private GameObject _outGameUIParentPrefab;
    [SerializeField] private GameObject _mainMenuGameUIPrefab;
    [SerializeField] private GameObject _achievementDisplayPrefab;

    private GameObject _gameOverUI;
    private GameObject _gamePauseUI;
    private GameObject _outGameUIParent;
    private GameObject _mainMenuUI;
    private GameObject _achievementDisplay;
    private GameObject _player;
    

    public GameObject Player
    {
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
        GetGameOverUI().SetActive(true);
    }

    public void Handle(PauseEvent @event)
    {
        if (GetGameOverUI().activeSelf)
        {
            _gameOverUI.SetActive(false);
            return;
        }

        if (GetMainMenuUI().activeSelf)
        {
            return; //Don't show pause menu (on ESC button) when showing main menu
        }

        @event.Pause();
        GetGamePauseUI().SetActive(true);
    }

    protected override void SingletonStarted()
    {
        base.SingletonStarted();
        EventAggregator.Register<PlayerDieEvent>(this);
        EventAggregator.Register<PauseEvent>(this);
        EventAggregator.Register<RespawnEvent>(this);
        EventAggregator.Register<QuitToMenuEvent>(this);
        EventAggregator.Register<ResumeEvent>(this);
        EventAggregator.Register<DisplayAchievement>(this);
        GetMainMenuUI();//.gameObject.SetActive(false); //spawn UI
        GetOutGameUIHolder();//.gameObject.SetActive(false); //spawn UI
    }

    private GameObject GetGameOverUI()
    {
        if (_gameOverUI == null)
        {
            _gameOverUI = Instantiate(_gameOverUIPrefab, GetOutGameUIHolder().transform);
            // DontDestroyOnLoad(_gameOverUI);
        }
        return _gameOverUI;
    }

    private GameObject GetAchievementDisplay()
    {
        if (_achievementDisplay == null)
        {
            _achievementDisplay = Instantiate(_achievementDisplayPrefab, GetOutGameUIHolder().transform);
        }

        return _achievementDisplay;
    }

    private GameObject GetGamePauseUI()
    {
        if (_gamePauseUI == null)
        {
            _gamePauseUI = Instantiate(_gamePauseUIPrefab, GetOutGameUIHolder().transform);
            // DontDestroyOnLoad(_gamePauseUI);
        }

        return _gamePauseUI;
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

        // _mainMenuUI.SetActive(true);
        return _mainMenuUI;
    }

    // public void Handle(RestartGameEvent @event)
    // {
    //     GetMainMenuUI().gameObject.SetActive(true);
    // }
    public void Handle(RespawnEvent @event)
    {
        GetGameOverUI().SetActive(false);
        if (_player != null)
        {
            _player.SetActive(true);
        }
    }

    public void Handle(QuitToMenuEvent @event)
    {
        GetMainMenuUI().SetActive(true);
        GetGameOverUI().SetActive(false);
        GetGamePauseUI().SetActive(false);

        PauseEvent pauseEvent = new PauseEvent();
        pauseEvent.Resume();
    }

    public void Handle(ResumeEvent @event)
    {
        GetMainMenuUI().SetActive(false);
        GetGameOverUI().SetActive(false);
        GetGamePauseUI().SetActive(false);

        PauseEvent pauseEvent = new PauseEvent();
        pauseEvent.Resume();
    }

    public void Handle(DisplayAchievement @event)
    {
        GetAchievementDisplay();
        
        _achievementDisplay.GetComponent<AchievementController>().DisplayAchievement(@event.Type, @event.Data);
    }
}