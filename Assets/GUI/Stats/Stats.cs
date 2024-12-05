using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private RectTransform healthBarTransform;
    
    
    private int maxHealthImgWidth;
    private int minHealthImgWidth = 0;
    
    private float _maxHealth => _playerDataSO.OriginalStats.Health;
    
    private float _currentHealth => _playerDataSO.CurrentStats.Health;
    [SerializeField] PlayerDataSO _playerDataSO;
    
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
