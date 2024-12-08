using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] float _fadeDuration = 1;

    private bool fadeIn = false;
    private bool fadeOut = false;

    public float FadeDuration => _fadeDuration;
    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }

    void Start()
    {
        FadeOut();
    }
    // Update is called once per frame
    void Update()
    {
        if (fadeIn == true)
        {
            if (_canvasGroup.alpha >= 1)
            {
                _canvasGroup.alpha = 0;
            }
            _canvasGroup.alpha += Time.deltaTime / _fadeDuration;
            if (_canvasGroup.alpha >= 1)
            {
                fadeIn = false;
            }
        }

        if (fadeOut == true)
        {
            if (_canvasGroup.alpha <= 0)
            {
                _canvasGroup.alpha = 1;
            }
            _canvasGroup.alpha -= Time.deltaTime / _fadeDuration;
            if (_canvasGroup.alpha <= 0)
            {
                fadeOut = false;
            }
        }
    }
}
