using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToDefense : MonoBehaviour
{
    public Image healthBarFill;
    public Image easeBarFill;

    public float maxHealth;
    public float health;
    public TextMeshProUGUI healthText;
    public bool preesSpaceBarToTest;
    private float healthBuffer;
    private float lerpSpeed = 0.05f;
    private GameOverOrCompleteHandle gm;

    void Start()
    {
        health = maxHealth;
        healthText.text = health.ToString("F0") + "/" + maxHealth.ToString("F0");
        healthBuffer = health/maxHealth;
        gm = FindObjectOfType<GameOverOrCompleteHandle>();
    }

    void Update()
    {
        if (healthBarFill.fillAmount != healthBuffer)
        {
            healthBarFill.fillAmount = healthBuffer;
            healthText.text = health.ToString("F0") + "/" + maxHealth.ToString("F0");

        }

        if (preesSpaceBarToTest)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(10);
            }
        }

        if (healthBarFill.fillAmount != easeBarFill.fillAmount)
        {
            easeBarFill.fillAmount = Mathf.Lerp(easeBarFill.fillAmount, healthBuffer, lerpSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBuffer = health/maxHealth;
        if (health <= 0) 
        {
            health = 0;
            healthText.text = health.ToString("F0") + "/" + maxHealth.ToString("F0");
            gm.GameOver();
        }
    }
}
