using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int NodeIndex;
    public float MaxHealth;
    public float Health;
    public float DamageResistance = 1f;
    public float Speed;
    public int ID;

    public void Init()
    {
        Health = MaxHealth;
        transform.position = GameLoopManager.NodePositions[0];
        NodeIndex = 0;
    }

    public void TakeDamageFromPlayer(float damage)
    {
        Health -= damage / DamageResistance;
        if (Health <= 0f)
        {
            GameLoopManager.EnqueueEnemyToRemove(this);
        } 
    }

}
