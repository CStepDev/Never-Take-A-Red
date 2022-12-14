using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour 
{

	public static bool GameIsPaused = false;

	public GameObject pauseMenuUI;

	public GameObject disableHUD;


	// Checking to see if the Escape key or the Start button 
	//on the controller is pressed to pause and unpause the game
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (GameIsPaused) 
			{
				Resume ();
			} 
			else 
			{
				Pause ();
			}
		} 

		else if (Input.GetKeyDown ("joystick button 7")) 
		{
			
				if (GameIsPaused) 
				{
					Resume ();
				}
				else 
				{
					Pause ();
				}
		}
	}




	public void Resume()
	{
		pauseMenuUI.SetActive (false);
		disableHUD.SetActive (true);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	void Pause()
	{
		pauseMenuUI.SetActive (true);
		disableHUD.SetActive (false);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}
		

	
	public void QuitGame()
	{
		Debug.Log("Quitting Game...");
		Application.Quit ();
	}
}
