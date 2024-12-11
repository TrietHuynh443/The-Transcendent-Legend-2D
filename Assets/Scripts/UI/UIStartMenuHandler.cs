using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIStartMenuHandler : MonoBehaviour, IGameEventListener<RestartGameEvent>
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Canvas _startMenuCanvas;
    [SerializeField] private float _fadeDuration = 0.5f;

    // public static bool _onMenu = true;

    private void Start()
    {
        EventAggregator.Register<RestartGameEvent>(this);
        _playButton.onClick.AddListener(PlayButtonClicked);
    }
    private void OnEnable()
    {
        // _onMenu = true;
    }

    void OnDisable()
    {
        // _onMenu = false;
    }

    void OnDestroy()
    {
        EventAggregator.Unregister<RestartGameEvent>(this);
        _playButton.onClick.RemoveListener(PlayButtonClicked);
    }

    private void PlayButtonClicked()
    {
        _startMenuCanvas.GetComponent<CanvasGroup>()
            .DOFade(0, _fadeDuration)
            .OnComplete(
                () =>
                {
                    _startMenuCanvas.GetComponent<CanvasGroup>().alpha = 1;
                    _startMenuCanvas.gameObject.SetActive(false);
                    StartGame();
                }
            );
    }

    private void StartGame()
    {
        EventAggregator.RaiseEvent<StartGameEvent>(new StartGameEvent(){Level = 1});
    }

    public void Handle(RestartGameEvent @event)
    {
        _startMenuCanvas.GetComponent<CanvasGroup>()
            .DOFade(1, 0)
            .OnComplete(
                () => {
                    _startMenuCanvas.gameObject.SetActive(true);
                    // StartGame();
                }
            );
    }
}
