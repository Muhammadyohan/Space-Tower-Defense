using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int NodeIndex;
    public float MaxHealth;
    public float Health;
    public float DamageResistance = 1f;
    public float Speed;
    public float Damage = 10f;
    public int ID;
    public Animator animator;
    public bool hasAnimation = false;

    [SerializeField] private HealthBar healthBar;
    private float totalDamage;

    public void Init()
    {
        Health = MaxHealth;
        healthBar.maxHealth = MaxHealth;
        healthBar.health = MaxHealth;
        transform.position = GameLoopManager.NodePositions[0];
        NodeIndex = 0;
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
                GameLoopManager.EnqueueEnemyToRemove(this);
        } 
    }

    public void EnemyDeath()
    {
        
    }
}
