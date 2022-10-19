// Author  : Curtis Stephenson
// Date    : 21/10/2017
// Purpose : The CameraAlign script is used to ensure the camera is always aligned correctly with the character during gameplay. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CameraAlign : MonoBehaviour 
{

	// This will hold a reference to the player, so we can keep track of where they are in order to align the camera
	public GameObject player;


	// We're using LateUpdate here to be a little bit fancy. If you want to know more, take a look through the Unity scripting stuff, it'll probably
	// let you know of some minor advantages of doing it this way.
	private void LateUpdate()
	{
		this.transform.position = new Vector3(player.transform.position.x, (player.transform.position.y) + 15f, (player.transform.position.z) - 12f);
		this.transform.rotation = Quaternion.Euler (50, 0, 0);
	}
}
