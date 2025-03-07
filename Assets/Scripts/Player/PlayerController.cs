using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputsManager inputsManager;
    AnimationManager animationManager;

    private bool isFacingRight = true;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private CircleCollider2D circleCollider2D;
    public float attackCheckRadius = 0.12f;

    public Rigidbody2D rb;
    public float speed;
    public float jumpForce;


    public float visionRadius = 5f; 
    public float visionAngle = 45f;

    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    private void Awake()
    {
        inputsManager = GetComponent<InputsManager>();
        animationManager = GetComponent<AnimationManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (circleCollider2D != null)
        {
            circleCollider2D.radius = attackCheckRadius;
        }
    }

    private void Update()
    {
        animationManager.SetVelocitY(rb.velocity.y);
        // transform.position = new Vector2(Mathf.Clamp(transform.position.x, -4.5f, 4.5f), transform.position.y);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float xMove = inputsManager.ReturnMoveValue();
        // transform.position += speed * Time.deltaTime * xMove * transform.right;
        rb.position += speed * Time.deltaTime * xMove * (Vector2)transform.right;

        animationManager.SetRunning(true);

        if (xMove > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (xMove < 0 && isFacingRight)
        {
            Flip();
        }

        if (xMove == 0) animationManager.SetRunning(false);
    }

    private bool IsGrounded()
    {
        //Vector2 rayOrigin = (Vector2)transform.position + Vector2.down * 0.1f;
        //Debug.DrawRay(rayOrigin, Vector2.down * 0.5f, Color.red);
        //bool isGrounded = Physics2D.Raycast(rayOrigin, Vector2.down, 0.5f);

        //return isGrounded;

        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGrounded())
        {
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animationManager.SetJumping();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animationManager.SetAttacking();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        // spriteRenderer.flipX = !spriteRenderer.flipX;

        Vector3 localScale = transform.localScale;
        localScale.x = -localScale.x; 
        transform.localScale = localScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
