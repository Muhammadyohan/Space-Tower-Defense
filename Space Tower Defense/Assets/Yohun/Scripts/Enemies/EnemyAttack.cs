using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask whatIsTarget;
    public float hitboxRange;
    public float centerOffset;
    public Enemy enemy;
    private ObjectToDefense objectToDefense;
    private PlayerHealthBar player;
    private Collider[] hitColliders;
    [HideInInspector] public bool targetAttacked = false;
    [HideInInspector] public bool hitting;
    Vector3 center;

    void Awake()
    {
        objectToDefense = FindObjectOfType<ObjectToDefense>();
        player = FindObjectOfType<PlayerHealthBar>();
    }

    void Update()
    {   
        center = new Vector3(transform.position.x, transform.position.y + centerOffset, transform.position.z);
        if (hitting)
        {  
            hitColliders = Physics.OverlapSphere(center, hitboxRange, whatIsTarget);
            if (!targetAttacked)
            {
                foreach (Collider hit in hitColliders)
                {
                    if (hit.gameObject.tag == "DefendedObject")
                    {
                        objectToDefense.TakeDamage(enemy.Damage);
                    }
                    if (hit.gameObject.tag == "Player")
                    {
                        player.TakeDamage(enemy.Damage);
                    }
                    targetAttacked = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, hitboxRange);
    }
}
