using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : UnitySingleton<GameEventManager>
{
    private GameEvent _currentGameState;
    private Action<GameEvent> _onPlayingEvent;
    private Action<GameEvent> _onVictoryEvent;
    private Action<GameEvent> _onDeadEvent;
    private Action<GameEvent> _onPauseEvent;

    protected override void SingletonAwake()
    {
        base.SingletonAwake();
        _currentGameState = GameEvent.Playing;
    }

    protected override void SingletonStarted()
    {
        base.SingletonStarted();
    }

    public void ChangeState(GameEvent gameEvent){
        if(_currentGameState == gameEvent) return;

        switch(gameEvent){
            case GameEvent.Playing:
                _onPlayingEvent?.Invoke(gameEvent);
                break;
            case GameEvent.Victory:
                _onVictoryEvent?.Invoke(gameEvent);
                break;
            case GameEvent.Dead:
                _onDeadEvent?.Invoke(gameEvent);
                break;
            case GameEvent.Pausing:
                _onPauseEvent?.Invoke(gameEvent);
                break;
        }

        _currentGameState = gameEvent;
    }

}
