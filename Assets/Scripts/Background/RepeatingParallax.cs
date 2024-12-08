using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingParallax : MonoBehaviour
{
    private Camera cam;
    private float startPosX;
    private float startPosY;
    private Vector3 length;


    [SerializeField]
    private float _parallaxXEffect;
    [SerializeField]
    private float _parallaxYEffect;
    [SerializeField]
    private float _scrollXEffect;

    void Start()
    {
        cam = Camera.main;
        startPosX = cam.transform.position.x;
        startPosY = cam.transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size;
    }

    void FixedUpdate()
    {
        startPosX += _scrollXEffect * Time.fixedDeltaTime;
        float distanceX = cam.transform.position.x * _parallaxXEffect;
        float movementX = cam.transform.position.x * (1 - _parallaxXEffect);
        
        float distanceY = cam.transform.position.y * _parallaxYEffect;
        
        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        if (movementX > startPosX + length.x)
        {
            startPosX += length.x;
        }
        else if (movementX < startPosX - length.x)
        {
            startPosX -= length.x;
        }
    }
}
