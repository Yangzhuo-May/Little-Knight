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
    private bool sawPlayer;
    private CircleCollider2D visionCollider;

    public float speed;
    public int moveDistance;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        colliderSize = collider.size;
        spriteRenderer = GetComponent<SpriteRenderer>();

        visionCollider = GetComponent<CircleCollider2D>();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sawPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sawPlayer = false;
        }
    }
    //public bool SawPlayer()
    //{
    //    Vector2 offsetPosition = (Vector2)transform.position + new Vector2(colliderSize.x / 2 + 0.5f, 0.25f);

    //    Debug.DrawRay(offsetPosition, Vector2.right * 2f, Color.green);

    //    bool sawPlayer = Physics2D.Raycast(offsetPosition, Vector2.right, 2f);

    //    return sawPlayer;
    //}

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
        Visuals();
        Patrol();

        if (sawPlayer)
        {
            UpdateState(IEnemy.State.attack);
        }
    }

    public void Attack()
    {
        Transform playerPostion = GameObject.Find("Player").transform;

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)playerPostion.position + new Vector2(0.5f, 0), Time.deltaTime * speed);

        if (!sawPlayer)
        {
            UpdateState(IEnemy.State.general);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        // spriteRenderer.flipX = !spriteRenderer.flipX;

        Vector3 localScale = transform.localScale;
        localScale.x = -localScale.x;
        transform.localScale = localScale;
    }

    private void Visuals()
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
    }

    private void Patrol()
    {
        if (forward)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPostion, Time.deltaTime * speed);
        }
        else if (backward)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPostion, Time.deltaTime * speed);
        }
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
