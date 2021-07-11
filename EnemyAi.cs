using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
	
	public Transform player;
	
	public LayerMask whatIsGround, whatIsPlayer;
	
	// Patroling
	public Vector3 walkPoint;
	bool walkPointSet;
	public float walkPointRange;
	
	// Attacking
	public float timeBetweenAttacks;
	bool alreadyAttacked;
	
	// States
	public float sightRange, attackRange;
	public bool playerInSightRange, playerInAttackRange;
	
	private void Awake()
	{
		player = GameObject.Find("PlayerArmature").transform;
		agent = GetComponent<NavMeshAgent>();
	}
	
	private void Update()
	{
		// Check for Sight and attack range
		playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
		playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
		
		if (!playerInSightRange && !playerInAttackRange) Patroling();
		if (playerInSightRange && !playerInAttackRange) ChasePlayer();
		if (playerInSightRange && playerInAttackRange) AttackPlayer();
	}
	
	// State Functions
	private void Patroling()
	{
		if (!walkPointSet) SearchWalkPoint();
		
		if (walkPointSet)
			agent.SetDestination(walkPoint);
		
		Vector3 distanceToWalkPoint = transform.position - walkPoint;
		
		// Walkpoint Reached
		if (distanceToWalkPoint.magnitude < 1f)
			walkPointSet = false;
		
	}
	
	private void SearchWalkPoint()
	{
		// Calculate random point in range
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
		agent.SetDestination(player.position);
	}
	
	private void AttackPlayer()
	{
		// Make sure enemy doesn't move
		agent.SetDestination(transform.position);
		
		transform.LookAt(player);
		
		if (!alreadyAttacked)
		{
			/// Attack Code
			// Goes Here
			alreadyAttacked = true;
			Invoke(nameof(ResetAttack), timeBetweenAttacks);
		}
	}
	
	private void ResetAttack()
	{
		alreadyAttacked = false;
	}
}

