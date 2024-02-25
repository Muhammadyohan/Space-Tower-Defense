using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WATING, COUNTING}

    [System.Serializable]
    public class Wave
    {
        public string name;
        public int enemyID;
        public int count;
        public float spawnDelay;
    }

    public Wave[] waves;
    private int nextWave = 0;
    
    public float timeBetweenWaves = 5f;
    public float waveCountdown = 0f;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {   
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.WATING)
        {
            if (!EnemyIsAlive())
            {
                // Begin a new round
                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        { 
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log ("ALL WAVES COMPLETE! Looping..");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        Debug.Log("Spawning Wave");
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            GameLoopManager.EnqueueEnemyIDToSummon(_wave.enemyID);
            yield return new WaitForSeconds(_wave.spawnDelay);
        }

        state = SpawnState.WATING;

        yield break;
    }
}
