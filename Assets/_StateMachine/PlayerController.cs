using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private int _maxJump;

    private float _horizontalInput;
    private float _currentJumpSpeed;
    private int _jumpCount; 
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {   
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        float velocityY = _rigidbody.velocity.y;

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            && (_jumpCount < _maxJump))
        {
            _currentJumpSpeed = _jumpSpeed;
            _animator.SetBool("Jumping", true);
            _jumpCount++;
        }

        if (velocityY == 0 && _currentJumpSpeed != _jumpSpeed)
        {
            _animator.SetBool("Jumping", false);
            _jumpCount = 0;
        }
    }

    void FixedUpdate()
    {
        var abs = Mathf.Abs(_horizontalInput);
        if(abs >= 0.001f)
        {
            var newY = _horizontalInput < -0.001f ? 180 : 0;
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(transform.rotation.x, newY)));
        }
        
        _rigidbody.velocity = new Vector2(_horizontalInput * _speed, _rigidbody.velocity.y + _currentJumpSpeed);
        _animator?.SetFloat("Move", abs);
        _currentJumpSpeed = 0;
    }
}
