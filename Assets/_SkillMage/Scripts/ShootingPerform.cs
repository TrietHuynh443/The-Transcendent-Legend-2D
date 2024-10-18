using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPerform : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _force;

    [SerializeField] private GameObject _objectShootingPrefab;

    private Queue<GameObject> _objectsShootingPool = new();
    private Rigidbody2D _objectShootingBody;

    private List<GameObject> _objectsShootingInProgress = new();
    private Action _shoot;
    private bool _isShooting;

    private void OnEnable()
    {
        
        _objectsShootingPool.Enqueue(SpawnShootingObject());
        _shoot += Shooting;
    }

    private GameObject SpawnShootingObject()
    {
        GameObject spawnObject = Instantiate(_objectShootingPrefab, transform.position, Quaternion.identity);
        spawnObject.SetActive(false);
        return spawnObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !_isShooting){
            _isShooting = true;
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
        _objectShootingBody = objectShooting.GetComponent<Rigidbody2D>();
        if(_objectShootingBody != null){
            objectShooting.SetActive(true);
            objectShooting.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            _objectShootingBody.AddForce(_direction*_force, ForceMode2D.Impulse);
            _objectsShootingInProgress.Add(objectShooting);
        }
    } 
}
