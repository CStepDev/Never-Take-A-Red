using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaHealth : MonoBehaviour 
{
	private int health = 1; // The actual health value that we'll use and modify in outside scripts, set to one so they don't die when first generated as inactive by the pool
	public int startHealth = 100; // Used to reset the health values of enemies when they are reused by the pooling system instead of being deleted upon death

	PlayerCharacterManager playerManagerReference; // Needed to update the score when the enemy dies

	// We're just going to use a function to return the private health value, rather than make it public
	// and potentially allow things to change it when they shouldn't be
	public int GetCurrentHealth()
	{
		return health;
	}


	// This function will allow for damage to be taken, just so nothing outside of the actual TakeDamage
	// function can mess with the enemy health values in the game
	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	// A quick setup of the actual health variable we'll use for the enemy
	void Start () 
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();
	}


	// Sets the object to inactive if it's ran out of health, rather than destroying it outright
	void Update()
	{
		if (health <= 0)
		{
			playerManagerReference.AddScore (1000);
			this.gameObject.SetActive (false);
		}
	}
}
