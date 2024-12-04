using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OutGameUIHandler : MonoBehaviour, IGameEventListener<PauseEvent>
{
    [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private Canvas _menuCanvas;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _menuButton;

    private bool gamePaused => PauseEvent._gameIsPaused;
    private bool onMenu => UIStartMenuHandler._onMenu;
    private float _fadeDuration = 1f;

    public void Handle(PauseEvent @event)
    {
        if(onMenu)
            return;

        @event.Pause();
        _pauseCanvas.gameObject.SetActive(gamePaused);
        Debug.Log($"game is paused ? '{gamePaused}'");
    }

    private void OnEnable()
    {
        EventAggregator.Register<PauseEvent>(this);
        
        if (_resumeButton == null || _menuButton == null)  
                Debug.Log("Where button ?");
       

        _resumeButton.onClick.AddListener(ResumeButtonClicked);
        _menuButton.onClick.AddListener(MenuButtonClicked);

    }

    void OnDisable()
    {
        EventAggregator.Unregister<PauseEvent>(this);

        if (_resumeButton == null || _menuButton == null)         
            Debug.Log("Where button ?");

        _resumeButton.onClick.RemoveListener(ResumeButtonClicked);
        _menuButton.onClick.RemoveListener(MenuButtonClicked);

    }

    void ResumeButtonClicked()
    {
        this.Handle(new PauseEvent());
        _pauseCanvas.gameObject.SetActive(gamePaused); 
    }

    void MenuButtonClicked()
    {
        this.Handle(new PauseEvent());
        _pauseCanvas.gameObject.SetActive(gamePaused); 
        _menuCanvas.GetComponent<CanvasGroup>()
            .DOFade(1f, _fadeDuration)
            .OnComplete(
                () => {
                    _menuCanvas.gameObject.SetActive(true);
                }
            );
    
    }
}