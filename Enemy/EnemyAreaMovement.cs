// Author  : Curtis Stephenson
// Date    : 16/05/2018
// Purpose : This script will handle the various 'states' and actions carried out by the area enemy in
//           the game. This could have been handled in more broken down scripts and deriving classes and
//           singletons, but due to time and the tools at our disposal from Unity itself we're going for
//           a far simpler approach.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaMovement : MonoBehaviour 
{
	GameObject player; // Player reference 
	UnityEngine.AI.NavMeshAgent nav; // A reference to the navmesh agent component for the area enemy

	Vector3 lastSeenPlayer = new Vector3 (0.0f, 0.0f, 0.0f); // The last seen location of the player
	int currentPointPatrollingTo = 0; // The point of the array the enemy is patrolling to

	enum AreaEnemyStates{PATROL, SEEKPLAYER, WANDER}; // The states the area enemy can be in
	AreaEnemyStates currentState = AreaEnemyStates.PATROL; // Initial state will always be patrolling

	public Transform[] patrolPoints; // An array of points the specific area enemy will go between while in the patrol state

	float currentWanderTime = 0.0f; // The current amount of time the area enemy will wander for
	const float maxWanderTime = 10.0f; // The amount of time the area enemy will wander before resetting to Patrol mode
	float currentWanderPathTime = 0.0f; // The amount of time before the enemy will select a new location to wander to
	const float maxWanderPathTime = 2.5f; // The maximum amount of time it'll take before the enemy picks a new wandering target


	// This is a lazy way to see if the player is currently inside of the viewing range of the area zombie, shifting
	// the state lazily/badly perhaps
	void OnTriggerStay (Collider other)
	{
		if(other.gameObject == player)
		{
			currentState = AreaEnemyStates.SEEKPLAYER;
		}
	}

	// This is a lazy way to see if the player has exited the viewing range of the area zombie, shifting
	// the state lazily/badly perhaps
	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player)
		{
			lastSeenPlayer = player.transform.position;
			currentWanderTime = maxWanderTime;
			currentWanderPathTime = maxWanderPathTime;
			currentState = AreaEnemyStates.WANDER;
		}
	}


	// Patrol will effectively walk the area enemy between a set of points passed in through the patrolPoints array and
	// not do much beyond that, but will always 
	void Patrol()
	{
		nav.SetDestination (patrolPoints [currentPointPatrollingTo].transform.position);

		if ((this.transform.position - patrolPoints[currentPointPatrollingTo].transform.position).magnitude < 5.0f)
		{
			currentPointPatrollingTo++;
		}

		if (currentPointPatrollingTo > patrolPoints.Length - 1)
		{
			currentPointPatrollingTo = 0;
		}
	}

	void SeekPlayer()
	{
		nav.SetDestination (player.transform.position);
	}

	void Wander()
	{
		nav.SetDestination (lastSeenPlayer);

		currentWanderTime -= (1.0f * Time.deltaTime);
		currentWanderPathTime -= (1.0f * Time.deltaTime);

		if (currentWanderPathTime < 0.0f)
		{
			lastSeenPlayer = lastSeenPlayer + new Vector3 (Random.Range (-10.0f, 10.0f), 0.0f, Random.Range (-10.0f, 10.0f));
			currentWanderPathTime = maxWanderPathTime;
		}

		if (currentWanderTime < 0.0f)
		{
			currentState = AreaEnemyStates.PATROL;
		}
	}

	// This function was intended to hold the attacking behaviour for the area enemy, but was reconsidered when I
	// realized I couldn't attach two trigger points to the same object, the solution to all of it being that I just
	// reused the regular attacking script and attached it to a different part of the overall area zombie object
	//void Attack()
	//{
		// Unused
	//}

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (currentState)
		{
		case AreaEnemyStates.PATROL:
			Patrol ();
			break;

		case AreaEnemyStates.SEEKPLAYER:
			SeekPlayer ();
			break;

		case AreaEnemyStates.WANDER:
			Wander ();
			break;
		}
	}
}
