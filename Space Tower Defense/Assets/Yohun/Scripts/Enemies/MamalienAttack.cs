using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamalienAttack : MonoBehaviour
{
    public LayerMask whatIsTarget;
    public Vector3 hitboxRange;
    public Enemy enemy;
    private ObjectToDefense objectToDefense;
    private PlayerHealthBar player;
    private Collider[] hitColliders;
    [HideInInspector] public bool targetAttacked = false;
    [HideInInspector] public bool hitting;

    void Awake()
    {
        objectToDefense = FindObjectOfType<ObjectToDefense>();
        player = FindObjectOfType<PlayerHealthBar>();
    }

    void Update()
    {   
        if (hitting)
        {  
            hitColliders = Physics.OverlapBox(transform.position, hitboxRange, Quaternion.LookRotation(Vector3.forward), whatIsTarget);
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
        Gizmos.DrawWireCube(transform.position, hitboxRange);
    }
}
