using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIStartMenuHandler : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Canvas _startMenuCanvas;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private GameObject _startElementRoot;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(PlayButtonClicked);
    }

    void OnDisable()
    {
        _playButton.onClick.RemoveListener(PlayButtonClicked);
    }

    private void PlayButtonClicked()
    {
        _startMenuCanvas.GetComponent<CanvasGroup>()
            .DOFade(0, _fadeDuration)
            .OnComplete(
                () => {
                    _startMenuCanvas.gameObject.SetActive(false);
                    InitScene();
                }
            );
    }

    private void InitScene()
    {
        _startElementRoot.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
