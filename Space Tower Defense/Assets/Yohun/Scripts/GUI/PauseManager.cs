using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    public UnityEvent PauseEvent;
    public UnityEvent ResumeEvent;

    bool isPause;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPause)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        isPause = true;
        StartCoroutine(PauseEnemy());
        Time.timeScale = 0;
        PauseEvent.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPause = false;
        StartCoroutine(ResumeEnemy());
        ResumeEvent.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    IEnumerator PauseEnemy()
    {
        EnemyAttack[] enemyAttacks = FindObjectsOfType<EnemyAttack>();
        MamalienAttack[] mamalienAttacks = FindObjectsOfType<MamalienAttack>();
        NavMeshAgent[] navMeshAgents = FindObjectsOfType<NavMeshAgent>();

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            enemyAttacks[i].enabled = false;
        }
        for (int i = 0; i < mamalienAttacks.Length; i++)
        {
            mamalienAttacks[i].enabled = false;
        }
        for (int i = 0; i < navMeshAgents.Length; i++)
        {
            navMeshAgents[i].enabled = false;
        }

        yield return null;
    }

    IEnumerator ResumeEnemy()
    {
        EnemyAttack[] enemyAttacks = FindObjectsOfType<EnemyAttack>();
        MamalienAttack[] mamalienAttacks = FindObjectsOfType<MamalienAttack>();
        NavMeshAgent[] navMeshAgents = FindObjectsOfType<NavMeshAgent>();

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            enemyAttacks[i].enabled = true;
        }
        for (int i = 0; i < mamalienAttacks.Length; i++)
        {
            mamalienAttacks[i].enabled = true;
        }
        for (int i = 0; i < navMeshAgents.Length; i++)
        {
            navMeshAgents[i].enabled = true;
        }

        yield return null;
    }
}
