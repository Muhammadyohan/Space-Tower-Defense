using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamalienAttack : MonoBehaviour
{
    public LayerMask whatIsTarget;
    public Vector3 hitboxRange;
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
            hitColliders = Physics.OverlapBox(transform.position, hitboxRange, transform.rotation, whatIsTarget);
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
        Gizmos.DrawWireCube(transform.position, hitboxRange);
    }
}
