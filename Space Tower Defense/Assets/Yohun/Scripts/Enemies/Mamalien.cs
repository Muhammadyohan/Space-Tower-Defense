using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Mamalien : MonoBehaviour
{   
    private Enemy thisEnemy;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, whatIsDefendedObject;

    [Header("Go to Target Object")]
    public Transform targetPoint;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    bool shouldRotate = true;
    bool isAttacking = false;
    public MamalienAttack enemyAttack;
    public UnityEvent StartFlameThrowEvent;
    public UnityEvent StopFlameThrowEvent;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, targetInAttackRange;

    void Awake()
    {
        StartCoroutine(GetPlayerObj());
        thisEnemy = GetComponent<Enemy>();
        thisEnemy.Init();
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

    private void GoToTargetObject()
    {
        if (!targetInAttackRange)
        {
            if (agent.enabled) agent.SetDestination(targetPoint.position);
            if(!thisEnemy.animator.GetBool("Move"))
            {
                thisEnemy.animator.SetBool("Move", true);
            }
        }
    }

    private void ChasePlayer()
    {
        if (agent.enabled) agent.SetDestination(player.position);
        if(!thisEnemy.animator.GetBool("Move"))
        {
            thisEnemy.animator.SetBool("Move", true);
        }
    }

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        if (agent.enabled) agent.SetDestination(transform.position);
        if(thisEnemy.animator.GetBool("Move"))
            thisEnemy.animator.SetBool("Move", false);

        if (shouldRotate)
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
        if (agent.enabled) agent.SetDestination(transform.position);
        if(thisEnemy.animator.GetBool("Move"))
            thisEnemy.animator.SetBool("Move", false);

        if (shouldRotate)
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
        agent.isStopped = true;
        shouldRotate = false;
        enemyAttack.hitting = true;
    }

    private void StopAttacking()
    {
        shouldRotate = true;
        isAttacking = false;
        enemyAttack.hitting = false;
        enemyAttack.targetAttacked = false;
        agent.isStopped = false;
    }

    private void InvokeStartFlameThrowEvent()
    {
        StartFlameThrowEvent.Invoke();
    }

    private void InvokeStopFlameThrowEvent()
    {
        StopFlameThrowEvent.Invoke();
    }
}
