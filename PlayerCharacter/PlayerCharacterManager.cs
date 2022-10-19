// Author  : Curtis Stephenson
// Date    : 02/02/2018
// Purpose : The PlayerCharacterManager handles a lot of the more meta and player specific information about the current
//           such as 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// I've suddenly realized I don't want to be clever with the use of ints as weapons and pickups, I want to be
// smart and use an enum. It might be a little more complicated to wrap your head around at first, but enums ARE
// more sensible, useful and readible in the long run, I promise you that much. You should be able to use these in
// ANY other script as well, so scope issues shouldn't really be present
public enum Weapons{HANDGUN, SHOTGUN, MACHINEGUN, RAILGUN, MAX_WEAPONS};
public enum Pickups{NONE, SPEED, DAMAGEBOOST, HEALTHSMALL, HEALTHLARGE, MAX_PICKUPS};


public class PlayerCharacterManager : MonoBehaviour 
{
	// Here's our public reference to the player, so we can use them for any operations or functions related to them
	// specifically.
	public GameObject player;

	public GameObject playerWeapon; // A reference to the weapon the player is holding in their hand, mostly so we can adjust damage values.

	// These const declarations are used on player spawns to ensure that all values are correctly assigned and cannot be
	// messed with at runtime. You may think of it like the rules of the game, like the player can only spawn with 
	// a certain amount of health, or always starts with the handgun
	const Weapons defaultWep = Weapons.HANDGUN;


	// The following variables are the actual variables to store the current status of the player's character, with things
	// like health and what weapon they're currently holding and if they're dead
	bool playerIsDead = true;
	Weapons currentWep = Weapons.HANDGUN;
	int currentWepAmmo = 0;
	Pickups currentPick = Pickups.NONE;
	float currentPickTimeRemaining = 0;

    // The current score the player possesses, this should be moved elsewhere ideally but we may as well be lazy given how
    // much time is remaining
	int currentScore = 0;

    // The current amount of keys the player possesses, this should be moved elsewhere ideally but we may as well be lazy
    // given how much time is remaining
    int keyCount = 0;

	// Here's some cheats in a temporary form, until I refactor them into some other place or form. This shouldn't end up
	// breaking anyone else's code, since I should be the only one mapping cheats to things regardless
	bool cheatInvul = false;
	bool cheatSpeed = false;
	bool cheatMachinegun = false;
	bool cheatShotgun = false;


	// A utility function for seeing the current weapon the player is holding, which is required due to the core player
	// variables being private
	public Weapons GetWep()
	{
		return currentWep;
	}


	// A utility function for seeing the current pickup the player has, if any, which is required due to the core player
	// variables being private
	public Pickups GetPick()
	{
		return currentPick;
	}

	// A utility function for returning the current amount of ammo the player is carrying for their equipped weapon
	public int GetCurrentAmmo()
	{
		return currentWepAmmo;
	}

	// A utility function for returning the current score the player has at the time of calling
	public int GetScore()
	{
		return currentScore;
	}

	// A utility function for returning how many key cards the player is currently carrying
    public int GetKeyCount()
    {
        return keyCount;
    }
		
	// A utility function for setting the current weapon, which should be used to change the weapon rather than setting
	// it directly due to bounds checking within the function
	public void SetWep(Weapons newEquippedWeapon, int ammo)
	{
		currentWep = newEquippedWeapon;
		currentWepAmmo = ammo;
	}

	// A function for setting the current pickup and taking appropriate actions depending on whether or not it's an instant
	// effect of it if lasts for a period of time before vanishing
	public void SetPick(Pickups newEquippedPickup)
	{
		if (newEquippedPickup == Pickups.HEALTHSMALL)
		{
			player.GetComponent<PlayerCharacterHealth>().GiveHealth(25.0f);
			newEquippedPickup = Pickups.NONE;
		}
		else if (newEquippedPickup == Pickups.HEALTHLARGE)
		{
			player.GetComponent<PlayerCharacterHealth>().GiveHealth(50.0f);
			newEquippedPickup = Pickups.NONE;
		}
		else if (newEquippedPickup == Pickups.SPEED)
		{
			player.GetComponent<PlayerCharacterController> ().characterSpeed = 1500.0f;
		}
		else if (newEquippedPickup == Pickups.DAMAGEBOOST)
		{
			playerWeapon.GetComponent<PlayerWeapon> ().modifier = 2;
		}

		if (newEquippedPickup != Pickups.NONE)
		{
			currentPickTimeRemaining = 10.0f;
			currentPick = newEquippedPickup;
		}
	}

	// A utility function for adding score to the currentScore variable, which tracks the score of the player in the
	// current session
	public void AddScore(int addScore)
	{
		currentScore += addScore;
	}

	// A utility function for adding a value to the keyCount variable, which tracks how many keys the player currently
	// has in their possession
    public void AddKey()
    {
        keyCount++;
        //Debug.Log("Key Incremented : " + keyCount.ToString());
    }

	public void UseBullet()
	{
		if (currentWepAmmo > 0)
		{
			currentWepAmmo--;
		}
	}

	// A utility function for resetting the current weapon, and should be used rather than setting it directly
	void ClearWep()
	{
		currentWep = Weapons.HANDGUN;
	}

	// A utility function for resetting the current pickup back to none and reversing whatever effects it would
	// have had throughout the code.
	void ClearPick()
	{
		if (currentPick == Pickups.SPEED)
		{
			player.GetComponent<PlayerCharacterController> ().characterSpeed = 750.0f;
		}
		else if (currentPick == Pickups.DAMAGEBOOST)
		{
			playerWeapon.GetComponent<PlayerWeapon> ().modifier = 1;
		}

		currentPick = Pickups.NONE;
	}


	// A utility function for resetting all of the values associated with 
	void ResetAll()
	{
		ClearPick ();

		playerIsDead = false;
	}


	// Start is used to ensure all variables are default values when the character object instance spawns. They are set
	// to default values further up in the script, I know, but having this extra line of code to ensure everything is 
	// working correctly won't hurt anything
	void Start () 
	{
        //ResetAll ();

        player = GameObject.FindGameObjectWithTag("Player");
		playerWeapon = GameObject.FindGameObjectWithTag ("Weapon");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.GetComponent<PlayerCharacterHealth>().isDead)
		{
			playerIsDead = true;
			Debug.Log ("Player is now dead.");
		}

		if ((currentWepAmmo <= 0) && (currentWep != Weapons.HANDGUN))
		{
			SetWep(Weapons.HANDGUN, 0);
		}

		currentPickTimeRemaining -= (1.0f * Time.deltaTime);

		if ((currentPickTimeRemaining < 0.0f) && (currentPick != Pickups.NONE))
		{
			ClearPick ();
		}


        if (Input.GetKeyDown(KeyCode.B))
        {
            SetWep(Weapons.MACHINEGUN, 500);
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetWep(Weapons.SHOTGUN, 100);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            player.GetComponent<PlayerCharacterHealth>().GiveHealth(50.0f);
        }

		//Debug.Log(currentWep);
	}
}
