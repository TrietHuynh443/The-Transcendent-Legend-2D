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
    [SerializeField] private float _onJumpGravityScale = 3f;
    [SerializeField] private float _originGravityScale = 1f;



    private float _horizontalInput;
    private float _currentJumpSpeed;
    private int _jumpCount; 
    private SpriteRenderer _spriteRenderer;

    private bool _isGrounded = false;


    // Start is called before the first frame update
    void Start()
    {   
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody.gravityScale = _originGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        float velocityY = _rigidbody.velocity.y;

        CheckGrounded();

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if(_isGrounded || _animator.GetBool("Jumping") && _jumpCount < _maxJump)
            {
                _currentJumpSpeed = _jumpSpeed;
                _animator.SetBool("Jumping", true);
                _jumpCount++;
                _isGrounded = false;
            }
        }

        if (velocityY == 0 && _currentJumpSpeed != _jumpSpeed)
        {
            _animator.SetBool("Jumping", false);
            _jumpCount = 0;
            _rigidbody.gravityScale = _originGravityScale;
        }
        else if(velocityY > 0)
            HandleOnJumping();
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        _isGrounded = hit.collider != null;
        // Debug.Log(hit.collider);
    }

    private void HandleOnJumping()
    {
       _rigidbody.gravityScale = _onJumpGravityScale;
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
