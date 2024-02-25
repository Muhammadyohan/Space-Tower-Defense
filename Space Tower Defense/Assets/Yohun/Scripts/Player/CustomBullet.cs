using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    // Assignables
    [Header("Reference")]
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    // Stats
    [Header("Bullet Stats")]
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    // Damage
    [Header("Damage")]
    public int explosionDamage;
    public float explosionRange;

    // Lifetime
    [Header("Lifetime")]
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    void Start()
    {
        Setup();
    }
    
    void Update()
    {
        // When to explode
        if (collisions > maxCollisions) Explode();

        // Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    void Explode()
    {
        // Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        // Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get component of enemy and call Take Damage
            enemies[i].GetComponentInParent<Enemy>().TakeDamageFromPlayer(explosionDamage);
        }

        // Add a little delay, just to make sure everything works fine
        Invoke("Delay", 0.02f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        // Don't count collisions with other bullets
        if (other.collider.CompareTag("Bullet")) return;

        // Count up collisions
        collisions++;

        // Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (other.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }

    private void Setup()
    {
        // Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        // Assign material to coliider
        GetComponent<SphereCollider>().material = physics_mat;

        // Set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
