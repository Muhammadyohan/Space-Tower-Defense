using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public UnityEvent WaitToRespawnEvent;
    public UnityEvent RespawnEvent;
    public PlayerMovement player;
    public PlayerCam playerCam;

    public Transform respawnPoint;

    public Image healthBarFill;
    public Image healthEaseBarFill;
    public Image shieldBarFill;
    public Image shieldEaseBarFill;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI respawnTimeTextAnounce;

    public float respawnWaitTime;
    public float respawnWaitTimeCountDown;
    public float maxHealth;
    public float health;
    public float maxShield;
    public float shield;
    
    public bool preesFToTest;

    private bool isDead = false;

    private float healthBuffer;
    private float shieldBuffer;

    private float lerpSpeed = 0.05f;

    void Start()
    {
        Init();
    }

    void Init()
    {
        respawnWaitTimeCountDown = respawnWaitTime;
        health = maxHealth;
        shield = maxShield;
        healthText.text = health.ToString("F0");
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
            shieldBarFill.fillAmount = shieldBuffer;
            
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
        if (shield > 0)
        {
            shield -= damage;
            shieldBuffer = shield/maxShield;
            if (shield <= 0) 
                shield = 0;
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
