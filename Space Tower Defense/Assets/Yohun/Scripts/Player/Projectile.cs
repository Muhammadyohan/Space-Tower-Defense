using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;

    // bullet force
    public float shootForce, upwardForce;

    [Header("Gun Stats")]
    public float damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    [Header("Recoil")]
    public Rigidbody playerRb;
    public float recoilForce;
    public CameraShake cameraShake;
    public float cameraShakeMagnitude, cameraShakeDuration;

    // bools
    bool shooting, readyToShoot, reloading;

    [Header("Reference")]
    public Camera fpsCam;
    public Transform attackPoint;
    [Header("Graphics")]
    public GameObject muzzleFlash;
    private WeaponGUI weaponGUI;
    [Header("Animation")]
    public Animator animator;
    [Header("SoundFX")]
    public AudioClip shootSoundClip;
    public AudioClip reloadSoundClip;
    [Header("Debug")]
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        reloading = false;
        weaponGUI = FindObjectOfType<WeaponGUI>();
    }

    private void Update()
    {
        MyInput();

        // Set text
        weaponGUI.bulletLeftText.SetText((bulletsLeft / bulletsPerTap).ToString());
        weaponGUI.magazineSizeText.SetText((magazineSize / bulletsPerTap).ToString());
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
            animator.SetBool("Shooting", true);
        }
        else if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            // Auto reload when out of ammo
            Reload();
        }
        else if (!shooting)
            animator.SetBool("Shooting", false);
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
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.LookRotation(attackPoint.forward));
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

        // Play shoot SFX
        SoundFXManager.instance.PlayerSoundFXClip(shootSoundClip, attackPoint, 1f);

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

        SoundFXManager.instance.PlayerSoundFXClip(reloadSoundClip, transform, 1f);

        animator.SetBool("Reloading", false);

        reloading = false;
    }
}
