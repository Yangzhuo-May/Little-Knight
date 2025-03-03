using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private const string RUNING = "isRunning";
    private const string JUMPING = "isJumping";
    private const string ATTACKING = "isAttacking";
    private const string Y_VELOCITY = "yVelocity";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool(RUNING, isRunning);
    }
    public void SetVelocitY(float Velocity)
    {
        animator.SetFloat(Y_VELOCITY, Velocity);
    }

    public void SetJumping()
    {
        animator.SetTrigger(JUMPING);
    }

    public void SetAttacking()
    {
        animator.SetTrigger(ATTACKING);
    }

}
