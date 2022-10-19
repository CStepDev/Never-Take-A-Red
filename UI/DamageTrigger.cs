using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour 
{
	float DmgPoints = 5f; 
	private PlayerCharacterHealth otherScriptToAccess;

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			other.gameObject.GetComponent<PlayerCharacterHealth> ().DealDamage (DmgPoints);
		}
	}
}
