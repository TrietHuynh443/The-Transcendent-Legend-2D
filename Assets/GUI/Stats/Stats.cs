using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private RectTransform healthBarTransform;
    
    
    private int maxHealthImgWidth;
    private int minHealthImgWidth = 0;
    
    [SerializeField]
    private float _maxHealth = 100.0f;
    
    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float _currentHealth = 100.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBarTransform = transform.Find("Health Bar").gameObject.GetComponent<RectTransform>();
        maxHealthImgWidth = (int)healthBarTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        float healthPercent = Mathf.Clamp(_currentHealth / _maxHealth, 0.0f, 1.0f);
        int newWidth = (int)((maxHealthImgWidth - minHealthImgWidth) * healthPercent + minHealthImgWidth);
        healthBarTransform.sizeDelta = new Vector2(newWidth, healthBarTransform.sizeDelta.y);
    }
}
