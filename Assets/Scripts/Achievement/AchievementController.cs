using System;
using System.Collections;
using System.Collections.Generic;
using Achievement;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    [SerializeField] private Image _achievementImage;
    [SerializeField] private TextMeshProUGUI _achievementNameTMP;
    [SerializeField] private TextMeshProUGUI _achievementDescriptionTMP;

    private CanvasGroup _canvasGroup;

    private CanvasGroup GetCanvasGroup()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }
        return _canvasGroup;
    }

    public void DisplayAchievement(EAchievementType type, AchievementData data)
    {
        StopAllCoroutines();
        GetCanvasGroup().DOFade(1, 1);
        _achievementImage.sprite = data.Resource;
        _achievementNameTMP.text = data.Name;
        _achievementDescriptionTMP.text = data.Description;
        StartCoroutine(HideAchievement());
    }

    private IEnumerator HideAchievement()
    {
        yield return new WaitForSeconds(4f);
        _canvasGroup.DOFade(0, 1);
    }
}
