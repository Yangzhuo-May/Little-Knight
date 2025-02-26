using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public InputActionAsset actions;
    private InputAction move;
    private InputAction jump;
    private InputAction attack;

    private Animator animator;
    private bool isJumping = false;
    private bool isRunning = false;

    private bool isFacingRight = true;
    private SpriteRenderer spriteRenderer;

    private BoxCollider2D collider;
    private Vector2 colliderSize;

    public Transform attackCheckPos;
    public Vector2 attackCheckSize = new Vector2(1f, 0.2f);

    public Rigidbody2D rb;
    public float speed;
    public float jumpForce;

    private void Awake()
    {
        move = actions.FindActionMap("Player").FindAction("Move");
        jump = actions.FindActionMap("Player").FindAction("Jump");
        attack = actions.FindActionMap("Player").FindAction("Attack");
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        collider = GetComponent<BoxCollider2D>();
        colliderSize = collider.size;
    }

    private void OnEnable()
    {
        actions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        actions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        Move();

        animator.SetFloat("yVelocity", rb.velocity.y);
        // transform.position = new Vector2(Mathf.Clamp(transform.position.x, -4.5f, 4.5f), transform.position.y);

    }

    private void Move()
    {
        float xMove = move.ReadValue<float>();
        transform.position += speed * Time.deltaTime * xMove * transform.right;

        animator.SetBool("isRunning", true);

        if (xMove > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (xMove < 0 && isFacingRight)
        {
            Flip();
        }

        if (xMove == 0) animator.SetBool("isRunning", false);
    }

    private bool isGrounded()
    {
        Vector2 rayOrigin = (Vector2)transform.position + Vector2.down * 0.1f;
        Debug.DrawRay(rayOrigin, Vector2.down * 0.5f, Color.red);
        bool isGrounded = Physics2D.Raycast(rayOrigin, Vector2.down, 0.5f);
        
        animator.SetBool("isGrounded", isGrounded);

        return isGrounded;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded())
        {
            return;
        }

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("isJumping");
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("isAttacking");
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(new Vector3(attackCheckPos.position.x, 0, 0), attackCheckSize);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
