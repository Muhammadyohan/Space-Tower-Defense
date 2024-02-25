using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    [Header("Gun Stats")]
    public float damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    // Reference
    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    // Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    public GameObject bulletImpactGraphic;
    public GameObject bloodImpactGarphic;
    public CameraShake cameraShake;
    public float cameraShakeMagnitude, cameraShakeDuration;
    public TextMeshProUGUI text;

    // Animation
    [Header("Animation")]
    public Animator animator;

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
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        // Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        else if (readyToShoot && shooting && !reloading && bulletsLeft == 0)
        {
            // Auto reload when out of ammo
            Reload();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        // RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponentInParent<Enemy>().TakeDamageFromPlayer(damage);
                Instantiate(bloodImpactGarphic, rayHit.point, Quaternion.FromToRotation(Vector3.forward , rayHit.normal));
            }
            else
            {
                // Graphic
                Instantiate(bulletImpactGraphic, rayHit.point, Quaternion.FromToRotation(Vector3.forward , rayHit.normal));
            }
        }

        // ShakeCamera
        StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));

        // Graphic
        Instantiate(muzzleFlash, attackPoint);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
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
