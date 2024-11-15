using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    [SerializeField] protected GameObject _gameManagerPrefabs;
    protected GameObject _gameManagerObject;
    protected bool isInit = false;
}
