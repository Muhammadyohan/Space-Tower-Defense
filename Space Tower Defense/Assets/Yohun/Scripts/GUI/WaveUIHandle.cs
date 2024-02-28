using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIHandle : MonoBehaviour
{
    public Image timeBarFill;
    public float maxTimeBarValue;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI preWaveText;
    private int wave = 1;
    private WaveSpawner waveSpawner;

    private float perCentValForCalculate;

    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        waveText.text = wave.ToString() + "/" + waveSpawner.waves.Length;
        maxTimeBarValue = waveSpawner.waves[waveSpawner.currentWave - 1].timeDuringWave;
    }

    void Update()
    {
        if (wave != waveSpawner.currentWave)
        {
            wave = waveSpawner.currentWave;
            if (wave == waveSpawner.waves.Length)
            {
                preWaveText.text = "FINAL WAVE";
                waveText.text = "";
            }
            else
                waveText.text = wave.ToString() + "/" + waveSpawner.waves.Length;
        }
        
        perCentValForCalculate = waveSpawner.timeCountdown/maxTimeBarValue;
        if (timeBarFill.fillAmount != perCentValForCalculate)
        {
            timeBarFill.fillAmount = perCentValForCalculate;
        }
    }
}
