using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _mainCam;

    [SerializeField]
    private GameObject[] _layers;

    [SerializeField]
    private float _defaultSpeed = 5f;

    [SerializeField]
    private float _speedControl = 2f;

    void Start()
    {
        _mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = _defaultSpeed;
        for (int i = 0; i < _layers.Length; i++)
        {
            _layers[i].transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            speed += _speedControl;
        }
    }
}
