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

    public Rigidbody2D rb;
    public float speed;
    public float jumpForce;

    private void Awake()
    {
        move = actions.FindActionMap("Player").FindAction("Move");
        jump = actions.FindActionMap("Player").FindAction("Jump");
        attack = actions.FindActionMap("Player").FindAction("Attack");

        rb = GetComponent<Rigidbody2D>();
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

        transform.position = new Vector2(
        Mathf.Clamp(transform.position.x, -4.5f, 4.5f),
        transform.position.y);
    }

    private void Move()
    {
        float xMove = move.ReadValue<float>();
        transform.position += speed * Time.deltaTime * xMove * transform.right;
    }

    private bool isGrounded()
    {
        Debug.DrawRay((Vector2)transform.position + Vector2.down * 0.5f, Vector2.down * 0.25f, Color.red);
        return Physics2D.Raycast((Vector2)transform.position + Vector2.down * 0.5f, Vector2.down, 0.25f);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded()) return;

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
