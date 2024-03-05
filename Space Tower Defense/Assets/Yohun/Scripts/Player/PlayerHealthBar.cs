using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("Unity Event")]
    public UnityEvent WaitToRespawnEvent;
    public UnityEvent RespawnEvent;
    [Header("Player Reference")]
    public PlayerMovement player;
    public PlayerCam playerCam;
    [Header("Set Respawn Point of the Player")]
    public Transform respawnPoint;
    [Header("GUI Reference")]
    public Image healthBarFill;
    public Image healthEaseBarFill;
    public Image shieldBarFill;
    public Image shieldEaseBarFill;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;

    [Header("Anoucement")]
    public TextMeshProUGUI respawnTimeTextAnounce;

    [Header("Take Damage VFX")]
    public float intensity = 0;
    [SerializeField] PostProcessVolume volume;
    public CameraShake cameraShake;
    public float cameraShakeMagnitude;
    public float cameraShakeDuration;
    Vignette vignette;

    [Header("Stats")]
    public float respawnWaitTime;
    [HideInInspector] public float respawnWaitTimeCountDown;
    public float maxHealth;
    [HideInInspector] public float health;
    public float maxShield;
    [HideInInspector] public float shield;
    
    [Header("Debug")]
    public bool preesFToTest;
    private bool isDead = false;
    private float healthBuffer;
    private float shieldBuffer;
    private float lerpSpeed = 0.05f;

    void Start()
    {
        Init();
        volume.profile.TryGetSettings<Vignette>(out vignette);

        if (!vignette)
        {
            Debug.Log("error, vignette empty");
        }
        else
        {
            vignette.enabled.Override(false);
        }
    }

    void Init()
    {
        respawnWaitTimeCountDown = respawnWaitTime;
        health = maxHealth;
        shield = maxShield;
        healthText.text = health.ToString("F0");
        shieldText.text = shield.ToString("F0");
        healthBuffer = health/maxHealth;
        shieldBuffer = shield/maxShield;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            WaitToRespawn();
        
        if (healthBarFill.fillAmount != healthBuffer)
        {
            healthBarFill.fillAmount = healthBuffer;
            healthText.text = health.ToString("F0");
        }

        if (healthBarFill.fillAmount != healthEaseBarFill.fillAmount)
            healthEaseBarFill.fillAmount = Mathf.Lerp(healthEaseBarFill.fillAmount, healthBuffer, lerpSpeed);

        if (shieldBarFill.fillAmount != shieldBuffer)
        {
            shieldBarFill.fillAmount = shieldBuffer;
            shieldText.text = shield.ToString("F0");
        }
            
        if (shieldBarFill.fillAmount != shieldEaseBarFill.fillAmount)
            shieldEaseBarFill.fillAmount = Mathf.Lerp(shieldEaseBarFill.fillAmount, shieldBuffer, lerpSpeed);

        if (preesFToTest)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TakeDamage(10);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamageFX());
        StartCoroutine(cameraShake.Shake(cameraShakeDuration, cameraShakeMagnitude));

        if (shield > 0)
        {
            shield -= damage;
            shieldBuffer = shield/maxShield;
            if (shield <= 0) 
            {
                shield = 0;
                shieldText.text = shield.ToString("F0");
            }
        }
        else
        {
            health -= damage;
            healthBuffer = health/maxHealth;
            if (health <= 0) 
            {
                health = 0;
                healthText.text = health.ToString("F0");
                WaitToRespawnEvent.Invoke();
                isDead = true;
            }
        }
    }

    private IEnumerator TakeDamageFX()
    {
        if (intensity + 0.4f > 0.5f) intensity = 0.5f;
        intensity += 0.2f;

        vignette.enabled.Override(true);
        vignette.intensity.Override(intensity);

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.01f;

            if (intensity < 0) intensity = 0;

            vignette.intensity.Override(intensity);

            yield return new WaitForSeconds(0.1f);
        }

        vignette.enabled.Override(false);
        yield break;
    }

    public void WaitToRespawn()
    {                    
        if (respawnWaitTimeCountDown <= 0) Respawn();
        else
        {
            respawnWaitTimeCountDown -= Time.deltaTime;
            respawnTimeTextAnounce.text = respawnWaitTimeCountDown.ToString("F0") + " " + "Seconds";
        }
    }

    public void Respawn()
    {
        RespawnEvent.Invoke();
        Init();
        player.ResetPosition(respawnPoint.position);
        playerCam.xRotation = 0;
        playerCam.yRotation = 0;
        isDead = false;
        respawnWaitTimeCountDown = respawnWaitTime;
    }
}
