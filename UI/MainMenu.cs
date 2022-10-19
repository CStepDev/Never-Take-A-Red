using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	void Update ()
	{
		if (Input.GetKeyDown ("joystick button 7")) 
		{
			SceneManager.LoadScene ("Level 1 City");
		}

		else if (Input.GetKeyDown (KeyCode.Space)) 
		{
			SceneManager.LoadScene ("Level 1 City");
		}
	}
}
