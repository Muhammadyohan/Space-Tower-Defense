using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReptileAlien : MonoBehaviour
{
    private Enemy thisEnemy;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    bool shouldRotateToPlayer = true;
    bool isAttacking = false;
    public EnemyAttack enemyAttack;

    [Header("States")]
    public float attackRange;
    public bool playerInAttackRange;

    void Awake()
    {
        StartCoroutine(GetPlayerObj());
        thisEnemy = GetComponent<Enemy>();
        thisEnemy.Init();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check for sight and attack range
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!isAttacking)
        {
            if (playerInAttackRange) AttackPlayer();
            else ChasePlayer();
        }
    }

    private IEnumerator GetPlayerObj()
    {
        yield return new WaitForSeconds(0.1f);
        try
        {
            player = GameObject.Find("PlayerObj").transform;
        }
        catch
        {
            StartCoroutine(GetPlayerObj());
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
            agent.SetDestination(player.position);
            
        if(!thisEnemy.animator.GetBool("Move"))
        {
            thisEnemy.animator.SetBool("Move", true);
        }
    }

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        if(thisEnemy.animator.GetBool("Move"))
            thisEnemy.animator.SetBool("Move", false);

        if (shouldRotateToPlayer)
        {
            // Rotate enemy to look at the player
            transform.LookAt(new Vector3(player.position.x, 0, player.position.z));
        } 

        if (!alreadyAttacked)
        {
            // Attack code here
            thisEnemy.animator.SetTrigger("Attack");
            isAttacking = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        } 
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void StartAttacking()
    {
        agent.isStopped = true;
        shouldRotateToPlayer = false;
        enemyAttack.hitting = true;
    }

    private void StopAttacking()
    {
        shouldRotateToPlayer = true;
        isAttacking = false;
        enemyAttack.hitting = false;
        enemyAttack.targetAttacked = false;
        agent.isStopped = false;
    }
}
