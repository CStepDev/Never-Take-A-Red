// Author  : Curtis Stephenson
// Date    : 15/05/2018
// Purpose : The EnemyPool script handles the instantiating, relocating and spawning of enemies within the current level. This is done to avoid the use of 'new' and 'destroy', since we'll always
//           need to continue generating horde enemies so long as a level is being played, so making new ones every time we want a new one and destroying/deleting them every time they die is more
//           wasteful than creating a pool of enemies to sit in memory for as long as a level is running. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour 
{
	const float spawnCooldown = 3.0f; // The maximum time it'll take for an enemy to spawn
	float localCooldownTimer = spawnCooldown; // The local cooldown of how long spawning an enemy should take

	GameObject player; // Player reference
	PlayerCharacterHealth playerHealth; // The health component reference


	public int minHordeEnemies = 10;	// The minimum amount of enemies the manager will try to uphold
	public int maxHordeEnemies = 35; // The maximum amount of enemies the manager will spawn before stopping

	public GameObject hordeEnemyPrefab; // Reference to a hordeEnemy to copy from

	public List<GameObject>hordeEnemyPool = new List<GameObject>(); // A pool of hordeEnemies for the manager to use instead of creating and destroying

	public Transform[] spawnPoints; // An array of spawnpoints for horde enemies on a given level

	// This function instantiates a new horde enemy and adds it to our list, making it usable for the rest of the functions within this script
	void InstantiateHordeEnemy()
	{
		hordeEnemyPool.Add ((GameObject)Instantiate(hordeEnemyPrefab));
	} // InstantiateHordeEnemy()

	// This function is used to move an enemy while it's inactive within the scene, effectively moving it to a new starting location as if it
	// had just spawned, rather than just being reused within the system
	void RelocateHordeEnemy(GameObject hordeEnemyToRelocate)
	{
		Vector3 furthestRelocationPoint = new Vector3 (0.0f, 0.0f, 0.0f);

		foreach (Transform relocationPoint in spawnPoints)
		{
			if (((player.transform.position - relocationPoint.transform.position).magnitude * 1.0f) > ((player.transform.position - furthestRelocationPoint).magnitude * 1.0f))
			{
				furthestRelocationPoint = relocationPoint.transform.position;
			}
		}

		hordeEnemyToRelocate.transform.position = furthestRelocationPoint;
	} // RelocateHordeEnemy (GameObject hordeEnemyToRelocate)

	// This function is where everything is chosen to be spawned from, such as calling relocate when neccesary and reactivating inactive enemies
	void SpawnHordeEnemy ()
	{
		if(playerHealth.CurrentHealth <= 0f)
		{
			return;
		}
		else
		{
			bool spawnedEnemy = false;

			foreach(GameObject hordeEnemyInList in hordeEnemyPool)
			{
				if ((!hordeEnemyInList.activeSelf) && (!spawnedEnemy))
				{
					RelocateHordeEnemy (hordeEnemyInList);
					hordeEnemyInList.SetActive (true);
					hordeEnemyInList.GetComponent<EnemyHealth> ().ReloadEnemy ();
					spawnedEnemy = true;
				}
			}

			if ((!spawnedEnemy) && (hordeEnemyPool.Count < maxHordeEnemies))
			{
				InstantiateHordeEnemy ();
				RelocateHordeEnemy(hordeEnemyPool[hordeEnemyPool.Count - 1].gameObject);
				hordeEnemyPool[hordeEnemyPool.Count - 1].gameObject.SetActive (true);
				hordeEnemyPool [hordeEnemyPool.Count - 1].gameObject.GetComponent<EnemyHealth> ().ReloadEnemy ();
			}
		}
	} // SpawnHordeEnemy()

	// The start function sets the relevant references and instantiates the minimum amount of horde enemies
	// we want in the current level, since there should never be less than this in typical gameplay it's safe
	// to assume we just want to have them made on starting this function
	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerCharacterHealth> ();

		for (int i = 0; i < minHordeEnemies; i++)
		{
			InstantiateHordeEnemy ();
		}
	} // Start()

	// Update is used to spawn another enemy when the time is correct
	void Update()
	{
		localCooldownTimer -= (1.0f * Time.deltaTime);

		if (localCooldownTimer < 0.0f)
		{
			SpawnHordeEnemy ();
			localCooldownTimer = spawnCooldown;
		}
	} // Update()
}
