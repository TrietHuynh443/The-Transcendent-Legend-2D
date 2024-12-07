using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OutGameUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _menuButton;

    private float _fadeDuration = 1f;



    private void OnEnable()
    {

        _resumeButton.onClick.AddListener(ResumeButtonClicked);
        _menuButton.onClick.AddListener(MenuButtonClicked);

    }

    void OnDisable()
    {

        _resumeButton.onClick.RemoveListener(ResumeButtonClicked);
        _menuButton.onClick.RemoveListener(MenuButtonClicked);

    }

    void ResumeButtonClicked()
    {
        EventAggregator.RaiseEvent<PauseEvent>(new PauseEvent());
    }

    void MenuButtonClicked()
    {
        EventAggregator.RaiseEvent<PauseEvent>(new PauseEvent());
        EventAggregator.RaiseEvent<RestartGameEvent>(new RestartGameEvent());
    }
}