using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

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
    public UnityEvent DeadEvent;
    [Header("SoundFX")]
    [SerializeField] private AudioClip deadSoundFX;
    private int playOnce = 0;
    [Header("Health Bar")]
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
            DeadEvent.Invoke();
            if (playOnce < 1)
            {
                animator.SetTrigger("Dead");
                SoundFXManager.instance.PlayerSoundFXClip(deadSoundFX, transform, 0.1f);
                playOnce++;
            }
        } 
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
