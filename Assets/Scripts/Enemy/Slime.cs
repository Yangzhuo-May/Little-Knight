using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.GraphicsBuffer;

public class Slime : MonoBehaviour, IEnemy
{
    private IEnemy.State _currentState;
    public IEnemy.State currentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }
    private BoxCollider2D collider;
    private Vector2 colliderSize;
    private Vector2 initialPostion;
    private Vector2 targetPostion;
    private float counter;
    private float waitTime = 3.0f;
    private bool forward, backward;
    private bool isFacingRight = true;
    private SpriteRenderer spriteRenderer;

    public float speed;
    public int moveDistance;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        colliderSize = collider.size;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (_currentState)
        {
            case IEnemy.State.idle:
                Idle();
                break;

            case IEnemy.State.general:
                GeneralBehavior();
                break;

            case IEnemy.State.attack:
                Attack();
                break;
        }
    }

    private void SavePosition()
    {
        initialPostion = (Vector2)transform.position;
        targetPostion = new Vector2(initialPostion.x + moveDistance, initialPostion.y);
    }
 

    public void UpdateState(IEnemy.State state)
    {
        _currentState = state;
    }

    public bool SawPlayer()
    {
        Debug.DrawRay((Vector2)transform.position + Vector2.right * ((colliderSize.x / 2) + 0.25f), Vector2.right * 2f, Color.green);

        bool sawPlayer = Physics2D.Raycast((Vector2)transform.position + Vector2.right * ((colliderSize.x / 2) + 0.25f), Vector2.right, 2f);

        return sawPlayer;
    }

    public void Idle()
    {
        counter += Time.deltaTime;
        if (counter >= waitTime)
        {
            UpdateState(IEnemy.State.general);
            SavePosition();
        }
    }

    public void GeneralBehavior()
    {

        if ((Vector2)transform.position == initialPostion)
        {
            if (!isFacingRight) Flip();
            forward = true;
            backward = false;
        }
        else if ((Vector2)transform.position == targetPostion)
        {
            if (isFacingRight) Flip();
            backward = true;
            forward = false;
        }


        if (forward)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPostion, Time.deltaTime * speed);
        }
        else if (backward)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPostion, Time.deltaTime * speed);
        }

        if (SawPlayer())
        {
            UpdateState(IEnemy.State.attack);
        }

    }

    public void Attack()
    {
        Debug.Log("is attacking");

        if (!SawPlayer())
        {
            UpdateState(IEnemy.State.general);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    /* if player is within the slime's attack range
     *      go to the position of player and attack
     * else
     *      keep general behavior
     *      
     * Set the distence for walking and check if it will falling down
     * if it's wall, return, if it's player, attack
     */
}
