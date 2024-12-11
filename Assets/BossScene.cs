using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    [SerializeField] private GameObject _boss;
    private void OnEnable()
    {
        var _player = GameObject.FindWithTag("Player");
        if(_player != null)
        {
            _boss.SetActive(true);
        }
    }
}
