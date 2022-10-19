// Author  : Curtis Stephenson
// Date    : 21/02/2018
// Purpose : This is a shared script between all enemies for their health variable. This is also where I eat
//           my own words for calling it a silly solution when I read it online, but in practice it seems about 
//           as smart as I could do it without spending too much time thinking about it.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour 
{
	private int health = 1; // The actual health value that we'll use and modify in outside scripts, set to one so they don't die when first generated as inactive by the pool
	public int startHealth = 100; // Used to reset the health values of enemies when they are reused by the pooling system instead of being deleted upon death

	//bool isDead = false;

	PlayerCharacterManager playerManagerReference; // Needed to update the score when the enemy dies

	// A utility function to check if the the enemy associated with this enemy health script is actually
	// considered dead
	//public bool IsEnemyDead()
	//{
		//return isDead;
	//}

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


	// Once the EnemySpawner/EnemyPool style setup is complete, this function will be used to set back up
	// an inactive enemy so we don't just keep making new ones and destroying them, which would be more
	// work than just keeping them around in memory until a scene switch
	public void ReloadEnemy()
	{
		health = startHealth;
		this.gameObject.SetActive(true);
	}


	// A quick setup of the actual health variable we'll use for the enemy
	void Start () 
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();
		this.gameObject.SetActive (false);
		//ReloadEnemy ();
	}


	// Sets the object to inactive if it's ran out of health, rather than destroying it outright
	void Update()
	{
		if (health <= 0)
		{
			playerManagerReference.AddScore (100);
			this.gameObject.SetActive (false);
		}
	}
}
