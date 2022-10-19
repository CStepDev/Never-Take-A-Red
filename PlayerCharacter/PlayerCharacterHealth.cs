// Author  : Curtis Stephenson & George Pouros
// Date    : 26/02/2018
// Purpose : The class used for managing the player's health within the game, separated into a different .cs file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCharacterHealth : MonoBehaviour 
{
	public float CurrentHealth;
	public float MaxHealth;

	public bool isDead = false;

	public Slider healthbar;

	void Start () 
	{
		MaxHealth = 100.0f;
		CurrentHealth = MaxHealth;

		healthbar.value = CalculateHealth();
	}

	// A function added to give health, since sounds are associated with DealDamage so I can't use it for
	// both purposes when writing in giving and taking health
	public void GiveHealth (float healthValue)
	{
		CurrentHealth += healthValue;

		if (CurrentHealth > MaxHealth)
		{
			CurrentHealth = MaxHealth;
		}

		healthbar.value = CalculateHealth ();
	}


	public void DealDamage(float damageValue)
	{
		CurrentHealth -= damageValue;
		healthbar.value = CalculateHealth();
		AkSoundEngine.PostEvent ("GettingHit", gameObject);

		if (CurrentHealth <= 0)
		{
			AkSoundEngine.PostEvent ("DyingSound", gameObject);
			Die();
		}
	}

	float CalculateHealth()
	{
		return CurrentHealth / MaxHealth;
	}


	void Die()
	{
		isDead = true;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
