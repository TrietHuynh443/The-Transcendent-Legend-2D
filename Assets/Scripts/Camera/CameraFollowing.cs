using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, this.transform.position.z);
    }
}
