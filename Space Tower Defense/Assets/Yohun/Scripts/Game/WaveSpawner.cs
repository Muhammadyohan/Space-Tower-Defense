using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WATING, COUNTING, GAMEEND}

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int amount;
        public float spawnRate;
        public float timeDuringWave;
        public Transform[] spawnPoints;
        public SubWave[] subWaves; 
    }
    [System.Serializable]
    public class SubWave
    {
        public string name;
        public Transform enemy;
        public int amount;
        public Transform[] spawnPoint;
        public float spawnRate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    [HideInInspector] public int currentWave = 1;
    
    public float intermissionTime = 10f;
    public float timeBetweenWaves = 5f;
    [HideInInspector] public float waveCountdown = 0f;
    [HideInInspector] public float timeCountdown = 0f;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    private WaveUIHandle waveUIHandle;
    private GameOverOrCompleteHandle gm;

    void Start()
    {   
        Physics.IgnoreLayerCollision(8, 8, true);
        waveCountdown = intermissionTime;
        timeCountdown = waves[currentWave -1].timeDuringWave;
        waveUIHandle = FindObjectOfType<WaveUIHandle>();
        gm = FindObjectOfType<GameOverOrCompleteHandle>();
    }

    void Update()
    {
        if (state == SpawnState.SPAWNING && currentWave != waves.Length)
            timeCountdown -= Time.deltaTime;


        if (state == SpawnState.WATING)
        {
            if (!EnemyIsAlive() || timeCountdown <= 0)
            {
                // Begin a new round
                WaveCompleted();
                return;
            }
            else
            {
                if (currentWave != waves.Length)
                    timeCountdown -= Time.deltaTime;
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                currentWave = nextWave + 1;
                timeCountdown = waves[nextWave].timeDuringWave;
                waveUIHandle.maxTimeBarValue = waves[currentWave - 1].timeDuringWave;
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
            // nextWave = 0;
            // Debug.Log ("ALL WAVES COMPLETE! Looping..");
            state = SpawnState.GAMEEND;
            gm.GameComplete();

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

        for (int i = 0; i < _wave.amount; i++)
        {
            SpawnEnemy(_wave.enemy, _wave.spawnPoints[Random.Range(0, _wave.spawnPoints.Length)].position);
            yield return new WaitForSeconds(_wave.spawnRate);
        }

        state = SpawnState.WATING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy, Vector3 spawnPoint)
    {
        Instantiate(_enemy, spawnPoint, Quaternion.identity);
    }
}
