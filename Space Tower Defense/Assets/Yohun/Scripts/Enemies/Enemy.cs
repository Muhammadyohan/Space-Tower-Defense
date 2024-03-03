using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int ID;

    [Header("Stats")]
    public float MaxHealth;
    public float Health;
    public float DamageResistance = 1f;
    public float Damage = 10f;
    
    [Header("Animation")]
    public Animator animator;
    public bool hasAnimation = false;

    [SerializeField] private HealthBar healthBar;
    private float totalDamage;

    public void Init()
    {
        Health = MaxHealth;
        healthBar.maxHealth = MaxHealth;
        healthBar.health = MaxHealth;
    }

    public void TakeDamageFromPlayer(float damage)
    {
        Health -= damage / DamageResistance;
        totalDamage = damage / DamageResistance;
        healthBar.TakeDamage(totalDamage);
        if (Health <= 0f)
        {
            if (hasAnimation)
                animator.SetBool("Dead", true);
            else
                Destroy(this);
        } 
    }
}
