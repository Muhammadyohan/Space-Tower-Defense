using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GreenAlien : MonoBehaviour
{
    private Enemy thisEnemy;
    public NavMeshAgent agent;
    public Transform player, thisColiiderTransform;
    public LayerMask whatIsGround, whatIsPlayer, whatIsDefendedObject;

    [Header("Go to Target Object")]
    public Transform targetPoint;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    bool shouldRotateToPlayer = true;
    bool isAttacking = false;
    public EnemyAttack enemyAttack;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, targetInAttackRange;

    void Awake()
    {
        thisEnemy = GetComponent<Enemy>();
        thisEnemy.Init();
        player = GameObject.Find("PlayerObj").transform;
        targetPoint = GameObject.Find("Shield Core").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsDefendedObject);

        if (!isAttacking)
        {
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            else if (playerInAttackRange && playerInSightRange) AttackPlayer();
            else if (targetInAttackRange) AttackTargetObject();
            else if (!playerInSightRange && !playerInAttackRange) GoToTargetObject();
        }
    }

    private void GoToTargetObject()
    {
        if (!targetInAttackRange)
        {
            agent.SetDestination(targetPoint.position);
            if(!thisEnemy.animator.GetBool("Move"))
            {
                thisEnemy.animator.SetBool("Move", true);
            }
        }
    }

    private void ChasePlayer()
    {
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

    private void AttackTargetObject()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        if(thisEnemy.animator.GetBool("Move"))
            thisEnemy.animator.SetBool("Move", false);

        if (shouldRotateToPlayer)
        {
            // Rotate enemy to look at the player
            transform.LookAt(new Vector3(targetPoint.position.x, 0, targetPoint.position.z));
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
        shouldRotateToPlayer = false;
        enemyAttack.hitting = true;
        agent.isStopped = true;
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
