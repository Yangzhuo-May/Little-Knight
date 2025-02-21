using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public static float speed;

    public enum State
    {
        idle,
        general,
        attack,
    }

    public IEnemy.State currentState { get; set; }
    public abstract void UpdateState(IEnemy.State state);
    public abstract void Idle();
    public abstract void GeneralBehavior();
    public abstract void Attack();
}
