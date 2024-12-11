using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
   private RectTransform healthBarTransform;
    
    
    private int maxHealthImgWidth;
    private int minHealthImgWidth = 0;
    
    public Boss boss;
    private float _maxHealth => boss.MaxHealth;
    
    private float _currentHealth => boss.CurrentHealth;

    void Start()
    {
        healthBarTransform = transform.Find("Boss Health Bar").gameObject.GetComponent<RectTransform>();
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
