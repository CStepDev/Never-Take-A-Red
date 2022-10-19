// Author  : Curtis Stephenson
// Date    : 15/05/2018
// Purpose : This script handles the level changing object found within each level, only allowing the level to
//           progress to the next if it is allowed by the condition of having enough keys

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour 
{
	PlayerCharacterManager playerManagerReference; // A variable used to hold a reference to the player manager
	public string sceneName; // The variable to hold which scene name we're going to transition to, used to 

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player")
		{
			if (playerManagerReference.GetKeyCount() == 3)
			{
				SceneManager.LoadScene (sceneName);
			}
			else
			{
				// INSERT CODE HERE TO DICTATE WHAT HAPPENS WHEN THE PLAYER DOESN'T HAVE ENOUGH KEYS!
			}
		}
	} // OnTriggerStay(Collider other)


	// Grabs the reference to the player manager in the scene for the number of keys the player posesses
	void Start () 
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();
	}
}
