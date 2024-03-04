using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    private Enemy thisEnemy;
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsDefendedObject;

    [Header("Go to Target Object")]
    public Transform targetPoint;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    bool shouldRotate = true;
    bool isAttacking = false;
    public EnemyAttack enemyAttack;

    [Header("States")]
    public float attackRange;
    public bool targetInAttackRange;

    void Awake()
    {
        thisEnemy = GetComponent<Enemy>();
        thisEnemy.Init();
        targetPoint = GameObject.Find("Shield Core").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check for sight and attack range
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsDefendedObject);

        if (!isAttacking)
        {
            if (targetInAttackRange) AttackTargetObject();
            else GoToTargetObject();
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

    private void AttackTargetObject()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);
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
}
