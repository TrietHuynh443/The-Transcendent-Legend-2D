using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // [SerializeField] private Color
    // private Button _button;
    [SerializeField][Range(0,1f)] private float _originAlpha;
    [SerializeField][Range(0,1f)] private float _hoverAlpha;

    [SerializeField]private Image _image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_image != null)
        {
            _image.DOFade(_hoverAlpha, 0.15f).SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_image != null)
        {
            _image.DOFade(_originAlpha, 0.15f).SetUpdate(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // _button = GetComponent<Button>();
        // _image = GetComponent<Image>();
        _image?.DOFade(_originAlpha, 0f).SetUpdate(true);

    }

    void Update()
    {
    }

}