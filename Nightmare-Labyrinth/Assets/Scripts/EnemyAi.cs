using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 0.5f; // Adjusted for smaller environment

    // States
    public float sightRange = 1.5f;  // Adjusted for smaller environment
    public float attackRange = 0.2f; // Adjusted for smaller environment
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Configure NavMeshAgent for obstacle avoidance
        agent.radius = 0.05f; // Adjusted for smaller environment
        agent.height = 0.2f;  // Adjusted for smaller environment
        agent.speed = 0.35f;  // Adjusted for smaller environment
        agent.acceleration = 0.8f;  // Adjusted for smaller environment
        agent.angularSpeed = 12.0f;  // Adjusted for smaller environment
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = 50; // Adjust based on your needs
    }

    private void Update()
    {
        if (player == null)
        {
            // Attempt to find the player if it's not set
            GameObject playerObject = GameObject.Find("PlayerObj");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        //if(playerInSightRange && playerInAttackRange) AttackPlayer(); //Commented out as attack isn't implemented yet
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(walkPoint);
            }
        }

        // Calculate distance to walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // If distance less than 0.1 (for smaller environment) walkpoint has been reached
        if (distanceToWalkPoint.magnitude < 0.1f)
        {
            walkPointSet = false; // Automatically search for a new walk point
        }
    }

    private void SearchWalkPoint()
    {
        // Calculate the random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Check if the point is on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 0.2f, whatIsGround)) // Adjusted for smaller environment
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }

    /* 
    //Commenting out because we don't have an attack implementation yet.
    private void AttackPlayer()
    {
        //Place Attack Code here.

        //This is to make the enemy not move when attacking.
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    */
}
