using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSounds : MonoBehaviour 
{

	PlayerCharacterManager playerManagerReference; 


	void Start ()
	{
		GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag ("PlayerManager");
		playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager> ();

	}


	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			if (playerManagerReference.GetPick () == Pickups.SPEED) 
			{
				AkSoundEngine.PostEvent ("SpeedBoost", gameObject);
			}

			else if (playerManagerReference.GetPick () == Pickups.DAMAGEBOOST) 
			{
				AkSoundEngine.PostEvent ("DamageBoost", gameObject);
			}

			else if (playerManagerReference.GetPick () == Pickups.HEALTHSMALL) 
			{
				AkSoundEngine.PostEvent ("HealthSmall", gameObject);
			}

			else if (playerManagerReference.GetPick () == Pickups.HEALTHLARGE) 
			{
				AkSoundEngine.PostEvent ("HealthLarge", gameObject);
			}
		}
	}
		
}
