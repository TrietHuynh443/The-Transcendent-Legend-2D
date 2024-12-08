using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScene : BaseScene
{
    [SerializeField] private CanvasGroup _splashCanvasGroup;
    [SerializeField] private Image _sky;
    [SerializeField] private Color _endSkyColor;
    [SerializeField] private TextMeshProUGUI title;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_splashCanvasGroup.DOFade(1f, 3f))
            .AppendCallback(() =>
            {
                // Tween the sky color
                _sky.DOColor(_endSkyColor, 1f);
            })
            .AppendInterval(1f) // Wait for sky color tween to complete
            .AppendCallback(() =>
            {
                title.DOFade(1f, 2f);
            })
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                title.DOFade(0f, 1.5f);
            })
            .AppendInterval(1.5f);

        sequence.Play().OnComplete(()=>InGameManager.Instance.GetMainMenuUI().SetActive(true));
    }

}
