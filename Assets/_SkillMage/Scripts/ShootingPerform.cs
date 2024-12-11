using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShootingPerform : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _force;

    [SerializeField] private GameObject _objectShootingPrefab;

    [SerializeField] private Image _cooldownUI;

    private Queue<GameObject> _objectsShootingPool = new();
    private Rigidbody2D _objectShootingBody;

    private List<GameObject> _objectsShootingInProgress = new();
    private Action _shoot;
    private bool _isShooting;
    private Animator _animator;
    private float _coolDown = 7f;
    private float _startSkillTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _objectsShootingPool.Enqueue(SpawnShootingObject());
        _shoot += Shooting;
        _startSkillTime = float.MinValue;

    }

    private GameObject SpawnShootingObject()
    {
        GameObject spawnObject = Instantiate(_objectShootingPrefab, transform.position, Quaternion.identity);
        spawnObject.SetActive(false);
        return spawnObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !_isShooting && (Time.time - _startSkillTime) >= _coolDown){
            _isShooting = true;
            _startSkillTime = Time.time;
            _cooldownUI.fillAmount = 1;
            _cooldownUI.DOFillAmount(0, _coolDown);
            EventAggregator.RaiseEvent<PlayerSkillEvent>(new PlayerSkillEvent(){Type=PlayerSkillType.Toxic});
            _animator.Play("Attack", 0);
        }

        ReloadObjectShootingPool();
    }

    private void FixedUpdate()
    {
        if (_isShooting){
            _shoot?.Invoke();
            _isShooting = false;
        }
    }

    private void ReloadObjectShootingPool()
    {
        int len = _objectsShootingInProgress.Count;
        bool[] indexArr = new bool[len]; 
        for(int i = 0; i < len; i++)
        {
            var obj = _objectsShootingInProgress[i];
            if(obj.activeSelf == false){
                _objectsShootingPool.Enqueue(obj);
                indexArr[i] = true;
            }
        }

        for(int i = 0; i < len; i++)
        {
            if(indexArr[i]){
                _objectsShootingInProgress.RemoveAt(i);
            }
        }
    }

    public void Shooting(){
        if(!_isShooting) return;

        if(!_objectsShootingPool.TryDequeue(out GameObject objectShooting)){
            objectShooting = SpawnShootingObject();
        }
        StartCoroutine(WaitForAttackToFinishThenShoot(objectShooting));
    }

    IEnumerator WaitForAttackToFinishThenShoot(GameObject objectShooting)
    {
        AnimatorStateInfo stateInfo;
        do
        {
            stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        while (stateInfo.IsName("Attack") && stateInfo.normalizedTime < 0.5f);
        var pos = transform.position;
        pos.x+=0.6f*transform.right.x;
        pos.y+=0.45f*transform.up.y;
        // Perform shooting
        _objectShootingBody = objectShooting.GetComponent<Rigidbody2D>();
        if(_objectShootingBody != null){
            objectShooting.SetActive(true);
            objectShooting.transform.SetPositionAndRotation(pos, Quaternion.identity);
            _direction.x = transform.right.x;
            _objectShootingBody.AddForce(_direction*_force, ForceMode2D.Impulse);
            _objectsShootingInProgress.Add(objectShooting);
        }
    }
}
