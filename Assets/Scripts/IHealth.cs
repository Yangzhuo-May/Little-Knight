using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float currentHealth { get; set; }
    public float maxHealth { get; set; }
    
    public bool IsDead { get; }
    public abstract void TakeDamage(int damage);
}
