using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    // bullet
    [Header("Bullet")]
    public GameObject bullet;

    // bullet force
    public float shootForce, upwardForce;

    // Gun stats
    [Header("Gun Stats")]
    public float damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // Recoil
    [Header("Recoil")]
    public Rigidbody playerRb;
    public float recoilForce;

    // bools
    bool shooting, readyToShoot, reloading;

    // Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;

    // Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    public GameObject bulletImpactGraphic;
    public GameObject bloodImpactGarphic;
    public CameraShake cameraShake;
    public float cameraShakeMagnitude, cameraShakeDuration;
    public TextMeshProUGUI ammunitionDisplay;

    // Animation
    [Header("Animation")]
    public Animator animator;

    [Header("Debug")]
    // bug fixing
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        reloading = false;
    }

    private void Update()
    {
        MyInput();

        // Set text
        if (ammunitionDisplay != null) 
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }

    private void MyInput()
    {   
        // Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        
        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            // Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
        else if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            // Auto reload when out of ammo
            Reload();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            // Graphic
            if (hit.collider.CompareTag("Enemy"))
                Instantiate(bloodImpactGarphic, hit.point, Quaternion.FromToRotation(Vector3.forward , hit.normal));
            else
            {
                // Graphic
                Instantiate(bulletImpactGraphic, hit.point, Quaternion.FromToRotation(Vector3.forward , hit.normal));
            }
        }
        else
            targetPoint = ray.GetPoint(75); // Just a point far away from the player

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x,y,0);

        // Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        // Shake Camera
        StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));

        // Instantiate muzzle flash
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint);

        bulletsLeft--;
        bulletsShot++;

        // Invoke resetShot function (is not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            // Add recoil to player
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        // If more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }


    private void Reload()
    {
        reloading = true;

        animator.SetBool("Reloading", true);

        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;

        animator.SetBool("Reloading", false);

        reloading = false;
    }
}
