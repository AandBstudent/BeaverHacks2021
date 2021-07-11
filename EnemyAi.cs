using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public float lookRadius = 10f;
	public float walkRange = 10f;
	public Vector3 walkPoint;
	
	Transform target;
	NavMeshAgent agent;
	
    // Start is called before the first frame update
    void Start()
    {
		target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
		
		if (distance <= lookRadius)
		{
			agent.SetDestination(target.position);
		}
		else
		{
			float randomMoveZ = Random.Range(-walkRange,walkRange);
			float randomMoveX = Random.Range(-walkRange,walkRange);
			
			walkPoint = new Vector3(transform.position.x + randomMoveX, transform.position.y, transform.position.z + randomMoveZ);
			
			agent.SetDestination(walkPoint);
		}
    }
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}

