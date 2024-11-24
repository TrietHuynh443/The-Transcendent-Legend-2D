using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingParallax : MonoBehaviour
{
    private Camera cam;
    private float startPos;
    private float length;


    [SerializeField]
    private float _parallaxEffect;
    [SerializeField]
    private float _scrollEffect;

    private float scroll = 0.0f;
    void Start()
    {
        cam = Camera.main;
        startPos = cam.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    // Update is called once per frame
    private void Update()
    {
        scroll += _scrollEffect * Time.deltaTime;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * _parallaxEffect;
        float movement = cam.transform.position.x * (1 - _parallaxEffect);
        
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
