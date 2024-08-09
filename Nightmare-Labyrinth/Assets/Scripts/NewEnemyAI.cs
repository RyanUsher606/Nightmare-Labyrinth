using UnityEngine;
using UnityEngine.AI;

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private NavMeshAgent navMeshAgent; // Reference to the NavMeshAgent component

    public float detectionRadius = 15f; // Radius within which the enemy detects and seeks the player
    public float stoppingDistance = 2f; // Distance from the player where the enemy stops moving
    public float chargeSpeed = 10f; // Speed of the enemy when charging the player

    private void Start()
    {
        // Find the player object in the scene by name "PlayerObj"
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("PlayerObj found: " + player.name);
            }
            else
            {
                Debug.LogError("PlayerObj not found!");
            }
        }

        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        else
        {
            navMeshAgent.stoppingDistance = stoppingDistance; // Set the stopping distance
        }
    }

    private void Update()
    {
        // If player is within detection radius, charge towards the player
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            // Increase speed for charging
            navMeshAgent.speed = chargeSpeed;
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            // Optionally, stop moving if the player is out of range
            navMeshAgent.ResetPath();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a red sphere to visualize the detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
