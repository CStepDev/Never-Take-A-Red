// Author  : Curtis Stephenson
// Date    : 19/03/2018
// Purpose : This script manages the attacking behavior of the 'Horde' enemy, and is separate from the 'Area' enemy on the basis of potential tweaks
//           and modifications we may not want to affect the pair of enemies with in the future. 

using UnityEngine;
using System.Collections;

public class EnemyHordeAttack : MonoBehaviour
{
	public float timeBetweenAttacks = 0.5f; // The time between swings the enemy will be able to make
	public float attackDamage = 10.0f; // The damage each successful hit will deliver to the player

	GameObject player; // Player reference
	PlayerCharacterHealth playerHealth; // The health component reference
	EnemyHealth enemyHealth; // Enemy health reference
	bool playerInRange; // A check for if the player is close enough to attack
	float timer; // Timer to check for if the enemy can attack again


	// We're going to use Awake because I wanted to be a bit different. The magic here that'll be great for the EnemySpawner/EnemyPool setup is that
	// the references can be kept as long as the scene is active. The enemy can die/set inactive, and when they return they'll still only need the
	// exact same references they had from this function, as the player, playerHealth and enemyHealth will never change
	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerCharacterHealth> ();
		enemyHealth = GetComponent<EnemyHealth>();
	}


	// This checks a touching collider upon entering to see if it's the player or not, and if it is, it'll make sure to alert the script that the
	// player is within range for an attack
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{
			//AkSoundEngine.PostEvent ("HordeAttacking", gameObject);
			playerInRange = true;
		}
	}


	// Once the player gets out of the range of the enemy, the enemy will no longer be able to attack them, so the boolean is set back appropriately
	void OnTriggerExit (Collider other)
	{
		if(other.gameObject == player)
		{
			playerInRange = false;
		}
	}


	// The update function adds to the timer, which is then used in the conditions for the enemy to be able to attack, for which it must be in range and
	// have health in addtion to the time between attacks being less than the timer being kept
	void Update ()
	{
		timer += Time.deltaTime;

		if((timer >= timeBetweenAttacks) && (enemyHealth.GetCurrentHealth() > 0) && (playerInRange))
		{
			Attack ();
		}
	}


	void Attack ()
	{
		timer = 0f;

		if(playerHealth.CurrentHealth > 0)
		{
			playerHealth.DealDamage (attackDamage);
		}
	}
}