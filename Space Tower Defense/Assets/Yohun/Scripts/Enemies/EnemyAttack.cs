using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask whatIsTarget;
    public float hitboxRange;
    public Enemy enemy;
    public AudioClip playerHittedSoundClip;
    public AudioClip defendedObjectHittedSoundClip;
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
            hitColliders = Physics.OverlapSphere(transform.position, hitboxRange, whatIsTarget);
            if (!targetAttacked)
            {
                foreach (Collider hit in hitColliders)
                {
                    if (hit.gameObject.tag == "DefendedObject")
                    {
                        objectToDefense.TakeDamage(enemy.Damage);
                        SoundFXManager.instance.PlayerSoundFXClip(defendedObjectHittedSoundClip, hit.transform, 1f);
                    }
                    if (hit.gameObject.tag == "Player")
                    {
                        player.TakeDamage(enemy.Damage);
                        SoundFXManager.instance.PlayerSoundFXClip(playerHittedSoundClip, hit.transform, 1f);
                    }
                    targetAttacked = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitboxRange);
    }
}
