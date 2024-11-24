using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 5f;        // Speed of movement
    [SerializeField] float range = 2f;   // Left boundary
    private Rigidbody2D rb;             // Rigidbody2D component
    private Vector2 moveDirection = Vector2.right; // Initial movement direction
    private Vector2 currentPosition = Vector2.zero;
    private float boundaryLeft = 0;
    private float boundaryRight = 0;
    Animator animator;

    bool isOverLeft;
    bool isOverRight;

    int isOverLeftHash;
    int isOverRightHash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component attached to the object
        currentPosition = transform.position;
        boundaryLeft = currentPosition.x - range / 2;
        boundaryRight = currentPosition.x + range / 2;
        animator = GetComponent<Animator>();

        isOverLeftHash = Animator.StringToHash("isOverLeft");
        isOverRightHash = Animator.StringToHash("isOverRight");

        isOverLeft = animator.GetBool(isOverLeftHash);
        isOverRight = animator.GetBool(isOverRightHash);
    }

    void Update()
    {
        // Check if the character has reached the boundaries and flip direction if needed
        if (transform.position.x <= boundaryLeft)
        {
            // animator.SetBool(isOverRightHash, true);
            animator.SetBool(isOverLeftHash, true);
            moveDirection = Vector2.right;  // Move right when reaching the left boundary
        }
        else if (transform.position.x >= boundaryRight)
        {
            // animator.SetBool(isOverRightHash, false);
            animator.SetBool(isOverLeftHash, false);
            moveDirection = Vector2.left;   // Move left when reaching the right boundary
        }
    }

    void FixedUpdate()
    {
        // Move the character by changing its velocity
        rb.velocity = new Vector2(moveDirection.x * MoveSpeed, rb.velocity.y);
    }
}
