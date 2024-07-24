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

    /*Attacking
    public float timeBetweenAttcaks
    bool alreadyAttacked
    */

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

}
