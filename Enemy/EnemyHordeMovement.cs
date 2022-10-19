// Author  : Curtis Stephenson
// Date    : 19/03/2018
// Purpose : This script manages the moving behavior of the 'Horde' enemy, and is separate from the 'Area' enemy on the basis of potential tweaks
//           and modifications we may not want to affect the pair of enemies with in the future. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHordeMovement : MonoBehaviour 
{
	Transform player; // Holds the players transform in a reference
	PlayerCharacterHealth playerHealth; // Holds the players health in a reference
	EnemyHealth enemyHealth; // Holds the current enemy's health in a reference
	UnityEngine.AI.NavMeshAgent nav; // A reference to the navmesh agent component for horde enemy movement


	// Variable setups upon awakening, grabbing needed references for correct operation
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth = player.GetComponent <PlayerCharacterHealth> ();
		enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
	}

	// The update function just ensures the horde enemy will seek out the player if
	// both of their health values are higher than zero, otherwise it'll stop bothering
	// to get updates from the NavMeshAgent component
	void Update ()
	{
		if (((this.transform.position - player.transform.position).magnitude * 1.0f) > 75.0f)
		{
			nav.speed = 15.0f;
		}
		else
		{
			nav.speed = 5.0f;
		}


		if((enemyHealth.GetCurrentHealth() > 0) && (playerHealth.CurrentHealth > 0))
		{
			nav.SetDestination (player.position);
		}
	}
}