using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Configure NavMeshAgent for obstacle avoidance
        agent.radius = 0.5f; // Adjust based on your maze
        agent.height = 2.0f; // Adjust based on your maze
        agent.speed = 3.5f; // Adjust based on your desired speed
        agent.acceleration = 8.0f; // Adjust for desired responsiveness
        agent.angularSpeed = 120.0f; // Adjust for desired turning speed
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = 50; // Adjust based on your needs
    }

    private void Update()
    {
        if (player == null)
        {
            // Attempt to find the player if it's not set
            GameObject playerObject = GameObject.Find("TestExample");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        //if(playerInSightRange && playerInAttackRange) AttackPlayer(); //Commenting out bc dont know how to attack yet.
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

        //Calculate distance of walkpoint
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //if distance less than 1 walkpoint has been reached.
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false; //this automatically searchs for a new Search walk point.
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate the random point in Range.
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
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

    /* //Commenting out because we dont know attack yet.
    private void AttackPlayer()
    {
        //Place Attack Code here.

        //this is to make the enemy not move when attacking.
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
