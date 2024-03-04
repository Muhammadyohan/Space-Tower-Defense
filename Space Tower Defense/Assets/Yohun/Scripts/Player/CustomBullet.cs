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
    public bool isSticky;
    public bool isExplode;

    // Damage
    [Header("Damage")]
    public int damage;
    public float explosionRange;

    // Lifetime
    [Header("Lifetime")]
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    // Graphics
    [Header("Graphics")]
    public GameObject bulletImpactGraphic;
    public GameObject bloodImpactGarphic;
    RaycastHit hit;
    int hitCount;
    int collisions;
    PhysicMaterial physics_mat;

    [Header("Debug")]
    public SphereCollider sphereCollider;

    void Start()
    {
        Setup();
    }
    
    void Update()
    {
        // When to explode
        if (isExplode)
            if (collisions > maxCollisions) Explode();

        // Count down lifetime
        if (isExplode)
        {
            maxLifetime -= Time.deltaTime;
            if (maxLifetime <= 0) Explode();
        }
    }

    void FixedUpdate()
    {
        // Bullet Impact
        if (explosion == null)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.1f))
            {
                if (hitCount <= 0)
                {
                    hitCount++;
                    if (hit.collider.CompareTag("Enemy"))
                        Instantiate(bloodImpactGarphic, hit.point, Quaternion.FromToRotation(Vector3.forward , hit.normal));
                    else if(!hit.collider.CompareTag("Invisible"))
                        Instantiate(bulletImpactGraphic, hit.point, Quaternion.FromToRotation(Vector3.forward , hit.normal), hit.transform);
                }
            }
        }
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
            try
            {
                enemies[i].GetComponent<Enemy>().TakeDamageFromPlayer(damage);
            }
            catch
            {
                enemies[i].GetComponentInParent<Enemy>().TakeDamageFromPlayer(damage);
            }
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
        if (isSticky)
            rb.isKinematic = true;
        
        // Don't count collisions with other bullets
        if (other.collider.CompareTag("Bullet")) return;

        // Count up collisions
        collisions++;

        if (isExplode)
        {
            // Explode if bullet hits an enemy directly and explodeOnTouch is activated
            if (other.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
        }
        else
        {
            if (other.collider.CompareTag("Enemy") && explodeOnTouch)
            {
                other.transform.GetComponentInParent<Enemy>().TakeDamageFromPlayer(damage);
                Debug.Log("Hitted Enemy");
            }
            Invoke("Delay", 0.02f);
        }
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
